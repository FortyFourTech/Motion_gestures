using System;
using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Выполнение кастомного действия.
    /// Выполняется на первом после запуска шаге.
    /// Завершается на последующей проверке - успехом.
    /// </summary>
    public class CustomAction : GestFragment
    {
        private Action _function;
        private bool _executed = false;

        public CustomAction(Action func)
        {
            _function = func;
        }

        public override void Reset()
        {
            _executed = false;
        }

        protected override void _Calc()
        {
            _function();
            _executed = true;
        }

        protected override bool _FailCondition()
        {
            return false;
        }

        protected override bool _SuccessCondition()
        {
            return _executed;
        }
    }
}
