using System.Collections;
using Assets.Scripts.Controllers.Characters;
using UnityEngine;
using UnityEngine.AI;

public class SliderController : NpcBaseController
{
    public NavMeshAgent NavMeshAgent;

    private Coroutine _attackCoroutine;

    [SerializeField]
    private AudioClip _deathAudioClip;

    /// <summary>
    /// Move character in space
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Move()
    {
        if (EnemyTarget != null)
        {
            NavMeshAgent.SetDestination(EnemyTarget.Transform.position);
            if(!Animator.GetBool("Walk Forward"))
                Animator.SetBool("Walk Forward", true);
        }
    }

    /// <summary>
    /// Attack enemy, starts attacking coroutine
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Attack()
    {
        _attackCoroutine = StartCoroutine(Attacking());
    }

    /// <summary>
    /// Call death of current character
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Die()
    {
       Animator.SetTrigger("Die");
       AudioSource.PlayOneShot(_deathAudioClip);
       NavMeshAgent.enabled = false;
       GetComponent<Collider>().enabled = false;
       enabled = false;
    }

    /// <summary>
    /// Stops attacking coroutine.
    /// </summary>
    public void StopAttack()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
    }

    /// <summary>
    /// Attackings coroutine that is called every 1 sec.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attacking()
    {
        while (true)
        {
            Animator.SetTrigger("Smash Attack");
            EnemyTarget.SetDamage(_damage);
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
