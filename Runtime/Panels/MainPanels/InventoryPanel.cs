using Aetheriaum.UISystem.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Panels
{
    public class InventoryPanel : BaseUIPanel
    {
        public override string PanelId => "inventory";
        public override UIState RequiredState => UIState.InventoryMenu;

        private VisualElement _itemGrid;
        private Label _titleLabel;
        private readonly List<InventorySlot> _inventorySlots = new();

        protected override VisualElement CreatePanel()
        {
            var panel = new VisualElement { name = "inventory-panel" };

            _titleLabel = new Label("Inventory") { name = "inventory-title" };
            _itemGrid = new VisualElement { name = "item-grid" };

            panel.Add(_titleLabel);
            panel.Add(_itemGrid);

            // Create inventory slots
            for (int i = 0; i < 40; i++) // 8x5 grid
            {
                var slot = new InventorySlot(i);
                _inventorySlots.Add(slot);
                _itemGrid.Add(slot.Element);
            }

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
            _panelElement.style.borderTopLeftRadius = 10;
            _panelElement.style.borderTopRightRadius = 10;
            _panelElement.style.borderBottomLeftRadius = 10;
            _panelElement.style.borderBottomRightRadius = 10;
            _panelElement.style.paddingTop = 20;
            _panelElement.style.paddingLeft = 20;
            _panelElement.style.paddingRight = 20;
            _panelElement.style.paddingBottom = 20;

            _titleLabel.style.fontSize = 24;
            _titleLabel.style.color = Color.white;
            _titleLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            _titleLabel.style.marginBottom = 20;

            _itemGrid.style.flexDirection = FlexDirection.Row;
            _itemGrid.style.flexWrap = Wrap.Wrap;
            _itemGrid.style.width = Length.Percent(100);
            _itemGrid.style.justifyContent = Justify.SpaceAround;
        }

        public override void UpdatePanel()
        {
            // Update inventory slots with current items
            // This would integrate with your inventory system
        }

        public override void OnInputReceived(UIInputData input)
        {
            if (input.CancelPressed || input.InventoryPressed)
            {
                Hide();
            }
        }

        private class InventorySlot
        {
            public VisualElement Element { get; private set; }
            public int SlotIndex { get; private set; }

            public InventorySlot(int index)
            {
                SlotIndex = index;
                CreateSlot();
            }

            private void CreateSlot()
            {
                Element = new VisualElement { name = $"inventory-slot-{SlotIndex}" };
                Element.style.width = 60;
                Element.style.height = 60;
                Element.style.marginLeft = 2;
                Element.style.marginTop = 2;
                Element.style.marginRight = 2;
                Element.style.marginBottom = 2;
                Element.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                Element.style.borderTopLeftRadius = 5;
                Element.style.borderTopRightRadius = 5;
                Element.style.borderBottomLeftRadius = 5;
                Element.style.borderBottomRightRadius = 5;
                Element.style.borderLeftColor = Color.gray;
                Element.style.borderTopColor = Color.gray;
                Element.style.borderRightColor = Color.gray;
                Element.style.borderBottomColor = Color.gray;
                Element.style.borderLeftWidth = 1;
                Element.style.borderTopWidth = 1;
                Element.style.borderRightWidth = 1;
                Element.style.borderBottomWidth = 1;
            }
        }
    }
}