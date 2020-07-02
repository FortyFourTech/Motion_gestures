using System;
using System.Collections;
using UnityEngine;

namespace Dimar.Gestures
{
    /// <summary>
    /// Просто нажать и отпустить кнопку.
    /// </summary>
    public class JustButton : GestureBase
    {
        private KeyCode _button;

        public override event Action onFire;
        public override event Action onStart;
        public override event Action onBrake;

        /// <param name="button">Controller button to fire</param>
        /// <param name="pcButton">Keyboard button (to test in editor)</param>
        public JustButton(KeyCode button)
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
                if (Input.GetKeyDown(_button))
                {
                    onStart?.Invoke();

                    while (!Input.GetKeyUp(_button))
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
