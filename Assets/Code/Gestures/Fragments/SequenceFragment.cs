using System.Linq;
using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Композиция фрагментов, выполняющихся последовательно.
    /// </summary>
    public class SequenceFragment : GestFragment
    {
        private GestFragment[] _fragments;
        private GestFragment _curFragment;
        private EFragmentState[] _gestStates;

        public GestFragment CurrentFragment => _curFragment;

        public SequenceFragment(params GestFragment[] fragments)
        {
            _fragments = fragments;
        }

        public override void Reset()
        {
            foreach (var fr in _fragments)
            {
                fr.Reset();
            }
            _curFragment = _fragments.First();
            _gestStates = Enumerable.Repeat(EFragmentState.receiving, _fragments.Length).ToArray();
        }

        protected override void _Calc()
        {
            _curFragment.MoveNext();
            for (int i = 0; i < _fragments.Length; i++)
            {
                var fr = _fragments[i];
                if (_gestStates[i] == EFragmentState.receiving && fr.state != EFragmentState.receiving)
                {
                    _gestStates[i] = fr.state;

                    if (i < _fragments.Length - 1)
                    {
                        _curFragment = _fragments[i + 1];
                        _curFragment.Reset();
                    }
                }

                if (fr.state == EFragmentState.receiving)
                    break;
            }
        }

        protected override bool _SuccessCondition()
        {
            return _gestStates.All(x => x == EFragmentState.succeeded);
        }

        protected override bool _FailCondition()
        {
            return _gestStates.Any(x => x == EFragmentState.failed);
        }
    }
}
