using System.Collections;
using UnityEngine;

namespace Dimar.Gestures.Conditions
{
    /// <summary>
    /// Совпадает ли вектор forward объекта с фрустумом камеры.
    /// </summary>
    class DirectionInFrustum : IChainable
    {
        private Camera _frustumCamera;
        private GestureBase.IDataSource _dataSource;

        public DirectionInFrustum(GestureBase.IDataSource dataSource, Camera frustumCamera)
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
            var boxSize = 0.01f;
            var bounds = new Bounds(_frustumCamera.transform.position + _dataSource.Rotation * Vector3.forward, new Vector3(boxSize, boxSize, boxSize));
            if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(_frustumCamera), bounds))
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
            Debugging.Draw.ThrowDirection(_frustumCamera.transform.position, _dataSource.Rotation * Vector3.forward, success);
        }
    }
}
