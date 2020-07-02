using System;
using System.Collections;
using UnityEngine;
using Dimar.Gestures.Fragments;
using Dimar.Gestures.Conditions;

namespace Dimar.Gestures
{
    /// <summary>
    /// Нажать кнопку и совершить бросательное движение.
    /// Бросок будет совершен в момент отпускания кнопки.
    /// В объекте будут сохранены данные на момент максимальной скорости
    /// в период когда была зажата кнопка.
    /// </summary>
    public class ButtonThrow : GestureBase,
                                WaitAngVelocityStartReduce.IDelegate
    {
        private KeyCode _button;

        public override event Action onFire;
        public override event Action onStart;
        public override event Action onBrake;

        private GestureBase.IDataSource _controller;
        /// <summary>
        /// Угловая скорость, при превышении которой бросок следует распознавать. В градусах за секунду
        /// </summary>
        private const float _critAngleVelocity = 800f;

#region OBSERVATION_VARIABLES
        Vector3 _maxPosition = Vector3.zero;
        Vector3 _maxVelocity = Vector3.zero;
        Quaternion _maxRotation = Quaternion.identity;
        Quaternion _maxAngVelocity = Quaternion.identity;
#endregion

#region DELEGATE_FIELDS
        public Vector3 MaxPosition
        {
            get => _maxPosition;
            set => _maxPosition = value;
        }
        public Vector3 MaxVelocity
        {
            get => _maxVelocity;
            set => _maxVelocity = value;
        }
        public Quaternion MaxRotation
        {
            get => _maxRotation;
            set => _maxRotation = value;
        }
        public Quaternion MaxAngVelocity
        {
            get => _maxAngVelocity;
            set => _maxAngVelocity = value;
        }
#endregion

        /// <param name="button">Controller button to fire</param>
        /// <param name="controller">Datasource to read the gesture</param>
        public ButtonThrow(KeyCode button, GestureBase.IDataSource controller)
        {
            _button = button;
            _controller = controller;
        }

        public override void ClearEventCallbacks()
        {
            onFire = null;
            onStart = null;
            onBrake = null;
        }

        // private YieldInstruction _yieldObject = null;
        public override IEnumerator StartReceiving()
        {
            var chain = _MakeChain();

            while (true)
            {
                yield return ReceiveGesture(chain);
            }
        }

        protected IEnumerator ReceiveGesture(IChainable[] chain)
        {
            foreach (var unit in chain)
            {
                yield return unit.Run();

                if (unit.Failed())
                {
                    onBrake?.Invoke();
                    yield break;
                }
            }
        }

        private IChainable[] _MakeChain()
        {
            IChainable[] chain = new IChainable[] {
                new ParallelFragment(EComposeType.All, EComposeType.Any,
                    new SequenceFragment(
                        // wait button down
                        new WaitButton(EButtonWaitMode.down, _button),
                        // invoke start
                        new CustomAction(() => { onStart?.Invoke(); }),
                        // wait button up
                        new WaitButton(EButtonWaitMode.up, _button),
                        // inovke fire
                        new CustomAction(() => { onFire?.Invoke(); })
                    ),
                    new SequenceFragment(
                        // new WaitCriticalVelocity(_controller, _critAngleVelocity),
                        new WaitAngVelocityStartReduce(_controller, this, 180f),
                        new WaitCustom(() => {return false;})
                    )
                ),
            };

            return chain;
        }
    }
}
