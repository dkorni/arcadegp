using System;
using Assets.Scripts.Interfaces;
using System.Collections;

namespace Assets.Scripts.Controllers.States
{
    public abstract class State : IState
    {
        /// <summary>
        /// The character controller which will be processed
        /// </summary>
        protected INpc _characterController;

        /// <summary>
        /// Occurs when [on reset].
        /// </summary>
        public event Action OnReset;

        protected State(INpc characterController)
        {
            _characterController = characterController;
        }

        /// <summary>
        /// Gets  a value indicating whether this state is finished.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this state is finished; otherwise, <c>false</c>.
        /// </value>
        public bool IsFinished => _isFinished;

        protected bool _isFinished;

        /// <summary>
        /// Processes some operation.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public abstract IEnumerator Process();

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            _isFinished = false;
            OnReset?.Invoke();
        }
    }
}