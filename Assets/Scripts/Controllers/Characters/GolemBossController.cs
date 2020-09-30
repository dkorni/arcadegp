using Assets.Scripts.Controllers.Characters;
using Assets.Scripts.Controllers.States;
using Assets.Scripts.Controllers.Utils;
using Assets.Scripts.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GolemBossController : NpcBaseController
{
    /// <summary>
    /// Speed value of character movement
    /// </summary>
    public override float Speed
    {
        get { return _speed;}
        set
        {
            _speed = value;
            NavMeshAgent.speed = value;
        }
    }

    /// <summary>
    /// The nav mesh agent
    /// </summary>
    public NavMeshAgent NavMeshAgent;

    /// <summary>
    /// The collision trigger
    /// </summary>
    public CollisionTrigger CollisionTrigger;

    /// <summary>
    /// Possible states of character
    /// </summary>
    public IState[] States;

    /// <summary>
    /// Moves this instance.
    /// </summary>
    public override void Move(Vector3 target)
    {
        NavMeshAgent.isStopped = false;
        NavMeshAgent.acceleration = _speed;
        Animator.SetBool("Move", true);
        AudioSource.Play();
        // move golem to player
        NavMeshAgent.SetDestination(target);
        CollisionTrigger.enabled = true;
        Debug.Log($"Now I am move to {target}");
    }

    /// <summary>
    /// Dies this instance.
    /// </summary>
    public override void Die()
    {
        Animator.SetTrigger("Die");
        NavMeshAgent.isStopped = true;
        GetComponent<CollisionTrigger>().enabled = false;
        Destroy(this);
    }

    /// <summary>
    /// Stops moving of npc.
    /// </summary>
    public override void StopMove()
    {
        NavMeshAgent.acceleration = _speed;
        Animator.SetBool("Move", false);
        AudioSource.Stop();
        NavMeshAgent.isStopped = true;
        CollisionTrigger.enabled = false;
    }

    /// <summary>
    /// Selects the state of golem and run it.
    /// </summary>
    public void SelectState()
    {
        var stateIndex = Random.Range(0, States.Length);
        var state = States[stateIndex];
        StartCoroutine(state.Process());
        StartCoroutine(WaitForFinishedState(state));
    }

    /// <summary>
    /// Waits the state of for finished.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns></returns>
    private IEnumerator WaitForFinishedState(IState state)
    {
        var startCamSize = Camera.main.orthographicSize;

        // camera shake effect when golem is move
        while (!state.IsFinished && Animator.GetBool("Move"))
        {
            Camera.main.orthographicSize = Random.Range(startCamSize - 2.5f, startCamSize);
            yield return null;
        }

        Camera.main.orthographicSize = startCamSize;

        // Reset current state
        state.Reset();

        // Wait for one second and start another state
        StartCoroutine(Timer.WaitForAndRun(1, SelectState));
    }

}
