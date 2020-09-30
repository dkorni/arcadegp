using Assets.Scripts.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.States
{
    /// <summary>
    /// Shoot 3 times, one to player direction and 2 others
    /// in right and left direction of 30 degrees from first
    /// </summary>
    /// <seealso cref="Assets.Scripts.Controllers.States.State" />
    public class Сannonade :State
    {
        public Сannonade(INpc characterController) : base(characterController)
        {
        }

        public override IEnumerator Process()
        {
            yield return null;

            _characterController.Animator.SetTrigger("Fireball");

            // rotate npc to player
            _characterController.Transform.rotation = Quaternion.LookRotation(
                _characterController.EnemyTarget.Transform.position - _characterController.Transform.position);

            // make shoot to player
            _characterController.Transform.GetComponent<FireBallGun>().Shoot<IPlayerController>(
                _characterController.EnemyTarget.Transform.position+new Vector3(0,0.5f,0),
                _characterController.Force, _characterController.Damage,
                _characterController.Transform.GetComponent<Collider>());

            // shoot to left
            ShootWithAngle(30f * Mathf.Deg2Rad);

            // shoot to right
            ShootWithAngle(30f * Mathf.Deg2Rad);
            ShootWithAngle(-30f * Mathf.Deg2Rad);

            _isFinished = true;
        }

        /// <summary>
        /// Shoots with  specific angle.
        /// </summary>
        private void ShootWithAngle(float angle)
        {
            var thisPosition = _characterController.Transform.position;
            var playerPosition = _characterController.EnemyTarget.Transform.position;

            var this2dPosition = new Vector2(thisPosition.x, thisPosition.z);
            var player2dPosition = new Vector2(playerPosition.x, playerPosition.z);

            var distance = Vector2.Distance(this2dPosition, player2dPosition);

            // calculate angle between player and npc
            var angleBtwThisAndPlayer = Mathf.Asin((player2dPosition.y - this2dPosition.y) / distance);

            // calculate new angle based on npc pose
            var newAngle = angleBtwThisAndPlayer + angle;

            // get new coordinates
            var newX = Mathf.Cos(newAngle) * distance;
            var absX = thisPosition.x + newX * Mathf.Sign(player2dPosition.x - thisPosition.x);

            var newZ = Mathf.Sin(newAngle) * distance;
            var absZ = thisPosition.z + newZ;

            var newTargetPos = new Vector3(absX, playerPosition.y, absZ);

            // make shoot to target
            _characterController.Transform.GetComponent<FireBallGun>().Shoot<IPlayerController>(
                newTargetPos + new Vector3(0, 0.5f, 0),
                _characterController.Force, _characterController.Damage,
                _characterController.Transform.GetComponent<Collider>());
        }
    }
}
