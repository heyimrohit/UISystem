
using Aetheriaum.UISystem.Data;
using System;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Interfaces
{
    public interface IUIPanel
    {
        string PanelId { get; }
        bool IsVisible { get; }
        UIState RequiredState { get; }

        void Show();
        void Hide();
        void Initialize(VisualElement root);
        void UpdatePanel();
        void OnInputReceived(UIInputData input);

        event Action<IUIPanel> PanelShown;
        event Action<IUIPanel> PanelHidden;
    }
}