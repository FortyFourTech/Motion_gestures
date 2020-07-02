using System;
using UnityEngine;
using Dimar.Gestures.Fragments;
using Dimar.Gestures.Debugging.Fragments;

namespace Dimar.Gestures.Debugging
{
    /// <summary>
    /// Утилиты для рисования гизмо жестов.
    /// </summary>
    public static class Draw
    {
        public static void ThrowDirection(Vector3 start, Vector3 direction, bool success)
        {
            var boxCenter = start + direction;
            var color = success ? Color.green : Color.magenta;
            Debug.DrawLine(start, boxCenter, color, 5f);

            var boxSize = 0.01f;
            Debug.DrawLine(boxCenter + new Vector3(boxSize, -boxSize, boxSize), boxCenter + new Vector3(boxSize, -boxSize, -boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(boxSize, -boxSize, boxSize), boxCenter + new Vector3(boxSize, boxSize, boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(boxSize, boxSize, boxSize), boxCenter + new Vector3(boxSize, boxSize, -boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(boxSize, boxSize, -boxSize), boxCenter + new Vector3(boxSize, boxSize, -boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(boxSize, -boxSize, boxSize), boxCenter + new Vector3(-boxSize, -boxSize, boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(boxSize, boxSize, boxSize), boxCenter + new Vector3(-boxSize, boxSize, boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(boxSize, boxSize, -boxSize), boxCenter + new Vector3(-boxSize, boxSize, -boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(boxSize, -boxSize, -boxSize), boxCenter + new Vector3(-boxSize, -boxSize, -boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(-boxSize, -boxSize, boxSize), boxCenter + new Vector3(-boxSize, boxSize, boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(-boxSize, boxSize, boxSize), boxCenter + new Vector3(-boxSize, boxSize, -boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(-boxSize, boxSize, -boxSize), boxCenter + new Vector3(-boxSize, -boxSize, -boxSize), color, 5f);
            Debug.DrawLine(boxCenter + new Vector3(-boxSize, -boxSize, -boxSize), boxCenter + new Vector3(-boxSize, -boxSize, boxSize), color, 5f);
        }

        public static void TransformAxis(Transform tr, bool light)
        {
            Action<Color> SetLineColor = (Color c) =>
            {
                var lineColor = c;
                if (light) lineColor = Color.Lerp(lineColor, Color.white, 0.5f);
                Gizmos.color = lineColor;
            };
            Action<Vector3> DrawLine = (Vector3 axis) =>
            {
                Gizmos.DrawLine(tr.position, tr.position + axis * 0.3f);
            };

            SetLineColor(Color.red);
            DrawLine(tr.right);

            SetLineColor(Color.blue);
            DrawLine(tr.forward);

            SetLineColor(Color.green);
            DrawLine(tr.up);
        }
    }

    public class DrawGesture : MonoBehaviour
    {
        public void DrawGizmo(GestureBase gesture)
        {
            if (gesture is Keyturn)
            {
                var tr = this.CreateChild("keyturn gizmo");
                var gizmoDrawer = tr.gameObject.AddComponent<KeyturnGizmo>();
                gizmoDrawer.SetGesture(gesture);
            }
            else if (gesture is Push)
            {
                var tr = this.CreateChild("push gizmo");
                var gizmoDrawer = tr.gameObject.AddComponent<PushGizmo>();
                gizmoDrawer.SetGesture(gesture);
            }
            else if (gesture is Throw)
            {
                var tr = this.CreateChild("throw gizmo");
                var gizmoDrawer = tr.gameObject.AddComponent<ThrowGizmo>();
                gizmoDrawer.SetGesture(gesture);
            }
        }
    }
}