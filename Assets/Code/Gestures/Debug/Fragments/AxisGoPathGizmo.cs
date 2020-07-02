using System;
using System.Reflection;
using UnityEngine;
using Dimar.Gestures.Fragments;

namespace Dimar.Gestures.Debugging.Fragments
{
    public class AxisGoPathGizmo : FragmentGizmo
    {
        private Type _fragmentType = typeof(WaitAxisGoPath);

        private Transform _t1;
        private Transform _t2;
        private Transform _t3;
        private Transform _t4;

        private void Start()
        {
            _t1 = this.CreateChild(GetType().ToString() + "_t1");
            _t2 = this.CreateChild(GetType().ToString() + "_t2");
            _t3 = this.CreateChild(GetType().ToString() + "_t3");
            _t4 = this.CreateChild(GetType().ToString() + "_t4");
        }

        public override void DrawGizmo()
        {
            var fr = _fragment as WaitAxisGoPath;

            var axis = (Vector3)_fragmentType.GetField("_axis", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var vect1 = (Vector3)_fragmentType.GetField("_startAxisPos", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var vect2 = (Vector3)_fragmentType.GetField("_endAxisPos", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            _t1.rotation = Quaternion.FromToRotation(axis, vect1);
            _t2.rotation = Quaternion.FromToRotation(axis, vect2);
            var rotPath = (Quaternion)_fragmentType.GetField("_rotPath", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var dataSource = _fragmentType.GetField("_dataSource", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr) as GestureBase.IDataSource;
            _t4.rotation = rotPath;
            _t3.rotation = dataSource.Rotation;

            var proj = (Vector3)_fragmentType.GetField("curProjection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var toProj = (Vector3)_fragmentType.GetField("rotToProj", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);

            // var startPos = new Vector3(-0.1f,0f,0f);
            var startPos = Vector3.zero;
            _t1.position = startPos;
            _t2.position = startPos;
            _t3.position = startPos;
            _t4.position = startPos;

            var startEnd = _t1.rotation * axis * 0.3f;
            var endEnd = _t2.rotation * axis * 0.3f;
            var pathEnd = _t4.rotation * axis * 0.3f;
            var curEnd = _t3.rotation * axis * 0.3f;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(_t1.position, startEnd);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_t2.position, endEnd);
            // Gizmos.DrawLine(pathEnd, pathEnd + toEnd);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(_t4.position, pathEnd); // path
            Gizmos.DrawLine(pathEnd, proj * 0.3f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(_t3.position, curEnd); // current rotation
            Gizmos.DrawLine(startPos, proj * 0.3f);
            Gizmos.DrawLine(curEnd, curEnd + toProj * 0.3f);
        }
    }
}
