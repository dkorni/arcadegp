using Assets.Scripts.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.States
{
    /// <summary>
    /// Start queue of attack with interval of 200 ms, then move to random point
    /// </summary>
    /// <seealso cref="Assets.Scripts.Controllers.States.State" />
    public class QueueAttack : State
    {
        public QueueAttack(INpc characterController) : base(characterController)
        {
        }

        /// <summary>
        /// Processes some operation.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Process()
        {
            var shootCount = 3;
            _characterController.Animator.SetTrigger("Fireball");
            while (shootCount > 0)
            {
                // rotate npc to player
                _characterController.Transform.rotation = Quaternion.LookRotation(
                    _characterController.EnemyTarget.Transform.position - _characterController.Transform.position);

                shootCount--;

                // make shoot to player
                _characterController.Transform.GetComponent<FireBallGun>().Shoot<IPlayerController>(
                    _characterController.EnemyTarget.Transform.position + new Vector3(0, 0.5f, 0),
                    _characterController.Force, _characterController.Damage,
                    _characterController.Transform.GetComponent<Collider>());

                yield return new WaitForSeconds(0.2f);
            }

            // move to random point of nav mesh
            var possiblePoints = GameManager.Instance.PossibleNavMeshPoints;
            _characterController.Move(possiblePoints[Random.Range(0, possiblePoints.Length-1)]);
            _isFinished = true;
        }
    }
}
