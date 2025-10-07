using Aetheriaum.UISystem.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.HUD
{
    public class SkillCooldownHUD : BaseHUDElement
    {
        public override string ElementId => "skill-cooldown";
        public override HUDLayer Layer => HUDLayer.UI;

        private readonly Dictionary<string, SkillCooldownDisplay> _skillDisplays = new();

        protected override VisualElement CreateElement()
        {
            var container = new VisualElement { name = "skill-cooldown-container" };
            return container;
        }

        protected override void ApplyStyling()
        {
            _element.style.position = Position.Absolute;
            _element.style.right = 50;
            _element.style.bottom = 50;
            _element.style.flexDirection = FlexDirection.Row;
        }

        protected override void OnUpdate(float deltaTime)
        {
            foreach (var display in _skillDisplays.Values)
            {
                display.Update(deltaTime);
            }
        }

        protected override void OnGameStateUpdate(GameStateData state)
        {
            // This would be updated from your skill system
            // For now, we'll create some sample skills
            EnsureSkillDisplay("ElementalSkill", 8f, 0f);
            EnsureSkillDisplay("ElementalBurst", 15f, 0f);
        }

        private void EnsureSkillDisplay(string skillId, float cooldown, float remaining)
        {
            if (!_skillDisplays.ContainsKey(skillId))
            {
                var display = new SkillCooldownDisplay(skillId, cooldown);
                _skillDisplays[skillId] = display;
                _element.Add(display.Element);
            }

            _skillDisplays[skillId].SetCooldown(remaining);
        }

        private class SkillCooldownDisplay
        {
            public VisualElement Element { get; private set; }

            private readonly string _skillId;
            private readonly float _maxCooldown;
            private float _remainingCooldown;
            private VisualElement _cooldownOverlay;
            private Label _cooldownText;

            public SkillCooldownDisplay(string skillId, float maxCooldown)
            {
                _skillId = skillId;
                _maxCooldown = maxCooldown;
                CreateElement();
            }

            private void CreateElement()
            {
                Element = new VisualElement { name = $"skill-{_skillId}" };
                Element.style.width = 60;
                Element.style.height = 60;
                Element.style.marginRight = 10;
                Element.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                Element.style.borderTopLeftRadius = 30;
                Element.style.borderTopRightRadius = 30;
                Element.style.borderBottomLeftRadius = 30;
                Element.style.borderBottomRightRadius = 30;

                _cooldownOverlay = new VisualElement { name = "cooldown-overlay" };
                _cooldownOverlay.style.width = Length.Percent(100);
                _cooldownOverlay.style.height = Length.Percent(100);
                _cooldownOverlay.style.backgroundColor = new Color(0f, 0f, 0f, 0.7f);
                _cooldownOverlay.style.borderTopLeftRadius = 30;
                _cooldownOverlay.style.borderTopRightRadius = 30;
                _cooldownOverlay.style.borderBottomLeftRadius = 30;
                _cooldownOverlay.style.borderBottomRightRadius = 30;
                _cooldownOverlay.style.display = DisplayStyle.None;

                _cooldownText = new Label("0") { name = "cooldown-text" };
                _cooldownText.style.position = Position.Absolute;
                _cooldownText.style.width = Length.Percent(100);
                _cooldownText.style.height = Length.Percent(100);
                _cooldownText.style.unityTextAlign = TextAnchor.MiddleCenter;
                _cooldownText.style.color = Color.white;
                _cooldownText.style.fontSize = 16;
                _cooldownText.style.display = DisplayStyle.None;

                Element.Add(_cooldownOverlay);
                Element.Add(_cooldownText);
            }

            public void SetCooldown(float remaining)
            {
                _remainingCooldown = remaining;

                if (remaining > 0)
                {
                    _cooldownOverlay.style.display = DisplayStyle.Flex;
                    _cooldownText.style.display = DisplayStyle.Flex;
                    _cooldownText.text = Mathf.Ceil(remaining).ToString();
                }
                else
                {
                    _cooldownOverlay.style.display = DisplayStyle.None;
                    _cooldownText.style.display = DisplayStyle.None;
                }
            }

            public void Update(float deltaTime)
            {
                if (_remainingCooldown > 0)
                {
                    _remainingCooldown -= deltaTime;
                    if (_remainingCooldown <= 0)
                    {
                        SetCooldown(0);
                    }
                    else
                    {
                        _cooldownText.text = Mathf.Ceil(_remainingCooldown).ToString();
                    }
                }
            }
        }
    }
}