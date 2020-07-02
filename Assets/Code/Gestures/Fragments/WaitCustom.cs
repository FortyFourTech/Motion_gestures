using System;
using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Кастомная проверка.
    /// Ожидание успеха проверки.
    /// </summary>
    public class WaitCustom : GestFragment
    {
        public delegate bool Condition();
        private Condition _condition;
        private bool _isMet = false;

        public WaitCustom(Condition condition)
        {
            _condition = condition;
        }

        public override void Reset()
        {
            _isMet = false;
        }

        protected override void _Calc()
        {
            _isMet = _condition();
        }

        protected override bool _SuccessCondition()
        {
            return _isMet == true;
        }

        protected override bool _FailCondition()
        {
            return false;
        }
    }
}
