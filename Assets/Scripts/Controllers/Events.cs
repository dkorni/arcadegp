using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Events 
{
    /// <summary>
    /// On Spawn event
    /// </summary>
    /// <seealso cref="UnityEngine.Events.UnityEvent{ICharacterController}" />
    public class CharacterEvent : UnityEvent<ICharacterController> { }

    /// <summary>
    /// On Trigger event
    /// </summary>
    /// <seealso cref="UnityEngine.Events.UnityEvent{Collider}" />
    public class OnTriggerEvent : UnityEvent<Collision> { }

    /// <summary>
    /// On Property Changed Event
    /// </summary>
    /// <seealso cref="UnityEngine.Events.UnityEvent{System.Object}" />
    public class OnPropertyChangedEvent : UnityEvent<object> { }
}
