using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Ожидание того, что угловая скорость достигнет определенного значения.
    /// </summary>
    public class WaitCriticalAngVelocity : DataFragment
    {
        private float _targetFrameVel;
        private float _curFrameVel;

        public WaitCriticalAngVelocity(GestureBase.IDataSource dataSource, float critAngleVelocity) : base(dataSource)
        {
            var frameDelta = Time.fixedDeltaTime;
            _targetFrameVel = critAngleVelocity * frameDelta;
        }

        public override void Reset()
        {
            _curFrameVel = 0f;
        }

        protected override void _Calc()
        {
            _curFrameVel = Quaternion.Angle(_dataSource.AngVelocity, Quaternion.identity);
        }

        protected override bool _SuccessCondition()
        {
            return _curFrameVel > _targetFrameVel;
        }

        protected override bool _FailCondition()
        {
            return false;
        }
    }
}
