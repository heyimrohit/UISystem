using Aetheriaum.UISystem.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Theming
{
    public static class UIThemeExtensions
    {
        public static void ApplyElementalTheme(this VisualElement element, ElementType elementType, UIThemeData theme)
        {
            if (theme.ElementColors.TryGetValue(elementType, out var color))
            {
                element.style.backgroundColor = color;
                element.style.borderLeftColor = color;
                element.style.borderTopColor = color;
                element.style.borderRightColor = color;
                element.style.borderBottomColor = color;
            }
        }

        public static void SetHealthBarColors(this VisualElement healthBar, float healthPercentage, UIThemeData theme)
        {
            Color healthColor;
            if (healthPercentage > 0.6f)
                healthColor = theme.HealthColor;
            else if (healthPercentage > 0.3f)
                healthColor = Color.Lerp(Color.red, Color.yellow, (healthPercentage - 0.3f) / 0.3f);
            else
                healthColor = Color.red;

            healthBar.style.backgroundColor = healthColor;
        }
    }
}