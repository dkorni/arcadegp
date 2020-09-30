using System.Collections;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : CharacterBaseController, IPlayerController
{
    public Rigidbody Rigidbody;

    public FireBallGun FireBallGun;

    private SortedList<INpc> _enemies = new SortedList<INpc>();

    private Coroutine _attackCoroutine;

    private bool _isMove;

    /// <summary>
    /// Move character in space
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Move()
    {
        // get input axis to set direction 
        var horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");
        var verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");

        // set direction
        var direction = new Vector3(horizontalAxis,0,verticalAxis);

        // move physically position of player 
        Rigidbody.MovePosition(Rigidbody.position+direction * _speed * Time.fixedDeltaTime);

        // rotate player to direction
        if(direction != Vector3.zero) 
            transform.rotation = Quaternion.LookRotation(direction);

        // control animations
        if (horizontalAxis != 0 || verticalAxis != 0)
        {
            if (!Animator.GetBool("IsMoving"))
                Animator.SetBool("IsMoving", true);
            if(!_isMove)
                _isMove = true;
        }
        else
        {
            Animator.SetBool("IsMoving", false);
            _isMove = false;
        }
    }

    /// <summary>
    /// Attack enemy
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Attack()
    {
        if (_attackCoroutine == null)
            _attackCoroutine = StartCoroutine(Attacking());
    }

    /// <summary>
    /// Call death of current character
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Die()
    {
        Animator.SetTrigger("OnDeath");
        StopAttack();
        enabled = false;
    }

    /// <summary>
    /// Adds the enemy.
    /// </summary>
    /// <param name="npcEnemy">The NPC enemy.</param>
    public void AddEnemy(INpc npcEnemy)
    {
        _enemies.Add(npcEnemy);
    }

    /// <summary>
    /// Removes the enemy.
    /// </summary>
    /// <param name="npcEnemy">The NPC enemy.</param>
    public void RemoveEnemy(INpc npcEnemy)
    {
        _enemies.Remove(npcEnemy);
    }

    private IEnumerator Attacking()
    {
        while (true)
        {
            // update enemies list to get first near target
            _enemies.Sort();
            var npc = _enemies.GetFirstOrDefault();

            if (npc != null)
            {
                // get direction to npc
                var direction = npc.Transform.position - transform.position;

                direction.y = 0;

                // rotate player to npc
                transform.rotation = Quaternion.LookRotation(direction);

                Animator.SetTrigger("OnAttack");

                // shoot fireball to enemy
                FireBallGun.Shoot<INpc>(npc.Transform.position, _force, Damage, GetComponent<Collider>());
            }
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Stops the attack.
    /// </summary>
    private void StopAttack()
    {
        if (_attackCoroutine != null)
        {
            // stop attacking when there isn't any enemy
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if player doesn't move and enemies count > 0, start attack
        if (!_isMove)
        {
            if (_enemies.Count > 0)
            {
                Attack();
            }
            else 
                StopAttack();
        }
        else
            StopAttack();
    }

    
    void FixedUpdate()
    {
        Move();
    }
}
