using Aetheriaum.UISystem.Data;
using Aetheriaum.UISystem.Interfaces;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.HUD
{
    public abstract class BaseHUDElement : IHUDElement
    {
        public abstract string ElementId { get; }
        public abstract HUDLayer Layer { get; }
        public bool IsActive { get; private set; } = true;

        protected VisualElement _element;
        protected GameStateData _lastGameState;

        public virtual void Initialize(VisualElement parent)
        {
            _element = CreateElement();
            parent.Add(_element);
            ApplyStyling();
        }

        public virtual void UpdateElement(float deltaTime)
        {
            if (!IsActive) return;
            OnUpdate(deltaTime);
        }

        public virtual void SetActive(bool active)
        {
            IsActive = active;
            _element.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public virtual void OnGameStateChanged(GameStateData state)
        {
            _lastGameState = state;
            OnGameStateUpdate(state);
        }

        protected abstract VisualElement CreateElement();
        protected abstract void ApplyStyling();
        protected abstract void OnUpdate(float deltaTime);
        protected abstract void OnGameStateUpdate(GameStateData state);
    }
}