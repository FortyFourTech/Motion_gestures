using System.Collections;
using UnityEngine;

namespace Dimar.Gestures.Conditions
{
    /// <summary>
    /// Проверка, завершения таймаута.
    /// </summary>
    class CheckCooldown : IChainable
    {
        private IDelegate _delegate;
        private float _cooldown;

        public CheckCooldown(IDelegate dlg, float cooldownTime)
        {
            _delegate = dlg;
            _cooldown = cooldownTime;
        }

        public IEnumerator Run()
        {
            yield break;
        }

        private bool _Condition()
        {
            var isFirstThrow = float.IsNaN(_delegate.LastFireTime);
            var isReadyForNextThrow = Time.time - _delegate.LastFireTime > _cooldown;

            return isFirstThrow || isReadyForNextThrow;
        }

        public bool Failed()
        {
            return !_Condition();
        }

        public bool Succeeded()
        {
            return _Condition();
        }

        public interface IDelegate
        {
            float LastFireTime { get; }
        }
    }
}
