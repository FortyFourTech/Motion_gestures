using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Dimar.Gestures.Fragments;

namespace Dimar.Gestures.Debugging.Fragments
{
    public class ParallelGizmo : FragmentGizmo
    {
        [SerializeField] private EFragmentState[] _fragmentStates;
        private Type _fragmentType = typeof(ParallelFragment);

        public override void DrawGizmo()
        {
            var fr = _fragment as ParallelFragment;
            _fragmentStates = _fragmentType.GetField("_gestStates", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fr) as EFragmentState[];
        }
    }
}
