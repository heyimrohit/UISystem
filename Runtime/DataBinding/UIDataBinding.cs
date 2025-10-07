using Aetheriaum.UISystem.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.DataBinding
{
    public class UIDataBinding
    {
        private readonly VisualElement _element;
        private readonly IUIDataContext _dataContext;
        private readonly Dictionary<string, Action<object>> _bindings = new();

        public UIDataBinding(VisualElement element, IUIDataContext dataContext)
        {
            _element = element;
            _dataContext = dataContext;
            _dataContext.PropertyChanged += OnPropertyChanged;
        }

        public void BindProperty(string propertyName, Action<object> updateAction)
        {
            _bindings[propertyName] = updateAction;

            // Initial update
            var value = _dataContext.GetProperty(propertyName);
            updateAction?.Invoke(value);
        }

        public void BindText(Label label, string propertyName, string format = null)
        {
            BindProperty(propertyName, value =>
            {
                var text = value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(format))
                {
                    text = string.Format(format, text);
                }
                label.text = text;
            });
        }

        public void BindVisibility(VisualElement element, string propertyName)
        {
            BindProperty(propertyName, value =>
            {
                var isVisible = value is bool boolValue ? boolValue : value != null;
                element.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
            });
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (_bindings.TryGetValue(propertyName, out var updateAction))
            {
                var value = _dataContext.GetProperty(propertyName);
                updateAction.Invoke(value);
            }
        }
    }
}