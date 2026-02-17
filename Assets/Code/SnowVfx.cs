using AxGrid.Base;
using AxGrid.Model;
using System;
using UnityEngine;

namespace SlotPrototype
{
    public class SnowVfx : MonoBehaviourExtBind
    {
        [SerializeField] private ParticleSystem _ps;

        [Bind("OnUI_ReelChanged")]
        private void ReelChanged(params object[] args)
        {
            var isReeling = args.Length > 0 ? args[0] : null;

            if (Convert.ToBoolean(isReeling))
                _ps.Play(withChildren: true);
            else
                StopEmit();
        }

        private void StopEmit() =>
            _ps.Stop(withChildren: true, stopBehavior: ParticleSystemStopBehavior.StopEmitting);
    }
}