using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace SlotPrototype.States
{
    [State("Stopping")]
    public class StoppingState : FSMState
    {
        public void Enter()
        {
            Settings.Model.Set("UI.CanStart", false);
            Settings.Model.Set("UI.CanStop", false);
        }

        [Bind("Reel.Stopped")]
        private void OnStopped()
        {
            Parent.Change("Ready");
        }
    }

}
