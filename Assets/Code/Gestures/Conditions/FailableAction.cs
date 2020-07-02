using System;
using System.Collections;

namespace Dimar.Gestures.Conditions
{
    /// <summary>
    /// Выполнить действие перед проверкой.
    /// Результат проверки - результат действия. 
    /// </summary>
    class FailableAction : IChainable
    {
        private Func<bool> _checkFunction;
        private bool _checkResult = false;

        public FailableAction(Func<bool> function)
        {
            _checkFunction = function;
        }

        public IEnumerator Run()
        {
            _checkResult = _checkFunction();
            yield break;
        }

        public bool Failed()
        {
            return !_checkResult;
        }

        public bool Succeeded()
        {
            return _checkResult;
        }
    }
}
