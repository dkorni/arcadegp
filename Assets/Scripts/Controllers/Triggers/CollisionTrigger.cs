using Events;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    public OnTriggerEvent OnTriggerEnterEvent = new OnTriggerEvent();
    public OnTriggerEvent OnTriggerExitEvent = new OnTriggerEvent();

    /// <summary>
    /// Called when [trigger enter].
    /// </summary>
    /// <param name="collider">The collider.</param>
    public void OnCollisionEnter(Collision collider)
    {
        // notify all subscribers about triggering
        OnTriggerEnterEvent.Invoke(collider);
    }

    /// <summary>
    /// Called when [trigger enter].
    /// </summary>
    /// <param name="collider">The collider.</param>
    public void OnCollisionExit(Collision collider)
    {
        // notify all subscribers about triggering
        OnTriggerExitEvent.Invoke(collider);
    }
}
