using Aetheriaum.InputSystem.Interfaces;
using Aetheriaum.UISystem.Data;
using Aetheriaum.UISystem.Interfaces;
using System;

namespace Aetheriaum.UISystem.Context
{
    public class GameplayUIContext : IUIContext
    {
        public InputContext Context => InputContext.Gameplay;
        public bool IsActive { get; private set; }

        public event Action<IUIContext> ContextActivated;
        public event Action<IUIContext> ContextDeactivated;

        public void Activate()
        {
            if (IsActive) return;

            IsActive = true;
            ContextActivated?.Invoke(this);
        }

        public void Deactivate()
        {
            if (!IsActive) return;

            IsActive = false;
            ContextDeactivated?.Invoke(this);
        }

        public bool HandleInput(UIInputData input)
        {
            if (!IsActive) return false;

            // Handle gameplay UI input (quick actions, etc.)
            return false; // Allow input to pass through to gameplay
        }
    }
}