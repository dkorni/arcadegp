using Assets.Scripts;
using Assets.Scripts.Controllers.States;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class GolemSpawnController : SpawnBaseController<INpc>
{
    [Inject(Id = "GolemBoss")]
    public override CharacterStateInfo StateInfo { get; set; }

    /// <summary>
    /// Creates the character on the scene and returns controller.
    /// </summary>
    /// <returns></returns>
    protected override INpc CreateCharacterController()
    {
        var prefab = Resources.Load<GameObject>("NPCs/Golem_Boss");
        var go = Instantiate(prefab, transform.position, Quaternion.identity);
        var characterController = go.GetComponent<GolemBossController>();
        characterController.CollisionTrigger = go.GetComponent<CollisionTrigger>();

        // Inject dependencies
        characterController.NavMeshAgent = go.GetComponent<NavMeshAgent>();
        characterController.NavMeshAgent.acceleration = StateInfo.Speed;
        characterController.Animator = go.GetComponent<Animator>();
        characterController.AudioSource = go.GetComponent<AudioSource>();

        // setup behaviour
        var playerTrigger = go.GetComponent<CollisionTrigger>();

        var npc = (INpc)characterController;
        npc.EnemyTarget = GameManager.Instance.PlayerController;

        // disable npc controller after death of player
        npc.EnemyTarget.OnDie.AddListener((c) => npc.Animator.SetTrigger("Victory"));
        npc.EnemyTarget.OnDie.AddListener((c) => Destroy(characterController));
        npc.EnemyTarget.OnDie.AddListener((c) => Destroy(playerTrigger));

        // run golem
        characterController.States = new IState[]
        {
            new RushMove(characterController),
            new QueueAttack(characterController),
            new Сannonade(characterController)
        };
        characterController.SelectState();

        return characterController;
    }
}
