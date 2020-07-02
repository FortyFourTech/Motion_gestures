using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Ожидаем что ось объекта повернется на определенный кватернион,
    /// отклонившись по пути не более чем на заданную погрешность.
    /// </summary>
    public class WaitAxisGoPath : AxisFragment
    {
        private float _deviationMax; // в градусах
        private float _finishEps;
        private Vector3 _startAxisPos, _endAxisPos;
        private float _deviationCur, _remainAngle;
        private Quaternion _rotPath, _axisPath;

        // for debugging
        private Vector3 curProjection, rotToProj;

        public WaitAxisGoPath(GestureBase.IDataSource dataSource, Vector3 axis, Quaternion axisPath, float deviation, float finishEps) : base(dataSource, axis)
        {
            _deviationMax = deviation;
            _finishEps = finishEps;
            _axisPath = axisPath;

            Reset();
        }

        public override void Reset()
        {
            _deviationCur = 0f;
            _remainAngle = float.MaxValue;
            _startAxisPos = _dataSource.Rotation * _axis;
            _endAxisPos = _dataSource.Rotation * _axisPath * _axis;
            _rotPath = _dataSource.Rotation;
        }

        protected override void _Calc()
        {
            var pathVector = _rotPath * _axis;
            // найти плоскость движения
            var planeNormal = Vector3.Cross(pathVector, _endAxisPos);
            // проекция текущего вращения на плоскость
            var curRotVector = _dataSource.Rotation * _axis;
            curProjection = Vector3.ProjectOnPlane(curRotVector, planeNormal);
            // вектор движения к проекции текущего вращения
            var moveToProj = curProjection - pathVector;
            // вектор движения к цели
            var moveToEnd = _endAxisPos - pathVector;
            // угол между движением к проекции и движением к цели
            // если угол == 0
            if (Vector3.Angle(moveToEnd, moveToProj) < 90f)
            {
                // то vRPath = вращение от axis к проекции
                var newPathRot = Quaternion.FromToRotation(_axis, curProjection);
                _rotPath = newPathRot;

                pathVector = _rotPath * _axis;
            }
            // иначе угол 180

            // угол между точкой пройденного пути и текущим вращением
            _deviationCur = Vector3.Angle(pathVector, curRotVector);
            _remainAngle = Vector3.Angle(_endAxisPos, pathVector);
            rotToProj = curProjection - curRotVector;
        }

        protected override bool _SuccessCondition()
        {
            return _remainAngle < _finishEps;
        }

        protected override bool _FailCondition()
        {
            return _deviationCur > _deviationMax;
        }
    }
}
