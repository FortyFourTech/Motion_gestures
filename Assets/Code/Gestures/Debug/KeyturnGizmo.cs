using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Dimar.Gestures.Fragments;
using Dimar.Gestures.Debugging.Fragments;

namespace Dimar.Gestures.Debugging
{
    public class KeyturnGizmo : GestGizmo
    {
        private Type _gestType = typeof(Keyturn);

        private SequenceFragment _sequence;

        private Dictionary<object, FragmentGizmo> _gizmos = new Dictionary<object, FragmentGizmo>();

        private void Start()
        {
            var gest = _gest as Keyturn;
            _sequence = _gestType.GetField("_sequence", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gest) as SequenceFragment;
            var fragments = typeof(SequenceFragment).GetField("_fragments", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_sequence) as GestFragment[];
            foreach (var item in fragments)
            {
                var tr = this.CreateChild(item.GetType().ToString() + " gizmo");
                FragmentGizmo gizmoDrawer = null;
                if (item is WaitAxisGoPath)
                {
                    gizmoDrawer = tr.gameObject.AddComponent<AxisGoPathGizmo>();
                }
                else if (item is WaitMovementDirectionChange)
                {
                    gizmoDrawer = tr.gameObject.AddComponent<MovementDirectionChangeGizmo>();
                }

                if (gizmoDrawer != null)
                {
                    gizmoDrawer.SetFragment(item);
                    _gizmos[item] = gizmoDrawer;
                }
            }
        }

        protected override void OnDrawGizmos()
        {
            var fr = _sequence.CurrentFragment;
            FragmentGizmo gizmo;
            if (_gizmos.TryGetValue(fr, out gizmo))
                gizmo.DrawGizmo();
        }
    }
}
