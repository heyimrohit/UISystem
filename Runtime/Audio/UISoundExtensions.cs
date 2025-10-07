using Aetheriaum.UISystem.Interfaces;
using Aetheriaum.UISystem.Service;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Audio
{
    public static class UISoundExtensions
    {
        private static UISoundManager _soundManager;

        public static void InitializeSounds(this UISystemService uiSystem, UISoundManager soundManager)
        {
            _soundManager = soundManager;
        }

        public static void PlayClickSound(this Button button)
        {
            button.clicked += () => _soundManager?.PlaySound("button_click");
        }

        public static void PlayOpenSound(this IUIPanel panel)
        {
            panel.PanelShown += _ => _soundManager?.PlaySound("panel_open");
        }

        public static void PlayCloseSound(this IUIPanel panel)
        {
            panel.PanelHidden += _ => _soundManager?.PlaySound("panel_close");
        }
    }
}