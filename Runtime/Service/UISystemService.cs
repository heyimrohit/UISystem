using Aetheriaum.AnimationSystem.Interfaces;
using Aetheriaum.CharacterSystem.Interfaces;
using Aetheriaum.CoreSystem.Architecture.AbstractClasses;
using Aetheriaum.CoreSystem.Architecture.ServiceLocator;
using Aetheriaum.InputSystem.Interfaces;
using Aetheriaum.UISystem.Data;
using Aetheriaum.UISystem.HUD;
using Aetheriaum.UISystem.Interfaces;
using Aetheriaum.UISystem.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using ElementType = Aetheriaum.CharacterSystem.Interfaces.ElementType;

namespace Aetheriaum.UISystem.Service
{
    /// <summary>
    /// Central UI system service managing all UI contexts, panels, and HUD elements.
    /// Integrates with Input, Character, and Animation systems for a complete UI experience.
    /// </summary>
    public class UISystemService : BaseUpdatableService, IUISystem
    {
        //public override int Priority => 30; // After Input (10), before Character systems (20)
        public override string ServiceName => "UISystem";
        public override int InitializationPriority => 30;

        private readonly bool _enableDebug = false;

        private UIState _currentState = UIState.Hidden;
        private bool _isUIVisible = false;
        private readonly Dictionary<Type, IUIPanel> _registeredPanels = new();
        private readonly List<IHUDElement> _hudElements = new();
        private readonly Dictionary<InputContext, IUIContext> _uiContexts = new();

        private VisualElement _rootElement;
        private VisualElement _hudContainer;
        private VisualElement _panelContainer;
        private VisualElement _overlayContainer;

        // Dependencies
        private IInputSystem _inputSystem;
        private ICharacterSystem _characterSystem;
        private IAnimationSystem _animationSystem;

        public UIState CurrentState => _currentState;
        public bool IsUIVisible => _isUIVisible;

        public event Action<UIState> StateChanged;
        public event Action<bool> VisibilityChanged;

        protected override Task OnInitializeAsync()
        {
            //await base.OnInitializeAsync();

            // Get dependencies
            _inputSystem = ServiceLocator.Instance.TryGetService<IInputSystem>();
            _characterSystem = ServiceLocator.Instance.TryGetService<ICharacterSystem>();
            _animationSystem = ServiceLocator.Instance.TryGetService<IAnimationSystem>();

            // Initialize UI root
            InitializeUIRoot();

            // Register default panels
            RegisterDefaultPanels();

            // Initialize HUD elements
            InitializeHUDElements();

            // Subscribe to system events
            SubscribeToSystemEvents();

            LogInfo("UI System initialized successfully");
            return Task.CompletedTask;
        }

        protected override void OnUpdate()
        {
            // Update HUD elements
            foreach (var hudElement in _hudElements)
            {
                if (hudElement.IsActive)
                {
                    hudElement.UpdateElement(Time.deltaTime);
                }
            }

            // Handle UI input
            HandleUIInput();

            // Update active panels
            UpdateActivePanels();
        }

        protected override async Task OnShutdownAsync()
        {
            // Unsubscribe from events
            if (_characterSystem != null)
            {
                _characterSystem.ActiveCharacterChanged -= OnActiveCharacterChanged;
                _characterSystem.CharacterLevelChanged -= OnLevelChanged;

                // Unsubscribe from active character stats
                UnsubscribeFromActiveCharacterStats();
            }

            if (_inputSystem != null)
            {
                _inputSystem.ContextChanged -= OnInputContextChanged;
            }

            // Cleanup UI elements
            foreach (var element in _hudElements)
            {
                if (element is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _hudElements.Clear();
            _registeredPanels.Clear();

            await Task.CompletedTask;
        }

        public void SetUIState(UIState state)
        {
            if (_currentState == state) return;

            var previousState = _currentState;
            _currentState = state;

            TransitionToState(previousState, state);
            StateChanged?.Invoke(state);

            LogDebug($"UI State changed: {previousState} -> {state}");
        }

        public void ShowHUD()
        {
            _isUIVisible = true;
            if (_hudContainer != null)
                _hudContainer.style.display = DisplayStyle.Flex;
            SetUIState(UIState.GameplayHUD);
            VisibilityChanged?.Invoke(true);
        }

        public void HideHUD()
        {
            _isUIVisible = false;
            if (_hudContainer != null)
                _hudContainer.style.display = DisplayStyle.None;
            SetUIState(UIState.Hidden);
            VisibilityChanged?.Invoke(false);
        }

        public void ShowPanel<T>() where T : class, IUIPanel
        {
            if (_registeredPanels.TryGetValue(typeof(T), out var panel))
            {
                panel.Show();
                SetUIState(panel.RequiredState);
            }
            else
            {
                LogWarning($"Panel of type {typeof(T).Name} is not registered");
            }
        }

        public void HidePanel<T>() where T : class, IUIPanel
        {
            if (_registeredPanels.TryGetValue(typeof(T), out var panel))
            {
                panel.Hide();
            }
        }

        public T GetPanel<T>() where T : class, IUIPanel
        {
            return _registeredPanels.TryGetValue(typeof(T), out var panel) ? panel as T : null;
        }

        public void RegisterPanel<T>(T panel) where T : class, IUIPanel
        {
            _registeredPanels[typeof(T)] = panel;
            if (_panelContainer != null)
                panel.Initialize(_panelContainer);
            LogDebug($"Registered panel: {typeof(T).Name}");
        }

        private void InitializeUIRoot()
        {
            try
            {
                var uiDocument = UnityEngine.Object.FindObjectOfType<UIDocument>();
                if (uiDocument == null)
                {
                    var uiGameObject = new GameObject("UI Root");
                    uiDocument = uiGameObject.AddComponent<UIDocument>();

                    // Load default UI template
                    var visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/UIRoot");
                    if (visualTreeAsset != null)
                    {
                        uiDocument.visualTreeAsset = visualTreeAsset;
                    }
                }

                _rootElement = uiDocument.rootVisualElement;

                // Create main containers
                _hudContainer = new VisualElement { name = "hud-container" };
                _panelContainer = new VisualElement { name = "panel-container" };
                _overlayContainer = new VisualElement { name = "overlay-container" };

                _rootElement.Add(_hudContainer);
                _rootElement.Add(_panelContainer);
                _rootElement.Add(_overlayContainer);

                // Apply styling
                ApplyRootStyling();

                LogDebug("UI Root initialized successfully");
            }
            catch (Exception ex)
            {
                LogError($"Failed to initialize UI Root: {ex.Message}");
                // Create minimal fallback UI structure
                CreateFallbackUIStructure();
            }
        }

        private void CreateFallbackUIStructure()
        {
            var uiGameObject = new GameObject("UI Root (Fallback)");
            var uiDocument = uiGameObject.AddComponent<UIDocument>();

            _rootElement = uiDocument.rootVisualElement;
            _hudContainer = new VisualElement { name = "hud-container" };
            _panelContainer = new VisualElement { name = "panel-container" };
            _overlayContainer = new VisualElement { name = "overlay-container" };

            _rootElement.Add(_hudContainer);
            _rootElement.Add(_panelContainer);
            _rootElement.Add(_overlayContainer);

            ApplyRootStyling();
            LogDebug("Fallback UI structure created");
        }

        private void RegisterDefaultPanels()
        {
            try
            {
                // Register core panels
                RegisterPanel(new InventoryPanel());
                RegisterPanel(new CharacterPanel());
                RegisterPanel(new SettingsPanel());
                RegisterPanel(new MapPanel());
                RegisterPanel(new PausePanel());

                LogDebug("Default panels registered successfully");
            }
            catch (Exception ex)
            {
                LogError($"Failed to register default panels: {ex.Message}");
            }
        }

        private void InitializeHUDElements()
        {
            try
            {
                if (_hudContainer == null)
                {
                    LogWarning("HUD container not available, skipping HUD element initialization");
                    return;
                }

                // Create and register HUD elements
                var healthBar = new HealthBarComponent();
                var energyBar = new EnergyBarComponent();
                var staminaBar = new StaminaBarComponent();
                var skillCooldownHUD = new SkillCooldownHUD();
                var miniMap = new MiniMapComponent();
                var statusEffectDisplay = new StatusEffectDisplay();
                var combatHUD = new CombatHUDComponent();

                _hudElements.AddRange(new IHUDElement[]
                {
                    healthBar, energyBar, staminaBar, skillCooldownHUD,
                    miniMap, statusEffectDisplay, combatHUD
                });

                // Initialize each element
                foreach (var element in _hudElements)
                {
                    element.Initialize(_hudContainer);
                }

                LogDebug("HUD elements initialized successfully");
            }
            catch (Exception ex)
            {
                LogError($"Failed to initialize HUD elements: {ex.Message}");
            }
        }

        private void SubscribeToSystemEvents()
        {
            try
            {
                if (_characterSystem != null)
                {
                    _characterSystem.ActiveCharacterChanged += OnActiveCharacterChanged;
                    _characterSystem.CharacterLevelChanged += OnLevelChanged;
                    LogDebug("Subscribed to character system events");

                    // Subscribe to active character's stats events if available
                    SubscribeToActiveCharacterStats();
                }
                else
                {
                    LogWarning("Character system not available for event subscription");
                }

                if (_inputSystem != null)
                {
                    _inputSystem.ContextChanged += OnInputContextChanged;
                    LogDebug("Subscribed to input system events");
                }
                else
                {
                    LogWarning("Input system not available for event subscription");
                }
            }
            catch (Exception ex)
            {
                LogError($"Failed to subscribe to system events: {ex.Message}");
            }
        }

        private void SubscribeToActiveCharacterStats()
        {
            try
            {
                var activeCharacter = _characterSystem?.ActiveCharacter;
                if (activeCharacter?.Stats != null)
                {
                    // Subscribe to the character's stats changed event
                    activeCharacter.Stats.StatsChanged += OnCharacterStatsChanged;
                    LogDebug($"Subscribed to stats for active character: {activeCharacter.CharacterId}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Failed to subscribe to character stats: {ex.Message}");
            }
        }

        private void UnsubscribeFromActiveCharacterStats()
        {
            try
            {
                var activeCharacter = _characterSystem?.ActiveCharacter;
                if (activeCharacter?.Stats != null)
                {
                    activeCharacter.Stats.StatsChanged -= OnCharacterStatsChanged;
                }
            }
            catch (Exception ex)
            {
                LogError($"Failed to unsubscribe from character stats: {ex.Message}");
            }
        }

        private void HandleUIInput()
        {
            try
            {
                if (_inputSystem?.CurrentContext != InputContext.UI) return;

                var inputData = GetCurrentUIInput();

                // Handle global UI shortcuts
                if (inputData.MenuPressed)
                {
                    TogglePauseMenu();
                }
                else if (inputData.InventoryPressed)
                {
                    ToggleInventory();
                }
                else if (inputData.CharacterPressed)
                {
                    ToggleCharacterMenu();
                }
                else if (inputData.MapPressed)
                {
                    ToggleMap();
                }

                // Pass input to active panel
                var activePanel = GetActivePanelForCurrentState();
                activePanel?.OnInputReceived(inputData);
            }
            catch (Exception ex)
            {
                LogError($"Error handling UI input: {ex.Message}");
            }
        }

        private UIInputData GetCurrentUIInput()
        {
            // This integrates with your actual input system
            return new UIInputData
            {
                NavigationInput = Vector2.zero,
                ConfirmPressed = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space),
                CancelPressed = Input.GetKeyDown(KeyCode.Escape),
                MenuPressed = Input.GetKeyDown(KeyCode.Escape),
                InventoryPressed = Input.GetKeyDown(KeyCode.I),
                CharacterPressed = Input.GetKeyDown(KeyCode.C),
                MapPressed = Input.GetKeyDown(KeyCode.M),
                PointerPosition = Input.mousePosition,
                PointerPressed = Input.GetMouseButtonDown(0)
            };
        }

        private void TransitionToState(UIState from, UIState to)
        {
            try
            {
                // Hide panels from previous state
                HidePanelsForState(from);

                // Show panels for new state
                ShowPanelsForState(to);

                // Update input context
                UpdateInputContextForState(to);
            }
            catch (Exception ex)
            {
                LogError($"Error transitioning UI state from {from} to {to}: {ex.Message}");
            }
        }

        private void HidePanelsForState(UIState state)
        {
            foreach (var panel in _registeredPanels.Values)
            {
                if (panel.RequiredState == state && panel.IsVisible)
                {
                    panel.Hide();
                }
            }
        }

        private void ShowPanelsForState(UIState state)
        {
            foreach (var panel in _registeredPanels.Values)
            {
                if (panel.RequiredState == state)
                {
                    panel.Show();
                    break; // Only show one panel per state
                }
            }
        }

        private void UpdateInputContextForState(UIState state)
        {
            if (_inputSystem == null) return;

            switch (state)
            {
                case UIState.GameplayHUD:
                case UIState.CombatFocused:
                    _inputSystem.SetContext(InputContext.Gameplay);
                    break;
                default:
                    _inputSystem.SetContext(InputContext.UI);
                    break;
            }
        }

        private void UpdateActivePanels()
        {
            try
            {
                foreach (var panel in _registeredPanels.Values)
                {
                    if (panel.IsVisible)
                    {
                        panel.UpdatePanel();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error updating active panels: {ex.Message}");
            }
        }

        private IUIPanel GetActivePanelForCurrentState()
        {
            return _registeredPanels.Values.FirstOrDefault(p => p.RequiredState == _currentState && p.IsVisible);
        }

        private void TogglePauseMenu()
        {
            if (_currentState == UIState.PauseMenu)
            {
                SetUIState(UIState.GameplayHUD);
            }
            else
            {
                ShowPanel<PausePanel>();
            }
        }

        private void ToggleInventory()
        {
            if (_currentState == UIState.InventoryMenu)
            {
                SetUIState(UIState.GameplayHUD);
            }
            else
            {
                ShowPanel<InventoryPanel>();
            }
        }

        private void ToggleCharacterMenu()
        {
            if (_currentState == UIState.CharacterMenu)
            {
                SetUIState(UIState.GameplayHUD);
            }
            else
            {
                ShowPanel<CharacterPanel>();
            }
        }

        private void ToggleMap()
        {
            if (_currentState == UIState.MapView)
            {
                SetUIState(UIState.GameplayHUD);
            }
            else
            {
                ShowPanel<MapPanel>();
            }
        }

        private void ApplyRootStyling()
        {
            if (_rootElement == null) return;

            _rootElement.style.width = Length.Percent(100);
            _rootElement.style.height = Length.Percent(100);
            _rootElement.style.position = Position.Absolute;

            if (_hudContainer != null)
            {
                _hudContainer.style.width = Length.Percent(100);
                _hudContainer.style.height = Length.Percent(100);
                _hudContainer.style.position = Position.Absolute;
            }

            if (_panelContainer != null)
            {
                _panelContainer.style.width = Length.Percent(100);
                _panelContainer.style.height = Length.Percent(100);
                _panelContainer.style.position = Position.Absolute;
            }

            if (_overlayContainer != null)
            {
                _overlayContainer.style.width = Length.Percent(100);
                _overlayContainer.style.height = Length.Percent(100);
                _overlayContainer.style.position = Position.Absolute;
            }
        }

        // Event Handlers
        private void OnActiveCharacterChanged(ICharacter oldCharacter, ICharacter newCharacter)
        {
            try
            {
                // Unsubscribe from old character's stats
                if (oldCharacter?.Stats != null)
                {
                    oldCharacter.Stats.StatsChanged -= OnCharacterStatsChanged;
                }

                // Subscribe to new character's stats
                if (newCharacter?.Stats != null)
                {
                    newCharacter.Stats.StatsChanged += OnCharacterStatsChanged;
                }

                var gameState = CreateGameStateFromCharacter(newCharacter);
                UpdateHUDElements(gameState);
                LogDebug($"Active character changed: {oldCharacter?.CharacterId ?? "None"} -> {newCharacter?.CharacterId ?? "None"}");
            }
            catch (Exception ex)
            {
                LogError($"Error handling active character change: {ex.Message}");
            }
        }

        private void OnCharacterStatsChanged(ICharacterStats stats)
        {
            try
            {
                // When character stats change (health, energy, etc.), update the HUD
                var activeCharacter = _characterSystem?.ActiveCharacter;
                if (activeCharacter != null)
                {
                    var gameState = CreateGameStateFromCharacter(activeCharacter);
                    UpdateHUDElements(gameState);
                    LogDebug($"Character stats changed for {activeCharacter.CharacterId}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error handling character stats change: {ex.Message}");
            }
        }

        private void OnLevelChanged(ICharacter character, int oldLevel, int newLevel)
        {
            try
            {
                var gameState = new GameStateData { Level = newLevel };
                UpdateHUDElements(gameState);
                LogDebug($"Character {character?.CharacterId} level changed: {oldLevel} -> {newLevel}");
            }
            catch (Exception ex)
            {
                LogError($"Error handling level change: {ex.Message}");
            }
        }

        private void OnInputContextChanged(InputContext oldContext, InputContext newContext)
        {
            try
            {
                // Update UI visibility based on input context
                if (newContext == InputContext.UI && !_isUIVisible)
                {
                    ShowHUD();
                }
                LogDebug($"Input context changed: {oldContext} -> {newContext}");
            }
            catch (Exception ex)
            {
                LogError($"Error handling input context change: {ex.Message}");
            }
        }

        private void UpdateHUDElements(GameStateData gameState)
        {
            try
            {
                foreach (var element in _hudElements)
                {
                    if (element.IsActive)
                    {
                        element.OnGameStateChanged(gameState);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error updating HUD elements: {ex.Message}");
            }
        }

        private GameStateData CreateGameStateFromCharacter(ICharacter character)
        {
            if (character == null)
            {
                return new GameStateData
                {
                    Health = 0f,
                    MaxHealth = 100f,
                    Energy = 0f,
                    MaxEnergy = 100f,
                    Level = 1,
                    ActiveCharacterName = "None"
                };
            }

            try
            {
                // Use your existing character stats structure
                var stats = character.Stats;
                var progression = character.Progression;
                var characterData = character.CharacterData;

                return new GameStateData
                {
                    Health = stats?.CurrentHP ?? 0f,
                    MaxHealth = stats?.MaxHP ?? (characterData?.BaseStats?.BaseHP ?? 100f),
                    Energy = stats?.CurrentEnergy ?? 0f,
                    MaxEnergy = stats?.MaxEnergy ?? 100f,
                    Stamina = 100f, // Add stamina if you implement it
                    MaxStamina = 100f,
                    Level = progression?.Level ?? 1,
                    Experience = (int)(progression?.Experience ?? 0),
                    MaxExperience = 1000, // You can calculate this based on your progression system
                    ActiveCharacterName = characterData?.DisplayName ?? characterData?.CharacterName ?? character.CharacterId,
                    ActiveElement = (Data.ElementType)(characterData?.Element ?? ElementType.None),
                    StatusEffects = new List<StatusEffect>(), // You can populate this from your character system
                    WorldPosition = character.Transform?.position ?? Vector3.zero,
                    CombatTime = 0f, // You can track this in your combat system
                    IsInCombat = false // You can determine this from your combat state
                };
            }
            catch (Exception ex)
            {
                LogError($"Error creating game state from character: {ex.Message}");
                // Return safe default values using your character's base stats if available
                var fallbackHealth = character.CharacterData?.BaseStats?.BaseHP ?? 100f;
                return new GameStateData
                {
                    Health = fallbackHealth,
                    MaxHealth = fallbackHealth,
                    Energy = 100f,
                    MaxEnergy = 100f,
                    Level = 1,
                    ActiveCharacterName = character.CharacterId ?? "Unknown",
                    ActiveElement = (Data.ElementType)(character.CharacterData?.Element ?? ElementType.None)
                };
            }
        }

        #region Logging Methods

        private void LogInfo(string message)
        {
            Debug.Log($"[{ServiceName}] {message}");
        }

        private void LogDebug(string message)
        {
            if (_enableDebug)
            {
            Debug.Log($"[{ServiceName}] {message}");
            }
        }

        private void LogWarning(string message)
        {
            Debug.LogWarning($"[{ServiceName}] {message}");
        }

        private void LogError(string message)
        {
            Debug.LogError($"[{ServiceName}] {message}");
        }

        #endregion

        #region Public Debug Methods

        [ContextMenu("Debug UI System Status")]
        public void DebugUISystemStatus()
        {
            Debug.Log($"=== UI SYSTEM STATUS ===");
            Debug.Log($"Current State: {_currentState}");
            Debug.Log($"UI Visible: {_isUIVisible}");
            Debug.Log($"Registered Panels: {_registeredPanels.Count}");
            Debug.Log($"HUD Elements: {_hudElements.Count}");
            Debug.Log($"Dependencies: Input={_inputSystem != null}, Character={_characterSystem != null}, Animation={_animationSystem != null}");
        }

        [ContextMenu("Show Test HUD")]
        public void ShowTestHUD()
        {
            ShowHUD();
            Debug.Log("Test HUD shown");
        }

        [ContextMenu("Hide Test HUD")]
        public void HideTestHUD()
        {
            HideHUD();
            Debug.Log("Test HUD hidden");
        }

        #endregion
    }
}