using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using UnityEngine;

namespace SlotPrototype.States
{
    public class InitSlotsFsm : MonoBehaviourExt
    {
        [OnAwake]
        private void Create()
        {
            Settings.Fsm = new FSM();
            Settings.Fsm.Add(new ReadyState());
            Settings.Fsm.Add(new SpinningLockedState());
            Settings.Fsm.Add(new SpinningState());
            Settings.Fsm.Add(new StoppingState());
        }

        [OnStart]
        private void StartFsm() => Settings.Fsm.Start("Ready");

        [OnUpdate]
        private void Tick() => Settings.Fsm.Update(Time.deltaTime);
    }

}
