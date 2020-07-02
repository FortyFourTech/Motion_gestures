using System;
using System.Reflection;
using UnityEngine;
using Dimar.Gestures.Fragments;

namespace Dimar.Gestures.Debugging.Fragments
{
    public class AxisMovingGizmo : FragmentGizmo
    {
        private Type _fragmentType = typeof(AxisMoving);

        public override void DrawGizmo()
        {
            var fr = _fragment as AxisMoving;
            var curRot = (Quaternion)_fragmentType.GetField("_rotCur", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var prevRot = (Quaternion)_fragmentType.GetField("_rotPrev", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var prevAxis = (Vector3)_fragmentType.GetField("_axisPrev", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var curAxis = (Vector3)_fragmentType.GetField("_axisCur", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var fail = (bool)_fragmentType.GetMethod("_FailCondition", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(fr, new object[0]);
            var axisDelta = (float)_fragmentType.GetField("_axisDelta", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var rotDelta = (float)_fragmentType.GetField("_rotDelta", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);

            var curRotVect = curRot * Vector3.forward;
            var prevRotVect = prevRot * Vector3.up;
            prevAxis = prevAxis.normalized;
            curAxis = curAxis.normalized;

            var dAxis = curAxis - prevAxis;
            var dRot = curRotVect - prevRotVect;

            var deviation = rotDelta - axisDelta;

            var lineColor = fail ? Color.magenta : Color.green;
            Gizmos.color = Color.Lerp(lineColor, Color.black, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + curRotVect);
            Gizmos.DrawLine(transform.position + curRotVect, transform.position + curRotVect + dAxis);

            Gizmos.color = Color.Lerp(lineColor, Color.white, 0.7f);
            Gizmos.DrawLine(transform.position + curRotVect, transform.position + curRotVect + dRot);

            lineColor = Color.magenta;
            Gizmos.color = lineColor;
            Gizmos.DrawLine(transform.position + curRotVect, transform.position + curRotVect + Vector3.left * deviation);
        }
    }
}
