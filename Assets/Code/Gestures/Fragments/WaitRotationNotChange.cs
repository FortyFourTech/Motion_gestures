using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Ожидаем что объект не будет вращаться
    /// или будет, но не более чем на заданную погрешность (в градусах).
    /// Для успеха задается время ожидания.
    /// </summary>
    public class WaitRotationNotChange : DataFragment
    {
        private float _deviationMax;
        private float _timeDelta;
        private Quaternion _startRot;
        private float _startTime;
        private float _deviationCur;
        private float _timeCur;

        public WaitRotationNotChange(GestureBase.IDataSource dataSource, float rotDelta, float timeDelta) : base(dataSource)
        {
            _dataSource = dataSource;
            _deviationMax = rotDelta;
            _timeDelta = timeDelta;

            Reset();
        }

        public override void Reset()
        {
            _startRot = _dataSource.Rotation;
            _deviationCur = 0f;
            _startTime = Time.time;
            _timeCur = 0f;
        }

        protected override void _Calc()
        {
            _deviationCur = Quaternion.Angle(_dataSource.Rotation, _startRot);
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
