using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dimar.Gestures.Fragments;
using Dimar.Gestures.Conditions;

namespace Dimar.Gestures
{
    /// <summary>
    /// Совершить бросательное движение рукой.
    /// </summary>
    public class Throw : GestureBase,
						WaitAngVelocityStartReduce.IDelegate,
						CheckCooldown.IDelegate
    {

        public override event Action onFire;
        public override event Action onStart;
        public override event Action onBrake;

#region PRIVATE_FIELDS
        private GestureBase.IDataSource _controller;
        private Transform _camera;

        private GameObject _allowedDirection;
        public SequenceFragment _sequence;
        private ParallelFragment _parallel;
        private AxisMoving _axisMoveFr;

        private Transform _directionOrigin;
#endregion

        /// <summary>
        /// Угловая скорость, при превышении которой бросок следует распознавать. В градусах за секунду
        /// </summary>
        private const float _critAngleVelocity = 1000f;
        Camera _frustumCamera;

#region OBSERVATION_VARIABLES
        Vector3 _maxPosition = Vector3.zero;
        Vector3 _maxVelocity = Vector3.zero;
        Quaternion _maxRotation = Quaternion.identity;
        Quaternion _maxAngVelocity = Quaternion.identity;

        float _lastFireTime = float.NaN;
#endregion

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

        public Throw(GestureBase.IDataSource controller, Transform directionOrigin)
        {
            _controller = controller;
            _directionOrigin = directionOrigin;
        }

        public override IEnumerator StartReceiving()
        {
            float lastFireTime = float.NaN;

            // _sequence = new SequenceFragment(
            // new WaitVelocityStartReduce(_controller, this, 10f),
            // new WaitVelocityStop(_controller, 20f)
            // );
            var reduce = new WaitAngVelocityStartReduce(_controller, this, 5f);
            _axisMoveFr = new AxisMoving(_controller, Vector3.forward, 10f);
            _parallel = new ParallelFragment(finish: EComposeType.Any, fragments: new GestFragment[] {
                    _axisMoveFr,
                    reduce,
                });
            IChainable[] chain = new IChainable[] {
                new WaitNonCriticalAngVelocity(_controller, _critAngleVelocity),
				// new CheckDirectionAlign(_controller, Vector3.up, 30f, _directionOrigin),
				new WaitCriticalAngVelocity(_controller, _critAngleVelocity),
                new CustomAction(() => onStart?.Invoke()),
                _parallel,
				// new CheckVerticalDirection(_controller, _frustumCamera),
				// new CheckDirectionAlign(_controller, Quaternion.Euler(-10f,0f,0f) * Vector3.forward, 30f, _directionOrigin),
				// new CheckCooldown(this, k_shootPeriod),
				new FailableAction(() => _DoThrow(ref lastFireTime))
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

        private bool _DoThrow(ref float lastThrowTime)
        {
            bool throwed = false;
            if (onFire != null)
            {
                lastThrowTime = Time.time;
                onFire.Invoke();
                throwed = true;
            }

            return throwed;
        }

        public void ThrowDir(Quaternion in1, out Vector3 dir)
        {
            var forward = Vector3.forward;
            // var cross = Vector3.Cross(in1 * forward, in2 * forward);
            // Quaternion Quat2 =
            // cross.magnitude > 0.05f ?
            // Quaternion.AngleAxis(45.0f, cross) * in1 // 90 - tangent; + (-60)
            // : Quaternion.identity
            // ;
            var result = in1 * Vector3.forward;
            // var result = Quaternion.Euler(in2.eulerAngles.x, in1.eulerAngles.y, 0f) * Vector3.forward;
            dir = result;
        }
    }
}
