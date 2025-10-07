using Aetheriaum.CharacterSystem.Interfaces;
using Aetheriaum.UISystem.Interfaces;
using System;
using System.Collections.Generic;

namespace Aetheriaum.UISystem.Context
{
    public class CharacterDataContext : IUIDataContext
    {
        private readonly Dictionary<string, object> _properties = new();

        public event Action<string> PropertyChanged;

        public object GetProperty(string propertyName)
        {
            return _properties.TryGetValue(propertyName, out var value) ? value : null;
        }

        public void SetProperty(string propertyName, object value)
        {
            _properties[propertyName] = value;
            PropertyChanged?.Invoke(propertyName);
        }

        public void UpdateFromCharacter(ICharacter character)
        {
            //SetProperty("Name", character.Name);
            //SetProperty("Level", character.Level);
            //SetProperty("Health", character.Health);
            //SetProperty("MaxHealth", character.MaxHealth);
            //SetProperty("Energy", character.Energy);
            //SetProperty("MaxEnergy", character.MaxEnergy);
            //SetProperty("Experience", character.Experience);
            //SetProperty("MaxExperience", character.MaxExperience);
        }
    }
}