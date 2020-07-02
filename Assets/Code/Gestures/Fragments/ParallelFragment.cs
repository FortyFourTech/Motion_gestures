using System.Linq;
using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Композиция фрагментов, выполняющихся параллельно.
    /// </summary>
    public class ParallelFragment : GestFragment
    {
        private EComposeType _failCompose;
        private EComposeType _finishCompose;
        private GestFragment[] _fragments;
        private EFragmentState[] _gestStates;

        public ParallelFragment(EComposeType fail = EComposeType.Any, EComposeType finish = EComposeType.All, params GestFragment[] fragments)
        {
            _failCompose = fail;
            _finishCompose = finish;
            _fragments = fragments;
            _gestStates = Enumerable.Repeat(EFragmentState.receiving, fragments.Length).ToArray();
        }

        public override void Reset()
        {
            foreach (var fr in _fragments)
            {
                fr.Reset();
            }
            _gestStates = Enumerable.Repeat(EFragmentState.receiving, _fragments.Length).ToArray();
        }

        protected override void _Calc()
        {
            _fragments.Where(x => x.state == EFragmentState.receiving).ToList()
                .ForEach(x => x.MoveNext());
                
            for (int i = 0; i < _fragments.Length; i++)
            {
                var fr = _fragments[i];
                if (_gestStates[i] == EFragmentState.receiving && fr.state != EFragmentState.receiving)
                    _gestStates[i] = fr.state;
            }
        }

        protected override bool _SuccessCondition()
        {
            if (_finishCompose == EComposeType.All) return _gestStates.All(x => x == EFragmentState.succeeded);
            else return _gestStates.Any(x => x == EFragmentState.succeeded);
        }

        protected override bool _FailCondition()
        {
            if (_failCompose == EComposeType.Any) return _gestStates.Any(x => x == EFragmentState.failed);
            else return _gestStates.All(x => x != EFragmentState.receiving) && _gestStates.Any(x => x == EFragmentState.failed);
        }
    }

    public enum EComposeType
    {
        Any,
        All
    }
}
