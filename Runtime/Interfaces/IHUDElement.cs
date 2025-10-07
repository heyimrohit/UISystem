using Aetheriaum.UISystem.Data;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Interfaces
{
    public interface IHUDElement
    {
        string ElementId { get; }
        bool IsActive { get; }
        HUDLayer Layer { get; }

        void Initialize(VisualElement parent);
        void UpdateElement(float deltaTime);
        void SetActive(bool active);
        void OnGameStateChanged(GameStateData state);
    }
}