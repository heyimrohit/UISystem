using Aetheriaum.UISystem.Data;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.HUD
{
    public class EnergyBarComponent : BaseHUDElement
    {
        public override string ElementId => "energy-bar";
        public override HUDLayer Layer => HUDLayer.UI;

        private VisualElement _energyBarFill;
        private float _targetEnergyPercentage;
        private float _currentEnergyPercentage;

        protected override VisualElement CreateElement()
        {
            var container = new VisualElement { name = "energy-bar-container" };
            var energyBarBackground = new VisualElement { name = "energy-bar-background" };
            _energyBarFill = new VisualElement { name = "energy-bar-fill" };

            energyBarBackground.Add(_energyBarFill);
            container.Add(energyBarBackground);

            return container;
        }

        protected override void ApplyStyling()
        {
            _element.style.position = Position.Absolute;
            _element.style.left = 20;
            _element.style.top = 70;
            _element.style.width = 300;
            _element.style.height = 15;

            var background = _element.Q("energy-bar-background");
            background.style.width = Length.Percent(100);
            background.style.height = Length.Percent(100);
            background.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            background.style.borderTopLeftRadius = 7;
            background.style.borderTopRightRadius = 7;
            background.style.borderBottomLeftRadius = 7;
            background.style.borderBottomRightRadius = 7;

            _energyBarFill.style.width = Length.Percent(100);
            _energyBarFill.style.height = Length.Percent(100);
            _energyBarFill.style.backgroundColor = new Color(0.2f, 0.4f, 1f);
            _energyBarFill.style.borderTopLeftRadius = 7;
            _energyBarFill.style.borderBottomLeftRadius = 7;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (Math.Abs(_currentEnergyPercentage - _targetEnergyPercentage) > 0.01f)
            {
                _currentEnergyPercentage = Mathf.Lerp(_currentEnergyPercentage, _targetEnergyPercentage, deltaTime * 8f);
                _energyBarFill.style.width = Length.Percent(_currentEnergyPercentage * 100);
            }
        }

        protected override void OnGameStateUpdate(GameStateData state)
        {
            _targetEnergyPercentage = state.MaxEnergy > 0 ? state.Energy / state.MaxEnergy : 0f;
        }
    }
}