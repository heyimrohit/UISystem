using Aetheriaum.UISystem.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.HUD
{
    public class StatusEffectDisplay : BaseHUDElement
    {
        public override string ElementId => "status-effects";
        public override HUDLayer Layer => HUDLayer.UI;

        private readonly List<StatusEffectIcon> _activeEffects = new();

        protected override VisualElement CreateElement()
        {
            var container = new VisualElement { name = "status-effects-container" };
            container.style.flexDirection = FlexDirection.Row;
            return container;
        }

        protected override void ApplyStyling()
        {
            _element.style.position = Position.Absolute;
            _element.style.left = 350;
            _element.style.top = 20;
            _element.style.flexDirection = FlexDirection.Row;
        }

        protected override void OnUpdate(float deltaTime)
        {
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                _activeEffects[i].Update(deltaTime);
                if (_activeEffects[i].IsExpired)
                {
                    _element.Remove(_activeEffects[i].Element);
                    _activeEffects.RemoveAt(i);
                }
            }
        }

        protected override void OnGameStateUpdate(GameStateData state)
        {
            // Update status effects from game state
            foreach (var effect in state.StatusEffects ?? new List<StatusEffect>())
            {
                var existingEffect = _activeEffects.Find(e => e.EffectName == effect.Name);
                if (existingEffect != null)
                {
                    existingEffect.UpdateDuration(effect.Duration);
                }
                else
                {
                    var newEffect = new StatusEffectIcon(effect);
                    _activeEffects.Add(newEffect);
                    _element.Add(newEffect.Element);
                }
            }
        }

        private class StatusEffectIcon
        {
            public VisualElement Element { get; private set; }
            public string EffectName { get; private set; }
            public bool IsExpired => _duration <= 0;

            private float _duration;
            private float _maxDuration;
            private Label _durationText;
            private VisualElement _durationBar;

            public StatusEffectIcon(StatusEffect effect)
            {
                EffectName = effect.Name;
                _duration = effect.Duration;
                _maxDuration = effect.MaxDuration;
                CreateElement(effect);
            }

            private void CreateElement(StatusEffect effect)
            {
                Element = new VisualElement { name = $"status-{effect.Name}" };
                Element.style.width = 40;
                Element.style.height = 40;
                Element.style.marginRight = 5;
                Element.style.backgroundColor = GetElementColor(effect.Element);
                Element.style.borderTopLeftRadius = 5;
                Element.style.borderTopRightRadius = 5;
                Element.style.borderBottomLeftRadius = 5;
                Element.style.borderBottomRightRadius = 5;

                _durationText = new Label(Mathf.Ceil(_duration).ToString());
                _durationText.style.position = Position.Absolute;
                _durationText.style.bottom = -2;
                _durationText.style.right = -2;
                _durationText.style.fontSize = 10;
                _durationText.style.color = Color.white;

                _durationBar = new VisualElement();
                _durationBar.style.position = Position.Absolute;
                _durationBar.style.bottom = 0;
                _durationBar.style.left = 0;
                _durationBar.style.width = Length.Percent(100);
                _durationBar.style.height = 3;
                _durationBar.style.backgroundColor = Color.white;

                Element.Add(_durationText);
                Element.Add(_durationBar);
            }

            public void UpdateDuration(float newDuration)
            {
                _duration = newDuration;
            }

            public void Update(float deltaTime)
            {
                _duration -= deltaTime;
                _durationText.text = Mathf.Max(0, Mathf.Ceil(_duration)).ToString();

                var percentage = _maxDuration > 0 ? _duration / _maxDuration : 0f;
                _durationBar.style.width = Length.Percent(percentage * 100);
            }

            private Color GetElementColor(ElementType element)
            {
                return element switch
                {
                    ElementType.Pyro => new Color(1f, 0.4f, 0.2f),
                    ElementType.Hydro => new Color(0.2f, 0.6f, 1f),
                    ElementType.Electro => new Color(0.6f, 0.3f, 0.8f),
                    ElementType.Cryo => new Color(0.6f, 0.8f, 1f),
                    ElementType.Dendro => new Color(0.4f, 0.8f, 0.2f),
                    ElementType.Anemo => new Color(0.4f, 0.8f, 0.7f),
                    ElementType.Geo => new Color(0.8f, 0.6f, 0.2f),
                    _ => Color.gray
                };
            }
        }
    }
}