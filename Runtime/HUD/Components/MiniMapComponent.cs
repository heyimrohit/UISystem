using Aetheriaum.UISystem.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.HUD
{
    public class MiniMapComponent : BaseHUDElement
    {
        public override string ElementId => "mini-map";
        public override HUDLayer Layer => HUDLayer.UI;

        private VisualElement _mapBackground;
        private VisualElement _playerIndicator;
        private VisualElement _compassRose;

        protected override VisualElement CreateElement()
        {
            var container = new VisualElement { name = "mini-map-container" };

            _mapBackground = new VisualElement { name = "map-background" };
            _playerIndicator = new VisualElement { name = "player-indicator" };
            _compassRose = new VisualElement { name = "compass-rose" };

            _mapBackground.Add(_playerIndicator);
            container.Add(_mapBackground);
            container.Add(_compassRose);

            return container;
        }

        protected override void ApplyStyling()
        {
            _element.style.position = Position.Absolute;
            _element.style.right = 20;
            _element.style.top = 20;
            _element.style.width = 200;
            _element.style.height = 200;

            _mapBackground.style.width = Length.Percent(100);
            _mapBackground.style.height = Length.Percent(100);
            _mapBackground.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
            _mapBackground.style.borderTopLeftRadius = 100;
            _mapBackground.style.borderTopRightRadius = 100;
            _mapBackground.style.borderBottomLeftRadius = 100;
            _mapBackground.style.borderBottomRightRadius = 100;
            _mapBackground.style.borderLeftColor = Color.white;
            _mapBackground.style.borderTopColor = Color.white;
            _mapBackground.style.borderRightColor = Color.white;
            _mapBackground.style.borderBottomColor = Color.white;
            _mapBackground.style.borderLeftWidth = 2;
            _mapBackground.style.borderTopWidth = 2;
            _mapBackground.style.borderRightWidth = 2;
            _mapBackground.style.borderBottomWidth = 2;

            _playerIndicator.style.position = Position.Absolute;
            _playerIndicator.style.left = 95;
            _playerIndicator.style.top = 95;
            _playerIndicator.style.width = 10;
            _playerIndicator.style.height = 10;
            _playerIndicator.style.backgroundColor = Color.red;
            _playerIndicator.style.borderTopLeftRadius = 5;
            _playerIndicator.style.borderTopRightRadius = 5;
            _playerIndicator.style.borderBottomLeftRadius = 5;
            _playerIndicator.style.borderBottomRightRadius = 5;
        }

        protected override void OnUpdate(float deltaTime)
        {
            // Update map based on player position and rotation
        }

        protected override void OnGameStateUpdate(GameStateData state)
        {
            // Update mini-map based on world position
        }
    }
}