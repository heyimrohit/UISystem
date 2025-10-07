using Aetheriaum.UISystem.Data;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.HUD
{
    public class HealthBarComponent : BaseHUDElement
    {
        public override string ElementId => "health-bar";
        public override HUDLayer Layer => HUDLayer.UI;

        private VisualElement _healthBarFill;
        private Label _healthText;
        private float _targetHealthPercentage;
        private float _currentHealthPercentage;

        protected override VisualElement CreateElement()
        {
            var container = new VisualElement { name = "health-bar-container" };

            var healthBarBackground = new VisualElement { name = "health-bar-background" };
            _healthBarFill = new VisualElement { name = "health-bar-fill" };
            _healthText = new Label("100/100") { name = "health-text" };

            healthBarBackground.Add(_healthBarFill);
            container.Add(healthBarBackground);
            container.Add(_healthText);

            return container;
        }

        protected override void ApplyStyling()
        {
            _element.style.position = Position.Absolute;
            _element.style.left = 20;
            _element.style.top = 20;
            _element.style.width = 300;
            _element.style.height = 40;

            var background = _element.Q("health-bar-background");
            background.style.width = Length.Percent(100);
            background.style.height = 20;
            background.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            background.style.borderTopLeftRadius = 10;
            background.style.borderTopRightRadius = 10;
            background.style.borderBottomLeftRadius = 10;
            background.style.borderBottomRightRadius = 10;

            _healthBarFill.style.width = Length.Percent(100);
            _healthBarFill.style.height = Length.Percent(100);
            _healthBarFill.style.backgroundColor = new Color(0.2f, 0.8f, 0.2f);
            _healthBarFill.style.borderTopLeftRadius = 10;
            _healthBarFill.style.borderBottomLeftRadius = 10;

            _healthText.style.position = Position.Absolute;
            _healthText.style.left = 0;
            _healthText.style.top = 25;
            _healthText.style.width = Length.Percent(100);
            _healthText.style.unityTextAlign = TextAnchor.MiddleCenter;
            _healthText.style.color = Color.white;
            _healthText.style.fontSize = 14;
        }

        protected override void OnUpdate(float deltaTime)
        {
            // Smooth health bar animation
            if (Math.Abs(_currentHealthPercentage - _targetHealthPercentage) > 0.01f)
            {
                _currentHealthPercentage = Mathf.Lerp(_currentHealthPercentage, _targetHealthPercentage, deltaTime * 5f);
                _healthBarFill.style.width = Length.Percent(_currentHealthPercentage * 100);
            }
        }

        protected override void OnGameStateUpdate(GameStateData state)
        {
            _targetHealthPercentage = state.MaxHealth > 0 ? state.Health / state.MaxHealth : 0f;
            _healthText.text = $"{(int)state.Health}/{(int)state.MaxHealth}";

            // Change color based on health percentage
            var healthPercent = _targetHealthPercentage;
            Color healthColor;

            if (healthPercent > 0.6f)
                healthColor = Color.green;
            else if (healthPercent > 0.3f)
                healthColor = Color.yellow;
            else
                healthColor = Color.red;

            _healthBarFill.style.backgroundColor = healthColor;
        }
    }
}