using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using System;

namespace SlotPrototype.UI
{
    public sealed class UIButtonBind_CanStart : UIButtonDataBind
    {
        private const string FieldName = "UI_CanStart";

        [OnStart]
        private void ApplyInitial()
        {
            //_button.interactable = Settings.Model.GetBool(FieldName); // вообще лажа получается
            Settings.Model.EventManager.Add(this);
        }

        [OnDestroy]
        private void Destr()
        {
            Settings.Model.EventManager.Remove(this);
        }

        [Bind("On" + FieldName + "Changed")]
        public void OnInteractableChanged(params object[] args)
        {
            var value = args.Length > 0 ? args[0] : null;
          //  Debug.Log(value);
            SetInteractable(Convert.ToBoolean(value));
        }

        //[OnUpdate]
        //private void Test()
        //{
        //    Debug.Log(Settings.Model.Get<bool>(FieldName));
        //}
    }
}

