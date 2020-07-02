using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Ожидание резкой смены направления движения оси объекта.
    /// Дельта задается углом за кадр.
    /// </summary>
    public class WaitMovementDirectionChange : AxisFragment
    {
        private float _targetDelta;

        private Quaternion _prevRot, _curRot;
        private Vector3 _curAxis, _prevAxis;
        private Vector3 _axisDeltaPrev, _axisDeltaCur;
        private float _curDelta;

        public WaitMovementDirectionChange(GestureBase.IDataSource dataSource, Vector3 axis, float rotDelta) : base(dataSource, axis)
        {
            _targetDelta = rotDelta;
        }

        public override void Reset()
        {
            _curDelta = 0f;
            _axisDeltaCur = Vector3.zero;
            _axisDeltaPrev = Vector3.zero;
            _curRot = _dataSource.Rotation;
            _prevRot = _curRot;
        }

        protected override void _Calc()
        {
            _prevRot = _curRot;
            _axisDeltaPrev = _axisDeltaCur;

            _curRot = _dataSource.Rotation;
            _curAxis = _curRot * _axis;
            _prevAxis = _prevRot * _axis;
            _axisDeltaCur = _curAxis - _prevAxis;

            _curDelta = Vector3.Angle(_axisDeltaPrev, _axisDeltaCur);

            if (_axisDeltaPrev == Vector3.zero) // case for first frame after reset
                _curDelta = 0f;
        }

        protected override bool _SuccessCondition()
        {
            return _curDelta > _targetDelta;
        }

        protected override bool _FailCondition()
        {
            return false;
        }
    }
}
