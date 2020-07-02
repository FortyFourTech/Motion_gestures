using System.Collections;
using UnityEngine;

namespace Dimar.Gestures.Conditions
{
    /// <summary>
    /// Совпадает ли вектор forward объекта с фрустумом камеры по вертикали.
    /// </summary>
    class CheckVerticalDirection : IChainable
    {
        private Camera _frustumCamera;
        private GestureBase.IDataSource _dataSource;

        public CheckVerticalDirection(GestureBase.IDataSource dataSource, Camera frustumCamera)
        {
            _frustumCamera = frustumCamera;
            _dataSource = dataSource;
        }

        public IEnumerator Run()
        {
            yield break;
        }

        private bool _Condition()
        {
            var curDirection = _dataSource.Rotation * Vector3.forward;
            var horizontal = Vector3.ProjectOnPlane(curDirection, Vector3.up);

            var angle = SignedAngle(horizontal, curDirection, Quaternion.AngleAxis(90f, Vector3.up) * horizontal);

            if (angle < _frustumCamera.transform.rotation.x + _frustumCamera.fieldOfView / 2 && angle > _frustumCamera.transform.rotation.x - _frustumCamera.fieldOfView / 2)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determine the signed angle between two vectors, with normal 'n'
        /// as the rotation axis.
        /// </summary>
        private float SignedAngle(Vector3 v1, Vector3 v2, Vector3 n)
        {
            return Mathf.Atan2(
                    Vector3.Dot(n, Vector3.Cross(v1, v2)),
                    Vector3.Dot(v1, v2)
                ) * Mathf.Rad2Deg;
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
            Debugging.Draw.ThrowDirection(_frustumCamera.transform.position, _dataSource.Rotation * Vector3.forward, success);
        }
    }
}
