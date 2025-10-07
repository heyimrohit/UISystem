using Aetheriaum.UISystem.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.HUD
{
    public class CombatHUDComponent : BaseHUDElement
    {
        public override string ElementId => "combat-hud";
        public override HUDLayer Layer => HUDLayer.Overlay;

        private VisualElement _combatContainer;
        private Label _comboCounter;
        private VisualElement _targetIndicator;
        private float _combatTimer;
        private bool _isInCombat;

        protected override VisualElement CreateElement()
        {
            var container = new VisualElement { name = "combat-hud-container" };

            _combatContainer = new VisualElement { name = "combat-container" };
            _comboCounter = new Label("0 Hits") { name = "combo-counter" };
            _targetIndicator = new VisualElement { name = "target-indicator" };

            _combatContainer.Add(_comboCounter);
            container.Add(_combatContainer);
            container.Add(_targetIndicator);

            return container;
        }

        protected override void ApplyStyling()
        {
            _element.style.width = Length.Percent(100);
            _element.style.height = Length.Percent(100);
            _element.style.position = Position.Absolute;

            _comboCounter.style.position = Position.Absolute;
            _comboCounter.style.right = 50;
            _comboCounter.style.top = 50;
            _comboCounter.style.fontSize = 24;
            _comboCounter.style.color = Color.yellow;
            _comboCounter.style.display = DisplayStyle.None;

            _targetIndicator.style.position = Position.Absolute;
            _targetIndicator.style.width = 60;
            _targetIndicator.style.height = 60;
            _targetIndicator.style.borderLeftColor = Color.red;
            _targetIndicator.style.borderTopColor = Color.red;
            _targetIndicator.style.borderRightColor = Color.red;
            _targetIndicator.style.borderBottomColor = Color.red;
            _targetIndicator.style.borderLeftWidth = 3;
            _targetIndicator.style.borderTopWidth = 3;
            _targetIndicator.style.borderRightWidth = 3;
            _targetIndicator.style.borderBottomWidth = 3;
            _targetIndicator.style.borderTopLeftRadius = 30;
            _targetIndicator.style.borderTopRightRadius = 30;
            _targetIndicator.style.borderBottomLeftRadius = 30;
            _targetIndicator.style.borderBottomRightRadius = 30;
            _targetIndicator.style.display = DisplayStyle.None;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (_isInCombat)
            {
                _combatTimer += deltaTime;

                // Hide combat UI after 5 seconds of no combat
                if (_combatTimer > 5f)
                {
                    SetCombatMode(false);
                }
            }
        }

        protected override void OnGameStateUpdate(GameStateData state)
        {
            if (state.IsInCombat && !_isInCombat)
            {
                SetCombatMode(true);
            }
            else if (!state.IsInCombat && _combatTimer > 3f)
            {
                SetCombatMode(false);
            }

            if (state.IsInCombat)
            {
                _combatTimer = 0f;
            }
        }

        private void SetCombatMode(bool inCombat)
        {
            _isInCombat = inCombat;
            _combatTimer = 0f;

            _comboCounter.style.display = inCombat ? DisplayStyle.Flex : DisplayStyle.None;
            // Target indicator would be shown when there's an active target
        }
    }
}
