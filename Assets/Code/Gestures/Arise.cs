using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dimar.Gestures.Fragments;
using Dimar.Gestures.Conditions;

namespace Dimar.Gestures
{
    /// <summary>
    /// Резко поднять руку в вертикальное положение.
    /// </summary>
    public class Arise : GestureBase,
                        WaitAngVelocityStartReduce.IDelegate,
                        CheckCooldown.IDelegate
    {
        public override event Action onFire;
        public override event Action onStart;
        public override event Action onBrake;

#region PRIVATE_FIELDS
        private IDataSource _controller;

        private Transform _directionOrigin;
#endregion

        /// <summary>
        /// Угловая скорость, при превышении которой бросок следует распознавать. В градусах за секунду
        /// </summary>
        private const float _critAngleVelocity = 750f;

#region DELEGATE_FIELDS
        public Vector3 MaxPosition
        {
            get => _maxPosition;
            set => _maxPosition = value;
        }
        public Vector3 MaxVelocity
        {
            get => _maxVelocity;
            set => _maxVelocity = value;
        }
        public Quaternion MaxRotation
        {
            get => _maxRotation;
            set => _maxRotation = value;
        }
        public Quaternion MaxAngVelocity
        {
            get => _maxAngVelocity;
            set => _maxAngVelocity = value;
        }

        public float LastFireTime => _lastFireTime;
#endregion

#region OBSERVATION_VARIABLES
        Vector3 _maxPosition = Vector3.zero;
        Vector3 _maxVelocity = Vector3.zero;
        Quaternion _maxRotation = Quaternion.identity;
        Quaternion _maxAngVelocity = Quaternion.identity;

        float _lastFireTime = float.NaN;
#endregion

        public Arise(IDataSource controller, Transform directionOrigin)
        {
            _controller = controller;
            _directionOrigin = directionOrigin;
        }

        public override IEnumerator StartReceiving()
        {
            IChainable[] chain = new IChainable[] {
                new WaitNonCriticalAngVelocity(_controller, _critAngleVelocity),
                new CheckDirectionAlign(_controller, new Vector3(0,-0.5f,0.5f), 45f, _directionOrigin),
                new WaitCriticalAngVelocity(_controller, _critAngleVelocity),
                new CustomAction(() => onStart?.Invoke()),
                new WaitAngVelocityStartReduce(_controller, this, 10f),
				// new WaitVelocityStop(_controller, 1f),
				// new DirectionInFrustum(_controller, _origin),
				// new WaitVelocityNotChange(_controller, 1f, 0.1f),
				new CheckDirectionAlign(_controller, Vector3.up, 45f, _directionOrigin),
				// new CheckCooldown(this, k_shootPeriod),
				new FailableAction(() => _DoFire(ref _lastFireTime) ),
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

                onFire();

                fired = true;
            }

            return fired;
        }
    }
}
