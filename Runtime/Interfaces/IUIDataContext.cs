using System;

namespace Aetheriaum.UISystem.Interfaces
{
    public interface IUIDataContext
    {
        event Action<string> PropertyChanged;
        object GetProperty(string propertyName);
        void SetProperty(string propertyName, object value);
    }
}
