using System;
using System.Reflection;
using UnityEngine;
using Dimar.Gestures.Fragments;

namespace Dimar.Gestures.Debugging.Fragments
{
    public class MovementDirectionChangeGizmo : FragmentGizmo
    {
        private Type _fragmentType = typeof(WaitMovementDirectionChange);
        public override void DrawGizmo()
        {
            var fr = _fragment as WaitMovementDirectionChange;
            var curRot = (Vector3)_fragmentType.GetField("_curRot", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var prevRot = (Vector3)_fragmentType.GetField("_prevRot", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var axisDeltaPrev = (Vector3)_fragmentType.GetField("_axisDeltaPrev", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var axisDeltaCur = (Vector3)_fragmentType.GetField("_axisDeltaCur", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr);
            var success = (bool)_fragmentType.GetMethod("_SuccessCondition", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(fr, new object[0]);

            curRot = curRot.normalized * 0.3f;
            prevRot = prevRot.normalized * 0.3f;
            axisDeltaPrev = axisDeltaPrev.normalized * 0.3f;
            axisDeltaCur = axisDeltaCur.normalized * 0.3f;

            var lineColor = success ? Color.green : Color.magenta;
            Gizmos.color = lineColor;
            Gizmos.DrawLine(transform.position, transform.position + curRot);
            Gizmos.DrawLine(transform.position + curRot, transform.position + curRot + axisDeltaCur);

            lineColor = Color.Lerp(lineColor, Color.white, 0.5f);
            Gizmos.color = lineColor;
            // Gizmos.DrawLine(transform.position, transform.position + prevRot);
            Gizmos.DrawLine(transform.position + curRot, transform.position + curRot + axisDeltaPrev);
        }
    }
}