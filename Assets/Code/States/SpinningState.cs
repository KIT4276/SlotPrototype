using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace SlotPrototype.States
{
    [State("Spinning")]
    public class SpinningState : FSMState
    {
        public void Enter()
        {
            Settings.Model.Set("UI.CanStart", false);
            Settings.Model.Set("UI.CanStop", true);
        }

        [Bind("UI.StopClick")]
        private void OnStop()
        {
            Settings.Model.Set("UI.CanStop", false);
            Settings.Invoke("Reel.Stop");
            Parent.Change("Stopping");
        }
    }
}
