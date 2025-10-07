using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Accessibility
{
    public class UIAccessibilityManager
    {
        private bool _highContrastMode;
        private float _textScale = 1f;
        private bool _colorBlindMode;

        public bool HighContrastMode
        {
            get => _highContrastMode;
            set
            {
                _highContrastMode = value;
                ApplyHighContrast(value);
            }
        }

        public float TextScale
        {
            get => _textScale;
            set
            {
                _textScale = Mathf.Clamp(value, 0.5f, 2f);
                ApplyTextScaling(_textScale);
            }
        }

        public bool ColorBlindMode
        {
            get => _colorBlindMode;
            set
            {
                _colorBlindMode = value;
                ApplyColorBlindSupport(value);
            }
        }

        private void ApplyHighContrast(bool enabled)
        {
            var root = GameObject.FindObjectOfType<UIDocument>()?.rootVisualElement;
            if (root != null)
            {
                if (enabled)
                {
                    root.AddToClassList("high-contrast");
                }
                else
                {
                    root.RemoveFromClassList("high-contrast");
                }
            }
        }

        private void ApplyTextScaling(float scale)
        {
            var root = GameObject.FindObjectOfType<UIDocument>()?.rootVisualElement;
            if (root != null)
            {
                ApplyTextScaleToElement(root, scale);
            }
        }

        private void ApplyTextScaleToElement(VisualElement element, float scale)
        {
            if (element is Label label)
            {
                var currentSize = label.style.fontSize.value.value;
                label.style.fontSize = currentSize * scale;
            }

            foreach (var child in element.Children())
            {
                ApplyTextScaleToElement(child, scale);
            }
        }

        private void ApplyColorBlindSupport(bool enabled)
        {
            // Apply colorblind-friendly color schemes
            // This would modify the theme colors to be more distinguishable
        }
    }
}