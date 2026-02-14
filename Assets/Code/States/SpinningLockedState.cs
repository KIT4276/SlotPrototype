using AxGrid;
using AxGrid.FSM;
using UnityEngine;

namespace SlotPrototype.States
{
    [State("SpinningLockedState")]
    public class SpinningLockedState : FSMState
    {
        [Enter]
        public  void Enter()
        {
            Debug.Log("Enter SpinningLockedState");
            Settings.Model.Set("UI_CanStart", false);
            Settings.Model.Set("UI_CanStop", false);
        }

        [One(3f)]
        private void UnlockStop()
        {
            Parent.Change("SpinningState");
        }
    }
}
