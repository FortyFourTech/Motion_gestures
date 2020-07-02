/*
using System;
using System.Collections;
using UnityEngine;

namespace Dimar.Gestures
{
    /// <summary>
    /// Аналог JustButton для использования с Oculus
    /// </summary>
    public class JustButton_OVR : GestureBase
    {
        private OVRInput.Button _button;

        public override event Action onFire;
        public override event Action onStart;
        public override event Action onBrake;

        /// <param name="button">Controller button to fire</param>
        /// <param name="pcButton">Keyboard button (to test in editor)</param>
        public JustButton_OVR(OVRInput.Button button)
        {
            _button = button;
        }

        public override void ClearEventCallbacks()
        {
            onFire = null;
            onStart = null;
            onBrake = null;
        }

        private YieldInstruction _yieldObject = null;
        public override IEnumerator StartReceiving()
        {
            while (true)
            {
                if (OVRInput.GetDown(_button))
                {
                    onStart?.Invoke();

                    while (!OVRInput.GetUp(_button))
                    {
                        yield return _yieldObject;
                    }

                    onFire?.Invoke();
                }

                yield return _yieldObject;
            }
        }
    }
}
*/
