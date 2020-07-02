using UnityEngine;
using Dimar.Gestures.Fragments;

namespace Dimar.Gestures.Debugging.Fragments
{
    public abstract class FragmentGizmo : MonoBehaviour
    {
        protected GestFragment _fragment;
        
        public void SetFragment(GestFragment fragment)
        {
            _fragment = fragment;
        }

        public abstract void DrawGizmo();
    }
}