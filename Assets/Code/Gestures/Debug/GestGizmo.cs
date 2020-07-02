using UnityEngine;

namespace Dimar.Gestures.Debugging
{
    /// <summary>
    /// Класс, помогающий дебажить жесты, рисуя кастомный гизмо в редакторе.
    /// </summary>
    public abstract class GestGizmo : MonoBehaviour
    {
        protected GestureBase _gest;
        public void SetGesture(GestureBase gesture)
        {
            _gest = gesture;
        }
        protected abstract void OnDrawGizmos();
    }
}
