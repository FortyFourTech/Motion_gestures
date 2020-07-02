/*
using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Аналог WaitButton для использования с Oculus
    /// </summary>
    public class WaitButton_OVR : GestFragment
    {
        public delegate bool Condition();
        private Condition _condition;
        private bool _isMet = false;
        
        private OVRInput.Button _button;
        private EButtonWaitMode _mode;
        
        public WaitButton_OVR(EButtonWaitMode mode, OVRInput.Button button)
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
                    button = OVRInput.GetDown(_button);
                    break;
                case EButtonWaitMode.up:
                    button = OVRInput.GetUp(_button);
                    break;
                case EButtonWaitMode.hold:
                    button = OVRInput.Get(_button);
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
}
*/
