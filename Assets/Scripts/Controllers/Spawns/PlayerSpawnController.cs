using Assets.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Controllers.Spawns
{
    public class PlayerSpawnController : SpawnBaseController<IPlayerController>
    {
        [Inject(Id = "Player")]
        public override CharacterStateInfo StateInfo { get; set; }

        /// <summary>
        /// Creates the character on the scene and returns controller.
        /// </summary>
        /// <returns></returns>
        protected override IPlayerController CreateCharacterController()
        {
            var prefab = Resources.Load<GameObject>("Player");
            var go = Instantiate(prefab, transform.position, Quaternion.identity);
            var characterController = go.GetComponent<PlayerController>();

            // Inject dependencies
            characterController.Rigidbody = go.GetComponent<Rigidbody>();
            characterController.AudioSource = go.GetComponent<AudioSource>();
            characterController.FireBallGun = go.GetComponent<FireBallGun>();
            return characterController;
        }
    }
}
