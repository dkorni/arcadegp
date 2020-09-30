using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using Zenject;

public class DragonSpawnController : SpawnBaseController<INpc>
{
    [Inject(Id = "Dragon")]
    public override CharacterStateInfo StateInfo { get; set; }

    protected override INpc CreateCharacterController()
    {
        // load prefab
        var prefab = Resources.Load<GameObject>("NPCs/Dragon");

        // create npc on the scene
        var go = Instantiate(prefab, transform.position, Quaternion.identity);

        // setup
        var characterController = go.GetComponent<DragonController>();
        var bounds = GameObject.FindWithTag("BoundPlane").GetComponent<MeshCollider>().bounds;
        characterController.FireBallGun = go.GetComponent<FireBallGun>();

        // calculate bounds
        characterController.Bounds["Top"] = bounds.center.z + bounds.size.z / 2;
        characterController.Bounds["Bottom"] = bounds.center.z - bounds.size.z / 2;
        characterController.Bounds["Left"] = bounds.center.x - bounds.size.x / 2;
        characterController.Bounds["Right"] = bounds.center.x + bounds.size.x / 2;


        var collider = go.GetComponent<Collider>();

        // make some collider ignored for collision with spawned dragon
        // todo rename ignoreFireBallCollider to ignoreColliderForFlyingObjects
        foreach (var ignoreFireBallCollider in GameManager.Instance.IgnoreFireBallColliders)
        {
            Physics.IgnoreCollision(collider, ignoreFireBallCollider);
        }

        // set player enemy
        var npc = (INpc)characterController;
        npc.EnemyTarget = GameManager.Instance.PlayerController;

        // disable npc controller after death of player
        npc.EnemyTarget.OnDie.AddListener((c) => Destroy(characterController));

        return characterController;
    }
}
