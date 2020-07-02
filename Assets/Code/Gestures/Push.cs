using System;
using System.Collections;
using UnityEngine;
using Dimar.Gestures.Fragments;
using Dimar.Gestures.Conditions;

namespace Dimar.Gestures
{
    /// <summary>
    /// Двинуть вперед и остановить.
    /// Сработает в момент остановки.
    /// Реализация кривовата, поскольку основывается на вращении объекта. Пока отсутствуют фрагменты для перемещения.
    /// </summary>
    public class Push : GestureBase
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

        public Push(IDataSource controller, Transform camera)
        {
            _controller = controller;
            _camera = camera;
        }

        public override IEnumerator StartReceiving()
        {
            float lastFireTime = float.NaN;

            _sequence = new SequenceFragment(
                        new WaitAxisGoPath(_controller, Vector3.forward, Quaternion.Euler(-7f, 0f, 0f), 10f, 2f),
                        new WaitMovementDirectionChange(_controller, Vector3.forward, 10f),
                        new CustomAction(() => onStart?.Invoke()),
                        new WaitAxisGoPath(_controller, Vector3.forward, Quaternion.Euler(7f, 0f, 0f), 10f, 2f),
                        new WaitAngVelocityStop(_controller, 1f)
                    );
            IChainable[] chain = new IChainable[] {
					_sequence,
                    new FailableAction(() => _DoFire(ref lastFireTime)),
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

        public void ThrowPosDir(Quaternion in1, out Vector3 pos, out Vector3 dir)
        {
            var forward = Vector3.forward;
            dir = in1 * forward;
            pos = _camera != null ?
                _camera.transform.position + (_camera.transform.right / 8.0f) : // 1/4 - fixed arm offset
                new Vector3(1f / 8f, 0, 0);
        }

    }
}
