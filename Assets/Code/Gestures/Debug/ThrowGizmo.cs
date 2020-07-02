using System;
using System.Reflection;
using UnityEngine;
using Dimar.Gestures.Fragments;
using Dimar.Gestures.Debugging.Fragments;

namespace Dimar.Gestures.Debugging
{
    public class ThrowGizmo : GestGizmo
    {
        private Type _gestType = typeof(Throw);

        public Transform _t1;
        public Transform _t2;
        public Transform _t3;
        public Transform _t4;

        AxisMovingGizmo _axisMoveGizmo;
        ParallelGizmo _parallelGizmo;
        string parallelResult;

        private void Start()
        {
            _t1 = this.CreateChild("_t1");
            _t2 = this.CreateChild("_t2");
            _t3 = this.CreateChild("_t3");
            _t4 = this.CreateChild("_t4");

            var gest = _gest as Throw;
            GestFragment fr = _gestType.GetField("_axisMoveFr", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gest) as AxisMoving;
            var tr = this.CreateChild(fr.GetType().ToString() + " gizmo");
            var axisMovingDrawer = tr.gameObject.AddComponent<AxisMovingGizmo>();
            axisMovingDrawer.SetFragment(fr);
            _axisMoveGizmo = axisMovingDrawer;

            fr = _gestType.GetField("_parallel", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gest) as ParallelFragment;
            tr = this.CreateChild(fr.GetType().ToString() + " gizmo");
            var parallelDrawer = tr.gameObject.AddComponent<ParallelGizmo>();
            parallelDrawer.SetFragment(fr);
            _parallelGizmo = parallelDrawer;
        }

        protected override void OnDrawGizmos()
        {
            _axisMoveGizmo.DrawGizmo();
            _parallelGizmo.DrawGizmo();

            var gest = _gest as Throw;
            var dataSource = _gestType.GetField("_controller", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gest) as GDS_Transform;
            var parallel = _gestType.GetField("_parallel", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gest) as ParallelFragment;
            parallelResult = parallel.Succeeded() ? "succeeded" : "";
            parallelResult = parallel.Failed() ? "failed" : "";
            var vect1 = dataSource.Rotation; //a1.rotation;
            var vect2 = (Quaternion)typeof(GDS_Transform).GetField("_curRotation", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dataSource); ; //vect1 * Quaternion.AngleAxis(90,Vector3.forward) * Quaternion.AngleAxis(20,Vector3.right);
            _t1.rotation = vect1;
            _t2.rotation = vect2;

            Vector3 dir;
            gest.ThrowDir(vect1, out dir);

            _t4.SetPositionAndRotation(
                _t1.position + _t1.forward * 0.3f,
                Quaternion.FromToRotation(Vector3.forward, dir)
            );

            Draw.TransformAxis(_t1, true);
            Draw.TransformAxis(_t2, false);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(_t4.position, _t4.position + _t4.forward * 0.3f);
        }
    }
}
