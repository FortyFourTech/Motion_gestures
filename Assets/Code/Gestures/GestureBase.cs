using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dimar.Gestures
{
    /// <summary>
    /// Базовый класс для написания жестов.
    /// </summary>
    public abstract class GestureBase : IGesture
    {
        public abstract event Action onFire;
        public abstract event Action onStart;
        public abstract event Action onBrake;

        public abstract IEnumerator StartReceiving();
        public abstract void ClearEventCallbacks(); // possibly subject to remove

        protected Camera _SetupFrustum(Transform parent, GameObject prefab)
        {
            Transform frustumParent;
            var entailCamComponent = parent.GetComponent<Entailing>();
            if (entailCamComponent != null)
            {
                frustumParent = entailCamComponent.Entailed;
            }
            else
            {
                frustumParent = (new GameObject("gesture frustums")).transform;
                parent.gameObject.AddComponent<Entailing>().Entailed = frustumParent;
            }
            var frustumGO = GameObject.Instantiate(prefab, frustumParent);
            var frustumCamera = frustumGO.GetComponent<Camera>();
            frustumCamera.aspect = 1;

            return frustumCamera;
        }

        public interface IDataSource
        {
            Vector3 Position { get; }
            Vector3 Velocity { get; }
            Quaternion Rotation { get; }
            Quaternion AngVelocity { get; }
        }
    }
}
