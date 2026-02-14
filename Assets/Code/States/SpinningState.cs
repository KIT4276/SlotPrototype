using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine;

namespace SlotPrototype.States
{
    [State("SpinningState")]
    public class SpinningState : FSMState
    {
        [Enter]
        public void Enter()
        {
            Debug.Log("Enter SpinningState");
            Settings.Model.Set("UI_CanStart", false);
            Settings.Model.Set("UI_CanStop", true);
        }

        [Bind("UI.StopClick")]
        private void OnStop()
        {
            Settings.Model.Set("UI_CanStop", false);
            Settings.Invoke("Reel.Stop");
            Parent.Change("StoppingState");
        }
    }
}
