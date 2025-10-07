using Aetheriaum.CoreSystem.Architecture.Interface;
using Aetheriaum.UISystem.Data;
using System;

namespace Aetheriaum.UISystem.Interfaces
{
    public interface IUISystem : IService
    {
        UIState CurrentState { get; }
        bool IsUIVisible { get; }

        void SetUIState(UIState state);
        void ShowHUD();
        void HideHUD();
        void ShowPanel<T>() where T : class, IUIPanel;
        void HidePanel<T>() where T : class, IUIPanel;
        T GetPanel<T>() where T : class, IUIPanel;
        void RegisterPanel<T>(T panel) where T : class, IUIPanel;

        event Action<UIState> StateChanged;
        event Action<bool> VisibilityChanged;
    }
}