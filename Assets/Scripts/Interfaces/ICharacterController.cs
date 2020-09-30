using Events;
using System.Collections;
using UnityEngine;

public interface ICharacterController
{
    /// <summary>
    /// Gets or sets the animator.
    /// </summary>
    /// <value>
    /// The animator.
    /// </value>
    Animator Animator { get; set; }

    /// <summary>
    /// Gets or sets the transform.
    /// </summary>
    /// <value>
    /// The transform.
    /// </value>
    Transform Transform { get; }

    /// <summary>
    /// Health value of character
    /// </summary>
    float Health { get; set; }

    /// <summary>
    /// Speed value of character movement
    /// </summary>
    float Speed { get; set; }

    /// <summary>
    /// Gets or sets the damage.
    /// </summary>
    /// <value>
    /// The damage.
    /// </value>
    float Damage { get; set; }

    /// <summary>
    /// The force of attack of character
    /// </summary>
    /// <value>
    /// The force.
    /// </value>
    float Force { get; set; }

    /// <summary>
    /// Notify subscribes that health of character is changed
    /// </summary>
    /// <value>
    /// The on health changed.
    /// </value>
    OnPropertyChangedEvent OnHealthChanged { get; }

    CharacterEvent OnDie { get; }

    /// <summary>
    /// Move character in space
    /// </summary>
    void Move();

    /// <summary>
    /// Move character to target
    /// </summary>
    void Move(Vector3 target);

    /// <summary>
    /// Attack enemy
    /// </summary>
    void Attack();

    /// <summary>
    /// Run some attack job for some time
    /// </summary>
    /// <param name="attackCoroutine">The attack coroutine.</param>
    Coroutine Attack(IEnumerator attackCoroutine);

    /// <summary>
    /// Set damage to current character. Reduce amount of health.
    /// </summary>
    /// <param name="damage"></param>
    void SetDamage(float damage);

    /// <summary>
    /// Call death of current character
    /// </summary>
    void Die();
}
