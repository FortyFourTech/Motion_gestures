using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Ожидаем что ось объекта не будет перемещаться
    /// или будет, но не более чем на заданную погрешность.
    /// Для успеха задается время ожидания.
    /// </summary>
    public class WaitAxisStandStill : AxisFragment
    {
        private float _deviationMax;
        private float _timeDelta;
        private Vector3 _startAxisPos;
        private float _startTime;
        private float _deviationCur, _timeCur;

        public WaitAxisStandStill(GestureBase.IDataSource dataSource, Vector3 axis, float deviation, float timeDelta) : base(dataSource, axis)
        {
            _deviationMax = deviation;
            _timeDelta = timeDelta;

            Reset();
        }

        public override void Reset()
        {
            _startAxisPos = _dataSource.Rotation * _axis;
            _deviationCur = 0f;
            _startTime = Time.time;
            _timeCur = 0f;
        }

        protected override void _Calc()
        {
            _deviationCur = Vector3.Angle(_startAxisPos, _dataSource.Rotation * _axis);
            _timeCur = Time.time - _startTime;
        }

        protected override bool _SuccessCondition()
        {
            return _timeCur > _timeDelta;
        }

        protected override bool _FailCondition()
        {
            return _deviationCur > _deviationMax;
        }
    }
}
