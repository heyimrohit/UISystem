using Aetheriaum.InputSystem.Interfaces;
using Aetheriaum.UISystem.Data;
using Aetheriaum.UISystem.Interfaces;
using System;
using UnityEngine;

namespace Aetheriaum.UISystem.Context
{
    public class MenuUIContext : IUIContext
    {
        public InputContext Context => InputContext.UI;
        public bool IsActive { get; private set; }

        public event Action<IUIContext> ContextActivated;
        public event Action<IUIContext> ContextDeactivated;

        public void Activate()
        {
            if (IsActive) return;

            IsActive = true;
            Time.timeScale = 0f; // Pause game when in menu
            ContextActivated?.Invoke(this);
        }

        public void Deactivate()
        {
            if (!IsActive) return;

            IsActive = false;
            Time.timeScale = 1f; // Resume game
            ContextDeactivated?.Invoke(this);
        }

        public bool HandleInput(UIInputData input)
        {
            if (!IsActive) return false;

            // Handle menu-specific input
            if (input.CancelPressed)
            {
                // Handle back navigation
                return true;
            }

            return false;
        }
    }
}