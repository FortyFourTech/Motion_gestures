using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Ожидание начала снижения угловой скорости.
    /// Сохраняет в делегат значения источника данных в момент максимальной скорости.
    /// </summary>
    public class WaitAngVelocityStartReduce : DataFragment
    {
        private float _targetDelta;// = 1f; // ?

        private Vector3 _maxPosition;
        private Vector3 _maxVelocity;
        private Quaternion _maxRotation;
        private Quaternion _maxAngVelocity;
        
        private float _curVel, _maxVel;
        private IDelegate _delegate;

        public WaitAngVelocityStartReduce(GestureBase.IDataSource dataSource, IDelegate dlg, float reduceDelta) : base(dataSource)
        {
            _delegate = dlg;
            _targetDelta = reduceDelta;

            Reset();
        }

        public override void Reset()
        {
            _curVel = Quaternion.Angle(_dataSource.AngVelocity, Quaternion.identity);
            _maxVel = _curVel;
            _maxRotation = _dataSource.Rotation;
            _maxAngVelocity = _dataSource.AngVelocity;

            _delegate.MaxPosition = Vector3.zero;
            _delegate.MaxVelocity = Vector3.zero;
            _delegate.MaxRotation = Quaternion.identity;
            _delegate.MaxAngVelocity = Quaternion.identity;
        }

        protected override void _Calc()
        {
            _curVel = Quaternion.Angle(_dataSource.AngVelocity, Quaternion.identity);
            if (_curVel >= _maxVel)
            {
                _maxPosition = _dataSource.Position;
                _maxVelocity = _dataSource.Velocity;
                _maxRotation = _dataSource.Rotation;
                _maxAngVelocity = _dataSource.AngVelocity;
                _maxVel = _curVel;
            }

            if (_SuccessCondition())
            {
                _delegate.MaxPosition = _maxPosition;
                _delegate.MaxVelocity = _maxVelocity;
                _delegate.MaxRotation = _maxRotation;
                _delegate.MaxAngVelocity = _maxAngVelocity;
            }
        }

        protected override bool _SuccessCondition()
        {
            return _maxVel - _curVel > _targetDelta;
        }

        protected override bool _FailCondition()
        {
            return false;
        }

        public interface IDelegate
        {
            Vector3 MaxPosition { get; set; }
            Vector3 MaxVelocity { get; set; }
            Quaternion MaxRotation { get; set; }
            Quaternion MaxAngVelocity { get; set; }
        }
    }
}
