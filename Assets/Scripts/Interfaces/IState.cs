using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Interfaces
{
    public interface IState
    {
        /// <summary>
        /// Gets  a value indicating whether this state is finished.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this state is finished; otherwise, <c>false</c>.
        /// </value>
        bool IsFinished { get; }

        /// <summary>
        /// Processes some operation.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        IEnumerator Process();

        /// <summary>
        /// Resets this state.
        /// </summary>
        void Reset();
    }
}
