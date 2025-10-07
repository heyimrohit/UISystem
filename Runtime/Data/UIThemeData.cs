using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aetheriaum.UISystem.Data
{
    [Serializable]
    public class UIThemeData
    {
        public Color PrimaryColor = new Color(0.2f, 0.6f, 1f);
        public Color SecondaryColor = new Color(0.8f, 0.8f, 0.8f);
        public Color AccentColor = new Color(1f, 0.8f, 0.2f);
        public Color HealthColor = new Color(0.2f, 0.8f, 0.2f);
        public Color EnergyColor = new Color(0.2f, 0.4f, 1f);
        public Color StaminaColor = new Color(1f, 1f, 0.2f);
        public Dictionary<ElementType, Color> ElementColors = new Dictionary<ElementType, Color>
        {
            { ElementType.Anemo, new Color(0.4f, 0.8f, 0.7f) },
            { ElementType.Geo, new Color(0.8f, 0.6f, 0.2f) },
            { ElementType.Electro, new Color(0.6f, 0.3f, 0.8f) },
            { ElementType.Dendro, new Color(0.4f, 0.8f, 0.2f) },
            { ElementType.Hydro, new Color(0.2f, 0.6f, 1f) },
            { ElementType.Pyro, new Color(1f, 0.4f, 0.2f) },
            { ElementType.Cryo, new Color(0.6f, 0.8f, 1f) }
        };
    }
}