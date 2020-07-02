using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Базовый класс для фрагментов жеста.
    /// </summary>
    public abstract class GestFragment : IEnumerator, IChainable
    {
        protected GestureBase.IDataSource _dataSource;

        public EFragmentState state
        {
            get
            {
                if (_SuccessCondition())
                    return EFragmentState.succeeded;
                else if (_FailCondition())
                    return EFragmentState.failed;
                else
                    return EFragmentState.receiving;
            }
        }
        public object Current => null;

        public bool MoveNext()
        {
            _Calc();
            return state == EFragmentState.receiving;
        }

        public virtual void Reset() { }

        /// <summary>
        /// Функция, вызываемая каждый тик, перед проаверками состояния.
        /// Все изменения следует вычислять здесь.
        /// </summary>
        protected abstract void _Calc();

        /// <summary>
        /// Проверка условия, при котором корутина должна завершиться успехом. 
        /// </summary>
        /// <returns>*true* чтобы завершить корутину, *false* для продолжения работы.</returns>
        protected abstract bool _SuccessCondition();

        /// <summary>
        /// Проверка условия, при котором корутина должна завершиться неудачей.
        /// </summary>
        /// <returns>*true* чтобы завершить корутину, *false* для продолжения работы.</returns>
        protected abstract bool _FailCondition();

        public IEnumerator Run()
        {
            Reset();
            yield return this;
        }

        public bool Succeeded()
        {
            return state == EFragmentState.succeeded;
        }

        public bool Failed()
        {
            return state == EFragmentState.failed;
        }
    }

    public enum EFragmentState
    {
        receiving,
        succeeded,
        failed,
    }
}
