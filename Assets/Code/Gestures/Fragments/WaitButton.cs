using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Ожидание взаимодействия с кнопкой.
    /// </summary>
    public class WaitButton : GestFragment
    {
        public delegate bool Condition();
        private Condition _condition;
        private bool _isMet = false;
        
        private KeyCode _button;
        private EButtonWaitMode _mode;
        
        public WaitButton(EButtonWaitMode mode, KeyCode button)
        {
            _mode = mode;
            
            _button = button;
        }

        public override void Reset()
        {
            _isMet = false;
        }

        protected override void _Calc()
        {
            bool button = false;
            switch (_mode)
            {
                case EButtonWaitMode.down:
                    button = Input.GetKeyDown(_button);
                    break;
                case EButtonWaitMode.up:
                    button = Input.GetKeyUp(_button);
                    break;
                case EButtonWaitMode.hold:
                    button = Input.GetKey(_button);
                    break;
            }

            _isMet = button;
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

    public enum EButtonWaitMode
    {
        down,
        up,
        hold,
    }
}
