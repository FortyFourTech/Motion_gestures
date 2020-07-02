using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Детектирование того что ось объекта отклоняется не больше
    /// заданного диапазона (в градусах).
    /// </summary>
    public class AxisMoving : AxisFragment
    {
        private float _failDelta;
        private Quaternion _rotPrev, _rotCur;
        private Vector3 _axisPrev, _axisCur;
        private float _axisDelta, _rotDelta, _deviationDelta;

        public AxisMoving(GestureBase.IDataSource dataSource, Vector3 axis, float failDelta) : base(dataSource, axis)
        {
            _failDelta = failDelta;

            Reset();
        }

        public override void Reset()
        {
            _rotCur = _dataSource.Rotation;
            _rotPrev = _rotCur;
            _axisCur = _rotCur * _axis;
            _axisPrev = _axisCur;
            _deviationDelta = 0f;
        }

        protected override void _Calc()
        {
            _rotPrev = _rotCur;
            _axisPrev = _axisCur;

            _rotCur = _dataSource.Rotation;
            _axisCur = _rotCur * _axis;

            _axisDelta = Vector3.Angle(_axisCur, _axisPrev);
            _rotDelta = Quaternion.Angle(_rotCur, _rotPrev);

            _deviationDelta = _rotDelta - _axisDelta;
        }

        protected override bool _SuccessCondition()
        {
            return false;
        }

        protected override bool _FailCondition()
        {
            return _deviationDelta > _failDelta;
        }
    }
}
