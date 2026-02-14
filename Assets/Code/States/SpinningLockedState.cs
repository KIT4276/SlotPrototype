using AxGrid;
using AxGrid.FSM;

namespace SlotPrototype.States
{
    [State("SpinningLocked")]
    public class SpinningLockedState : FSMState
    {
        public void Enter()
        {
            Settings.Model.Set("UI.CanStart", false);
            Settings.Model.Set("UI.CanStop", false);
        }

        [One(3f)]
        private void UnlockStop()
        {
            Parent.Change("Spinning");
        }
    }
}
