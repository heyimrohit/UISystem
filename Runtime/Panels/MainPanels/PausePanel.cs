using Aetheriaum.UISystem.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Panels
{
    public class PausePanel : BaseUIPanel
    {
        public override string PanelId => "pause";
        public override UIState RequiredState => UIState.PauseMenu;

        protected override VisualElement CreatePanel()
        {
            var panel = new VisualElement { name = "pause-panel" };
            var titleLabel = new Label("Game Paused") { name = "pause-title" };

            var buttonContainer = new VisualElement { name = "pause-buttons" };
            var resumeButton = new Button(() => Hide()) { text = "Resume" };
            var settingsButton = new Button(() => { /* Open settings */ }) { text = "Settings" };
            var quitButton = new Button(() => { /* Quit game */ }) { text = "Quit" };

            buttonContainer.Add(resumeButton);
            buttonContainer.Add(settingsButton);
            buttonContainer.Add(quitButton);

            panel.Add(titleLabel);
            panel.Add(buttonContainer);
            return panel;
        }

        protected override void ApplyPanelStyling()
        {
            _panelElement.style.position = Position.Absolute;
            _panelElement.style.width = Length.Percent(40);
            _panelElement.style.height = Length.Percent(50);
            _panelElement.style.left = Length.Percent(30);
            _panelElement.style.top = Length.Percent(25);
            _panelElement.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            _panelElement.style.alignItems = Align.Center;
            _panelElement.style.justifyContent = Justify.Center;
        }

        public override void UpdatePanel() { }

        public override void OnInputReceived(UIInputData input)
        {
            if (input.CancelPressed || input.MenuPressed)
            {
                Hide();
            }
        }
    }
}
