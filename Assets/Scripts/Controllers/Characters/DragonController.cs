using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers.Characters;
using Assets.Scripts.Controllers.Utils;
using Assets.Scripts.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

public class DragonController : NpcBaseController
{

    public Dictionary<string, float> Bounds = new Dictionary<string, float>();

    public FireBallGun FireBallGun;

    private Coroutine _moveCoroutine;

    private void Start()
    {
        Move();
    }

    /// <summary>
    /// Moves this instance.
    /// </summary>
    public override void Move()
    {
        var newPosition = new Vector3(Random.Range(Bounds["Left"], Bounds["Right"]), transform.position.y,
            Random.Range(Bounds["Bottom"], Bounds["Top"]));

        // calculate direction to new position
        var direction = newPosition-transform.position;
        
        Animator.SetBool("IsMove", true);

        // rotate dragon to target
        transform.rotation = Quaternion.LookRotation(direction);

        // start move coroutine
        _moveCoroutine = StartCoroutine(MoveTo(newPosition));
    }

    /// <summary>
    /// Attacks this instance.
    /// </summary>
    public override void Attack()
    {
        // calculate direction to player
        var direction = EnemyTarget.Transform.position - transform.position;
        direction.y = 0;

        Animator.SetTrigger("Attack");

        transform.rotation = Quaternion.LookRotation(direction);
        FireBallGun.Shoot<IPlayerController>(EnemyTarget.Transform.position, _force, _damage, GetComponent<Collider>());

        StartCoroutine(Timer.WaitForAndRun(1, Move));
    }

    /// <summary>
    /// Dies this instance.
    /// </summary>
    public override void Die()
    {
        Animator.SetTrigger("Die");
        if(_moveCoroutine!=null)
            StopCoroutine(_moveCoroutine); 
        gameObject.AddComponent<Rigidbody>();
        Destroy(this);
    }

    /// <summary>
    /// Moves dragon to point each frame.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns></returns>
    private IEnumerator MoveTo(Vector3 point)
    {
        while (Vector3.Distance(transform.position, point) > 0)
        {
            // move dragon to new position
            transform.position = Vector3.MoveTowards(transform.position, point, _speed * Time.deltaTime);

            yield return null;
        }

        Animator.SetBool("IsMove", false);

        StopCoroutine(_moveCoroutine);

        // start attack after 1 second
        StartCoroutine(Timer.WaitForAndRun(1, Attack));
    }

   
}
