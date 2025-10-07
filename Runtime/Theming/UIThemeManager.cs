using Aetheriaum.UISystem.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Theming
{
    public class UIThemeManager
    {
        private UIThemeData _currentTheme;
        private readonly Dictionary<string, UIThemeData> _themes = new();

        public UIThemeData CurrentTheme => _currentTheme;
        public event Action<UIThemeData> ThemeChanged;

        public void RegisterTheme(string name, UIThemeData theme)
        {
            _themes[name] = theme;
        }

        public void ApplyTheme(string themeName)
        {
            if (_themes.TryGetValue(themeName, out var theme))
            {
                _currentTheme = theme;
                ApplyThemeToRoot();
                ThemeChanged?.Invoke(theme);
            }
        }

        private void ApplyThemeToRoot()
        {
            var root = UnityEngine.Object.FindObjectOfType<UIDocument>()?.rootVisualElement;
            if (root != null)
            {
                ApplyThemeToElement(root, _currentTheme);
            }
        }

        private void ApplyThemeToElement(VisualElement element, UIThemeData theme)
        {
            // Apply theme colors based on element classes and names
            if (element.ClassListContains("primary-button"))
            {
                element.style.backgroundColor = theme.PrimaryColor;
            }
            else if (element.ClassListContains("secondary-button"))
            {
                element.style.backgroundColor = theme.SecondaryColor;
            }
            else if (element.ClassListContains("health-bar"))
            {
                element.style.backgroundColor = theme.HealthColor;
            }
            else if (element.ClassListContains("energy-bar"))
            {
                element.style.backgroundColor = theme.EnergyColor;
            }

            // Recursively apply to children
            foreach (var child in element.Children())
            {
                ApplyThemeToElement(child, theme);
            }
        }
    }
}