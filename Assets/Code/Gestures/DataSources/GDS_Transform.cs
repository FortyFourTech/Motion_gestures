using System;
using System.Collections;
using UnityEngine;

namespace Dimar.Gestures
{
    public class GDS_Transform : MonoBehaviour, GestureBase.IDataSource
    {
#region Params
        [Header("Parameters")]
        [SerializeField] float alphaPositionInFrame = 0.3f;
        [SerializeField] float alphaVelocityInFrame = 0.1f;
        [SerializeField] float alphaRotationInFrame = 0.3f;
        [SerializeField] float alphaAngVelocityInFrame = 0.1f;
#endregion

#region Refs
        [Header("Refs")]
        [SerializeField] private Transform _controller;
#endregion

#region PRIVATE_FIELDS
        private Transform _cameraMarker = null;
        // protected DebugTools.GraphCanvas _angleVelocityGraph;
#endregion

#region DELEGATE_FIELDS
        public Vector3 Position => _smoothedPosition;
        public Vector3 Velocity => _smoothedVelocity;
        public Quaternion Rotation => _smoothedRotation;
        public Quaternion AngVelocity => _smoothedAngVelocity;
#endregion

#region OBSERVATION_VARIABLES
        Vector3 _curPosition = Vector3.zero;
        Quaternion _curRotation = Quaternion.identity;
        Vector3 _smoothedPosition = Vector3.zero;
        Vector3 _smoothedVelocity = Vector3.zero;
        Quaternion _smoothedRotation = Quaternion.identity;
        Quaternion _smoothedAngVelocity = Quaternion.identity;
#endregion

        private void Update()
        {
            if (_cameraMarker != null)
                _cameraMarker.rotation = _curRotation;
        }

        private void FixedUpdate()
        {
            _Recalc();
        }

        public void Setup(Transform controller)
        {
            _controller = controller;
        }

        private void _Recalc()
        {
            // position and velocity
            // var prevPos = _curPosition; // just in case
            _curPosition = _controller.position;

            var dPos = _curPosition - _smoothedPosition;
            // var prevSmoothedPos = _smoothedPosition; // just in case
            _smoothedPosition = Vector3.Lerp(Vector3.zero, dPos, alphaPositionInFrame) + _smoothedPosition;
            _smoothedVelocity = Vector3.Lerp(Vector3.zero, dPos, alphaVelocityInFrame) + Vector3.Lerp(Vector3.zero, _smoothedVelocity, 1 - alphaVelocityInFrame);

            // rotation and angular velocity
            var prevRot = _curRotation;
            _curRotation = _controller.rotation;

            var dRot = _curRotation * Quaternion.Inverse(_smoothedRotation);
            var prevSmoothedRot = _smoothedRotation;
            _smoothedRotation = Quaternion.Lerp(Quaternion.identity, dRot, alphaRotationInFrame) * _smoothedRotation;//Quaternion.Lerp(Quaternion.identity, _curRotation, alphaRotationInFrame) * Quaternion.Lerp(Quaternion.identity, _smoothedRotation, 1 - alphaRotationInFrame);
            _smoothedAngVelocity = Quaternion.Lerp(Quaternion.identity, dRot, alphaAngVelocityInFrame) * Quaternion.Lerp(Quaternion.identity, _smoothedAngVelocity, 1 - alphaAngVelocityInFrame);

            _DrawAxisPath(prevRot, _curRotation, false);
            _DrawAxisPath(prevSmoothedRot, _smoothedRotation, true);
        }

        #region DEBUG_THINGS
        public void SetupControllerAvatar(Transform cam)
        {
            var markerParent = cam.CreateChild("controller avatar");
            markerParent.localPosition = new Vector3(0, -0.3f, 1.3f);
            markerParent.localScale = Vector3.one * 0.1f;
            var markerGO = GameObject.Instantiate((Resources.Load("ObjectTransformMarker") as GameObject), markerParent);
            _cameraMarker = markerGO.transform;
        }

        private void _DrawAxisPath(Quaternion qOld, Quaternion qNew, bool light)
        {
            Color lineColor = default(Color); Vector3 lineStart = default(Vector3), lineEnd = default(Vector3);
            Action<Color> SetLineColor = (Color c) =>
            {
                lineColor = c;
                if (light) lineColor = Color.Lerp(lineColor, Color.white, 0.5f);
            };
            Action<Vector3> SetLinePoints = (Vector3 axis) =>
            {
                lineStart = qOld * axis * 0.3f;
                lineEnd = qNew * axis * 0.3f;
            };
            Action DrawLine = () =>
            {
                Debug.DrawLine(lineStart, lineEnd, lineColor, 0.5f);
            };

            SetLineColor(Color.red);
            SetLinePoints(Vector3.right);
            DrawLine();

            SetLineColor(Color.blue);
            SetLinePoints(Vector3.forward);
            DrawLine();

            SetLineColor(Color.green);
            SetLinePoints(Vector3.up);
            DrawLine();
        }
        #endregion
    }
}