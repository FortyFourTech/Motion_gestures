/*
using UnityEngine;
using Dimar.Gestures;

namespace Dimar.Gestures
{
    /// <summary>
    /// Gesture data source made of oculus controller.
    /// </summary>
    public class ControllerMotion : GestureBase.IDataSource
    {
        private OVRInput.Controller _controller;
        private Component _parent;
        public ControllerMotion(OVRInput.Controller controller, Component parent)
        {
            _controller = controller;
            _parent = parent;
        }

        private Transform _vrRigRoot;
        private Transform _VRRigRoot
        {
            get
            {
                if (_vrRigRoot == null)
                {
                    OvrAvatar ovrAvatar = _parent.GetComponentInParent<OvrAvatar>();
                    if (ovrAvatar != null)
                    {
                        if (ovrAvatar.transform.parent != null) _vrRigRoot = ovrAvatar.transform.parent;
                        else _vrRigRoot = ovrAvatar.transform;
                    }
                    else
                    {
                        _vrRigRoot = _parent.transform.root;
                    }
                }
                return _vrRigRoot;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return _VRRigRoot.rotation * OVRInput.GetLocalControllerVelocity(_controller);
            }
        }

        public Quaternion AngVelocity
        {
            get
            {
                return _VRRigRoot.rotation * Quaternion.Euler(OVRInput.GetLocalControllerAngularVelocity(_controller));
            }
        }

        public Vector3 Position
        {
            get
            {
                return _VRRigRoot.TransformPoint(OVRInput.GetLocalControllerPosition(_controller));
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return _VRRigRoot.rotation * OVRInput.GetLocalControllerRotation(_controller);
            }
        }
    }
}
*/
