using Aetheriaum.InputSystem.Interfaces;
using Aetheriaum.UISystem.Data;
using Aetheriaum.UISystem.Interfaces;
using System.Collections.Generic;

namespace Aetheriaum.UISystem.Context
{
    public class UIContextManager
    {
        private readonly Dictionary<InputContext, IUIContext> _contexts = new();
        private IUIContext _activeContext;

        public IUIContext ActiveContext => _activeContext;

        public void RegisterContext(IUIContext context)
        {
            _contexts[context.Context] = context;
            context.ContextActivated += OnContextActivated;
            context.ContextDeactivated += OnContextDeactivated;
        }

        public void ActivateContext(InputContext contextType)
        {
            if (_contexts.TryGetValue(contextType, out var context))
            {
                _activeContext?.Deactivate();
                _activeContext = context;
                context.Activate();
            }
        }

        public bool HandleInput(UIInputData input)
        {
            return _activeContext?.HandleInput(input) ?? false;
        }

        private void OnContextActivated(IUIContext context)
        {
            // Handle context activation
        }

        private void OnContextDeactivated(IUIContext context)
        {
            // Handle context deactivation
        }
    }

}