using System.Collections;
using Events;
using UnityEngine;

public abstract class CharacterBaseController : MonoBehaviour, ICharacterController
{
    public AudioSource AudioSource;

    [SerializeField]
    private AudioClip _damageClip;

    /// <summary>
    /// Gets or sets the animator.
    /// </summary>
    /// <value>
    /// The animator.
    /// </value>
    public Animator Animator { get; set; }

    /// <summary>
    /// Gets or sets the transform.
    /// </summary>
    /// <value>
    /// The transform.
    /// </value>
    public Transform Transform
    {
        get
        {
            if (_transform == null) _transform = transform;
            return _transform;
        }
    }

    /// <summary>
    /// Health value of character
    /// </summary>
    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }

    /// <summary>
    /// Speed value of character movement
    /// </summary>
    public virtual float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    /// <summary>
    /// The force of attack of character
    /// </summary>
    /// <value>
    /// The force.
    /// </value>
    public float Force
    {
        get { return _force; }
        set { _force = value; }
    }

    [SerializeField] protected float _damage;

    [SerializeField] protected float _force;

    /// <summary>
    /// Notify subscribes that health of character is changed
    /// </summary>
    /// <value>
    /// The on health changed.
    /// </value>
    public OnPropertyChangedEvent OnHealthChanged
    {
        get
        {
            // lazy loading
            if (_onPropertyChanged == null) _onPropertyChanged = new OnPropertyChangedEvent();
            return _onPropertyChanged;
        }
    }

    public CharacterEvent OnDie
    {
        get
        {
            // lazy loading
            if (_onDie == null) _onDie = new CharacterEvent();
            return _onDie;
        }
    }

    protected OnPropertyChangedEvent _onPropertyChanged;

    protected CharacterEvent _onDie;

    [SerializeField] protected float _health;

    [SerializeField] protected float _speed;

    private Transform _transform;

    /// <summary>
    /// Move character in space
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// Move character to target
    /// </summary>
    /// <param name="target"></param>
    public virtual void Move(Vector3 target) { }

    /// <summary>
    /// Attack enemy
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// Run some attack job for some time
    /// </summary>
    /// <param name="attackCoroutine">The attack coroutine.</param>
    public Coroutine Attack(IEnumerator attackCoroutine)
    {
        return StartCoroutine(attackCoroutine);
    }

    /// <summary>
    /// Call death of current character
    /// </summary>
    public abstract void Die();

    /// <summary>
    /// Set damage to current character. Reduce amount of health.
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage(float damage)
    {
        _health -= damage;
        OnHealthChanged.Invoke(_health);

        if (_health <= 0)
        {
            Die();
            OnDie.Invoke(this);
        }
        else
        {
            if (_damageClip != null)
                AudioSource.PlayOneShot(_damageClip);
        }
    }
}