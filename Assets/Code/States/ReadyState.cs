using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine;

namespace SlotPrototype.States
{
    [State("ReadyState")]
    public class ReadyState : FSMState
    {
        [Enter]
        public void Enter()
        {
            Settings.Model.Set("UI_CanStart", true);
            Settings.Model.Set("UI_CanStop", false);
        }

        [Bind("UI.StartClick")]
        private void OnStart()
        {
            Settings.Invoke("Reel.Start");
            Parent.Change("SpinningLockedState");
        }
    }
}
