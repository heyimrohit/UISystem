using Aetheriaum.InputSystem.Interfaces;
using Aetheriaum.UISystem.Data;
using System;

namespace Aetheriaum.UISystem.Interfaces
{
    public interface IUIContext
    {
        InputContext Context { get; }
        bool IsActive { get; }

        void Activate();
        void Deactivate();
        bool HandleInput(UIInputData input);

        event Action<IUIContext> ContextActivated;
        event Action<IUIContext> ContextDeactivated;
    }
}