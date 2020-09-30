using System;
using System.Collections;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Controllers.States
{
    /// <summary>
    /// Move character with double speed and apply damage to player when colliding
    /// </summary>
    /// <seealso cref="Assets.Scripts.Controllers.States.State" />
    public class RushMove : State
    {
        public RushMove(INpc characterController) : base(characterController)
        {
        }

        private event Action _onTriggerExit;

        /// <summary>
        /// Processes some operation.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IEnumerator Process()
        {
            // set double speed and move to player
            _characterController.Speed *= 2;
            _characterController.Move(_characterController.EnemyTarget.Transform.position);
            var renamingTime = 2;

            // setup collision trigger for attack player
            var characterOnTriggerEnterEvent =
                _characterController.Transform.GetComponent<CollisionTrigger>().OnTriggerEnterEvent;
            var characterOnTriggerExitEvent =
                _characterController.Transform.GetComponent<CollisionTrigger>().OnTriggerExitEvent;


            characterOnTriggerEnterEvent.AddListener((c) =>
            {
                if(c.transform.GetComponent<IPlayerController>()!=null)
                    _characterController.Attack(EnemyAttack(this));
            });

            characterOnTriggerExitEvent.AddListener((c)=> _onTriggerExit?.Invoke());

            // do this jub during renaming time
            while (renamingTime > 0)
            {
                renamingTime -= 1;
                yield return new WaitForSeconds(1);
            }

            // return state of character to stock and stop move
            characterOnTriggerEnterEvent.RemoveAllListeners();
            characterOnTriggerExitEvent.RemoveAllListeners();
            _characterController.Speed /= 2;
            _characterController.StopMove();
            _isFinished = true;
        }

        /// <summary>
        /// Enemy Attack job
        /// </summary>
        /// <returns></returns>
        private IEnumerator EnemyAttack(RushMove rushMove)
        {
            var wasStopped = false;

            // when reset or player doesn't collides with character, we must stop this coroutine
            OnReset += () => wasStopped = true;
            _onTriggerExit += () => wasStopped = true;

            while (!wasStopped)
            {
                _characterController.Animator.SetTrigger("Attack_1");
                _characterController.EnemyTarget.SetDamage(_characterController.Damage);

                yield return new WaitForSeconds(1);
            }
        }
    }
}
