using Aetheriaum.UISystem.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Panels
{
    public class MapPanel : BaseUIPanel
    {
        public override string PanelId => "map";
        public override UIState RequiredState => UIState.MapView;

        protected override VisualElement CreatePanel()
        {
            var panel = new VisualElement { name = "map-panel" };
            var titleLabel = new Label("Map") { name = "map-title" };
            panel.Add(titleLabel);
            return panel;
        }

        protected override void ApplyPanelStyling()
        {
            _panelElement.style.position = Position.Absolute;
            _panelElement.style.width = Length.Percent(90);
            _panelElement.style.height = Length.Percent(90);
            _panelElement.style.left = Length.Percent(5);
            _panelElement.style.top = Length.Percent(5);
            _panelElement.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.95f);
        }

        public override void UpdatePanel() { }

        public override void OnInputReceived(UIInputData input)
        {
            if (input.CancelPressed || input.MapPressed)
            {
                Hide();
            }
        }
    }
}