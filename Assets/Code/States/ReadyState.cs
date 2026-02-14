using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace SlotPrototype.States
{
    [State("Ready")]
    public class ReadyState : FSMState
    {
        public void Enter()
        {
            Settings.Model.Set("UI.CanStart", true);
            Settings.Model.Set("UI.CanStop", false);
        }

        [Bind("UI.StartClick")]
        private void OnStart()
        {
            Settings.Invoke("Reel.Start");
            Parent.Change("SpinningLocked");
        }
    }

}
