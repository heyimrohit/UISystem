using Aetheriaum.UISystem.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Panels
{
    public class SettingsPanel : BaseUIPanel
    {
        public override string PanelId => "settings";
        public override UIState RequiredState => UIState.SettingsMenu;

        protected override VisualElement CreatePanel()
        {
            var panel = new VisualElement { name = "settings-panel" };
            var titleLabel = new Label("Settings") { name = "settings-title" };
            panel.Add(titleLabel);
            return panel;
        }

        protected override void ApplyPanelStyling()
        {
            _panelElement.style.position = Position.Absolute;
            _panelElement.style.width = Length.Percent(60);
            _panelElement.style.height = Length.Percent(70);
            _panelElement.style.left = Length.Percent(20);
            _panelElement.style.top = Length.Percent(15);
            _panelElement.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        }

        public override void UpdatePanel() { }

        public override void OnInputReceived(UIInputData input)
        {
            if (input.CancelPressed)
            {
                Hide();
            }
        }
    }
}