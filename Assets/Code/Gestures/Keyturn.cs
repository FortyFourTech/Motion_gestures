using System;
using System.Collections;
using UnityEngine;
using Dimar.Gestures.Fragments;
using Dimar.Gestures.Conditions;

namespace Dimar.Gestures
{
    /// <summary>
    /// Повернуть направо, потом налево. Как будто открываем ключем дверь.
    /// </summary>
    public class Keyturn : GestureBase
    {
        public override event Action onFire;
        public override event Action onStart;
        public override event Action onBrake;

#region PRIVATE_FIELDS
        private IDataSource _controller;
        private Transform _camera;
        SequenceFragment _sequence;
        // GestFragment _debugFragment;
#endregion

        public Keyturn(IDataSource controller, Transform camera)
        {
            _controller = controller;
            _camera = camera;
        }

        public override IEnumerator StartReceiving()
        {
            float lastFireTime = float.NaN;

            // _debugFragment = new WaitAxisGoPath(_controller, Vector3.up, Quaternion.Euler(0f, 0f, -30f), 10f, 1f);
            // _debugFragment = new WaitMovementDirectionChange(_controller, Vector3.up, 10f);
            _sequence = new SequenceFragment(
                        new WaitAxisGoPath(_controller, Vector3.up, Quaternion.Euler(-7f, 0f, -15f), 10f, 2f),
                        new WaitMovementDirectionChange(_controller, Vector3.up, 20f),
                        new CustomAction(() => onStart?.Invoke()),
                        new WaitAxisGoPath(_controller, Vector3.up, Quaternion.Euler(7f, 0f, 15f), 10f, 2f),
                        new WaitMovementDirectionChange(_controller, Vector3.up, 20f)
                    );
            IChainable[] chain = new IChainable[] {
				// new ParallelFragment(parameters: new GestFragment[] {
					// new WaitAxisStandStill(_controller, Vector3.forward, 1f, 1000f),
					_sequence,
                    new FailableAction(() => _DoFire(ref lastFireTime)),
				// }),
			};

            while (true)
            {
                yield return ReceiveGesture(chain);
            }
        }

        protected IEnumerator ReceiveGesture(IChainable[] chain)
        {
            foreach (var unit in chain)
            {
                yield return unit.Run();

                if (unit.Failed())
                {
                    onBrake?.Invoke();
                    yield break;
                }
            }
        }

        public override void ClearEventCallbacks()
        {
            onFire = null;
            onStart = null;
            onBrake = null;
        }

        private bool _DoFire(ref float lastFireTime)
        {
            bool fired = false;
            if (onFire != null)
            {
                lastFireTime = Time.time;

                onFire.Invoke();

                fired = true;
            }

            return fired;
        }

        public void ThrowDir(Quaternion in1, out Vector3 dir)
        {
            var forward = Vector3.forward;
            dir = in1 * forward;
        }
    }
}
