using Aetheriaum.UISystem.Data;
using Aetheriaum.UISystem.Interfaces;
using System;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Panels
{
    public abstract class BaseUIPanel : IUIPanel
    {
        public abstract string PanelId { get; }
        public abstract UIState RequiredState { get; }
        public bool IsVisible { get; private set; }

        protected VisualElement _panelElement;
        protected VisualElement _rootContainer;

        public event Action<IUIPanel> PanelShown;
        public event Action<IUIPanel> PanelHidden;

        public virtual void Initialize(VisualElement root)
        {
            _rootContainer = root;
            _panelElement = CreatePanel();
            _rootContainer.Add(_panelElement);
            ApplyPanelStyling();
            Hide(); // Start hidden
        }

        public virtual void Show()
        {
            if (IsVisible) return;

            IsVisible = true;
            _panelElement.style.display = DisplayStyle.Flex;
            OnPanelShown();
            PanelShown?.Invoke(this);
        }

        public virtual void Hide()
        {
            if (!IsVisible) return;

            IsVisible = false;
            _panelElement.style.display = DisplayStyle.None;
            OnPanelHidden();
            PanelHidden?.Invoke(this);
        }

        public abstract void UpdatePanel();
        public abstract void OnInputReceived(UIInputData input);

        protected abstract VisualElement CreatePanel();
        protected abstract void ApplyPanelStyling();
        protected virtual void OnPanelShown() { }
        protected virtual void OnPanelHidden() { }
    }
}