using UnityEngine;

/// <summary>
/// Fire Bal lGun
/// </summary>
public class FireBallGun : MonoBehaviour
{
    [SerializeField]
    private GameObject _fireBall;

    [SerializeField] private Transform _fireballSpawn;

    private AudioSource AudioSource
    {
        get
        {
            if(_audioSource == null)
                _audioSource = GetComponent<AudioSource>();
            return _audioSource;
        }
    }

    private AudioSource _audioSource;

    /// <summary>
    /// Shoots fireball by the specified direction.
    /// </summary>
    /// <param name="direction">The direction.</param>
    /// <param name="force">The force.</param>
    public void Shoot<T>(Vector3 target, float force, float damage, Collider colliderOfShooter) where T:ICharacterController
    {
        _audioSource = GetComponent<AudioSource>();

        var fireBall = Instantiate(_fireBall, _fireballSpawn.position, Quaternion.identity);
        var rigidbody = fireBall.GetComponent<Rigidbody>();

        var collider = fireBall.GetComponent<Collider>();
        var collisionTrgigger = fireBall.GetComponent<CollisionTrigger>();

        // subscribe damage method to fireball collision trigger
        collisionTrgigger.OnTriggerEnterEvent.AddListener((c) =>
        {
            c.transform.GetComponent<T>()?.SetDamage(damage);
            Debug.Log($"{transform.name} hit {c.transform.name}");
            Destroy(fireBall);
        });

        // make some collider ignored for collision with spawned fireball
        // todo rename IgnoreFireBallColliders
        foreach (var ignoreFireBallCollider in GameManager.Instance.IgnoreFireBallColliders)
        {
            Physics.IgnoreCollision(collider, ignoreFireBallCollider);
        }

        // we need ignore self collision
        Physics.IgnoreCollision(collider, colliderOfShooter);

        // calculate direction to target
        var direction = target - fireBall.transform.position;
        rigidbody.AddForce(direction*force, ForceMode.Impulse);

        AudioSource.PlayOneShot(_audioSource.clip);
    }
}
