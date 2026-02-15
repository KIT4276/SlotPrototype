using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine;

namespace SlotPrototype.States
{
    [State("StoppingState")]
    public class StoppingState : FSMState
    {
        [Enter]
        public  void Enter()
        {
           // Debug.Log("Enter StoppingState");
            Settings.Model.Set("UI_CanStart", false);
            Settings.Model.Set("UI_CanStop", false);
        }

        [Bind("Reel.Stopped")]
        private void OnStopped()
        {
            Parent.Change("ReadyState");
        }
    }
}
