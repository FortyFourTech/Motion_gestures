using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Ожидаем что ось объекта займет определенную позицию,
    /// пройдя любой путь.
    /// </summary>
    public class WaitAxisReachPoint : AxisFragment
    {
        private float _finishEps;
        private Vector3 _endAxisPos;
        private float _remainAngle;

        public WaitAxisReachPoint(GestureBase.IDataSource dataSource, Vector3 axis, Vector3 axisPosition, float finishEps) : base (dataSource, axis)
        {
            _finishEps = finishEps;
            _endAxisPos = axisPosition;

            Reset();
        }

        public override void Reset()
        {
            _remainAngle = 0f;
        }

        protected override void _Calc()
        {
            _remainAngle = Vector3.Angle(_endAxisPos, _dataSource.Rotation * _axis);
        }

        protected override bool _SuccessCondition()
        {
            return _remainAngle < _finishEps;
        }

        protected override bool _FailCondition()
        {
            return false;
        }
    }
}
