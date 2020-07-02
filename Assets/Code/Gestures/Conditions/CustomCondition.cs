using System.Collections;

namespace Dimar.Gestures.Conditions
{
    class CustomCondition : IChainable
    {
        public delegate bool Condition();
        private Condition _condition;

        public CustomCondition(Condition condition)
        {
            _condition = condition;
        }

        public IEnumerator Run()
        {
            yield break;
        }

        public bool Failed()
        {
            return !_condition();
        }

        public bool Succeeded()
        {
            return _condition();
        }
    }
}
