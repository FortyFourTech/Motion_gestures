using System.Collections;
using UnityEngine;

namespace Dimar.Gestures.Conditions
{
    /// <summary>
    /// Совпадает ли ось с заданным направлением.
    /// С учетом заданной погрешности в градусах.
    /// </summary>
    class CheckDirectionAlign : IChainable
    {
        private Transform _origin;
        private Vector3 _axisVector;
        private float _maxAngle;
        private GestureBase.IDataSource _dataSource;

        public CheckDirectionAlign(GestureBase.IDataSource dataSource, Vector3 axis, float maxAngle, Transform origin)
        {
            _origin = origin;
            _axisVector = axis;
            _maxAngle = maxAngle;
            _dataSource = dataSource;
        }

        public IEnumerator Run()
        {
            yield break;
        }

        private bool _Condition()
        {
            var curDirection = _dataSource.Rotation * Vector3.forward;
            var axisDirection = _origin.TransformDirection(_axisVector);

            var angle = Vector3.Angle(curDirection, axisDirection);

            if (angle < _maxAngle)
                return true;
            else
                return false;
        }

        public bool Failed()
        {
            var result = !_Condition();
            _DrawThrowDirection(!result);
            return result;
        }

        public bool Succeeded()
        {
            var result = _Condition();
            _DrawThrowDirection(result);
            return result;
        }

        private void _DrawThrowDirection(bool success)
        {
            Debugging.Draw.ThrowDirection(_origin.transform.position, _dataSource.Rotation * Vector3.forward, success);
        }
    }
}
