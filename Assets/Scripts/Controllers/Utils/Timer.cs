using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Controllers.Utils
{
    /// <summary>
    /// Timer utility
    /// </summary>
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// The time
        /// </summary>
        [SerializeField]
        private float _time;

        /// <summary>
        /// The on time event
        /// </summary>
        [SerializeField]
        private UnityEvent _onTime;

        // run timer
        private void Start()
        {
            WaitForAndRun(_time, _onTime.Invoke);
        }

        /// <summary>
        /// Run task action after some seconds.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static IEnumerator WaitForAndRun(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }
    }
}
