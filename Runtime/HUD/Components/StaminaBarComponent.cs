using Aetheriaum.UISystem.Data;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.HUD
{
    public class StaminaBarComponent : BaseHUDElement
    {
        public override string ElementId => "stamina-bar";
        public override HUDLayer Layer => HUDLayer.UI;

        private VisualElement _staminaBarFill;
        private float _targetStaminaPercentage;
        private float _currentStaminaPercentage;
        private float _hideTimer;
        private bool _shouldShow;

        protected override VisualElement CreateElement()
        {
            var container = new VisualElement { name = "stamina-bar-container" };
            var staminaBarBackground = new VisualElement { name = "stamina-bar-background" };
            _staminaBarFill = new VisualElement { name = "stamina-bar-fill" };

            staminaBarBackground.Add(_staminaBarFill);
            container.Add(staminaBarBackground);

            return container;
        }

        protected override void ApplyStyling()
        {
            _element.style.position = Position.Absolute;
            _element.style.left = 20;
            _element.style.top = 90;
            _element.style.width = 300;
            _element.style.height = 10;
            _element.style.display = DisplayStyle.None; // Hidden by default

            var background = _element.Q("stamina-bar-background");
            background.style.width = Length.Percent(100);
            background.style.height = Length.Percent(100);
            background.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            background.style.borderTopLeftRadius = 5;
            background.style.borderTopRightRadius = 5;
            background.style.borderBottomLeftRadius = 5;
            background.style.borderBottomRightRadius = 5;

            _staminaBarFill.style.width = Length.Percent(100);
            _staminaBarFill.style.height = Length.Percent(100);
            _staminaBarFill.style.backgroundColor = new Color(1f, 1f, 0.2f);
            _staminaBarFill.style.borderTopLeftRadius = 5;
            _staminaBarFill.style.borderBottomLeftRadius = 5;
        }

        protected override void OnUpdate(float deltaTime)
        {
            // Update stamina bar fill
            if (Math.Abs(_currentStaminaPercentage - _targetStaminaPercentage) > 0.01f)
            {
                _currentStaminaPercentage = Mathf.Lerp(_currentStaminaPercentage, _targetStaminaPercentage, deltaTime * 10f);
                _staminaBarFill.style.width = Length.Percent(_currentStaminaPercentage * 100);
            }

            // Auto-hide stamina bar when full
            if (_shouldShow)
            {
                _hideTimer -= deltaTime;
                if (_hideTimer <= 0 && _targetStaminaPercentage >= 1f)
                {
                    _shouldShow = false;
                    _element.style.display = DisplayStyle.None;
                }
            }
        }

        protected override void OnGameStateUpdate(GameStateData state)
        {
            _targetStaminaPercentage = state.MaxStamina > 0 ? state.Stamina / state.MaxStamina : 0f;

            // Show stamina bar when not full
            if (_targetStaminaPercentage < 1f)
            {
                _shouldShow = true;
                _hideTimer = 3f; // Hide after 3 seconds when full
                _element.style.display = DisplayStyle.Flex;
            }
        }
    }
}