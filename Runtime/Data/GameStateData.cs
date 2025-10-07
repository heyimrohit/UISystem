using System.Collections.Generic;
using UnityEngine;

namespace Aetheriaum.UISystem.Data
{
    public struct GameStateData
    {
        public float Health;
        public float MaxHealth;
        public float Energy;
        public float MaxEnergy;
        public float Stamina;
        public float MaxStamina;
        public int Level;
        public int Experience;
        public int MaxExperience;
        public string ActiveCharacterName;
        public ElementType ActiveElement;
        public List<StatusEffect> StatusEffects;
        public Vector3 WorldPosition;
        public float CombatTime;
        public bool IsInCombat;
    }
}