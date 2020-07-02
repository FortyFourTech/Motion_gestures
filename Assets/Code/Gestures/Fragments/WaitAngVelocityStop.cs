using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Ожидание остановки вращения.
    /// Фейлится, если объект ускоряется, вместо замедления.
    /// </summary>
    public class WaitAngVelocityStop : DataFragment
    {
        private float _velocityEps;
        private float _velocityDelta;
        private float _curVel;
        private float _minVel;

        public WaitAngVelocityStop(GestureBase.IDataSource dataSource, float eps) : base(dataSource)
        {
            _velocityEps = eps;
        }

        public override void Reset()
        {
            _curVel = Quaternion.Angle(_dataSource.AngVelocity, Quaternion.identity);
            _minVel = _curVel;
        }

        protected override void _Calc()
        {
            _curVel = Quaternion.Angle(_dataSource.AngVelocity, Quaternion.identity);

            if (_curVel < _minVel)
            {
                _minVel = _curVel;
            }
        }

        protected override bool _SuccessCondition()
        {
            return _curVel < _velocityEps;
        }

        protected override bool _FailCondition()
        {
            return _curVel > _minVel;
        }
    }
}
