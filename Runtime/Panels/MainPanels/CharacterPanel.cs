using Aetheriaum.UISystem.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Panels
{
    public class CharacterPanel : BaseUIPanel
    {
        public override string PanelId => "character";
        public override UIState RequiredState => UIState.CharacterMenu;

        protected override VisualElement CreatePanel()
        {
            var panel = new VisualElement { name = "character-panel" };
            var titleLabel = new Label("Character") { name = "character-title" };
            panel.Add(titleLabel);
            return panel;
        }

        protected override void ApplyPanelStyling()
        {
            _panelElement.style.position = Position.Absolute;
            _panelElement.style.width = Length.Percent(80);
            _panelElement.style.height = Length.Percent(80);
            _panelElement.style.left = Length.Percent(10);
            _panelElement.style.top = Length.Percent(10);
            _panelElement.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        }

        public override void UpdatePanel() { }

        public override void OnInputReceived(UIInputData input)
        {
            if (input.CancelPressed || input.CharacterPressed)
            {
                Hide();
            }
        }
    }
}
