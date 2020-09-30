using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class SliderSpawnController : SpawnBaseController<INpc>
{
    [Inject(Id="Slider")]
    public override CharacterStateInfo StateInfo { get; set; }

    /// <summary>
    /// Creates the character on the scene and returns controller.
    /// </summary>
    /// <returns></returns>
    protected override INpc CreateCharacterController()
    {
        var prefab = Resources.Load<GameObject>("NPCs/Slider");
        var go = Instantiate(prefab, transform.position, Quaternion.identity);
        var characterController = go.GetComponent<SliderController>();

        // Inject dependencies
        characterController.NavMeshAgent = go.GetComponent<NavMeshAgent>();
        characterController.NavMeshAgent.speed = StateInfo.Speed;
        characterController.AudioSource = go.GetComponent<AudioSource>();

        // setup behaviour
        var playerTrigger = go.GetComponent<CollisionTrigger>();
        playerTrigger.OnTriggerEnterEvent.AddListener((c)=>
        {
            if(c.transform.GetComponent<PlayerController>())
                characterController.Attack();
        });
        playerTrigger.OnTriggerExitEvent.AddListener((c)=>
        {
            if(c.transform.GetComponent<PlayerController>())
                characterController.StopAttack();
        });

        var npc = (INpc)characterController;
        npc.EnemyTarget = GameManager.Instance.PlayerController;

        // disable npc controller after death of player
        npc.EnemyTarget.OnDie.AddListener((c)=>Destroy(characterController));
        npc.EnemyTarget.OnDie.AddListener((c) => Destroy(playerTrigger));
        return characterController;
    }
}
