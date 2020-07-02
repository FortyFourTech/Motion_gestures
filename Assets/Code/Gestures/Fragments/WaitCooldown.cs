using System;
using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// ВременнАя задержка.
    /// </summary>
    public class WaitCooldown : GestFragment
    {
        private float _timeDelta;
        private float _startTime;
        private float _timeCur;

        public WaitCooldown(GestureBase.IDataSource dataSource, float timeDelta)
        {
            _dataSource = dataSource;
            _timeDelta = timeDelta;

            _startTime = Time.time;
            _timeCur = 0f;
        }

        protected override void _Calc()
        {
            _timeCur = Time.time - _startTime;
        }

        protected override bool _SuccessCondition()
        {
            return _timeCur > _timeDelta;
        }

        protected override bool _FailCondition()
        {
            return false;
        }
    }
}
