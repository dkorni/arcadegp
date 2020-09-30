using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Controllers.Spawns;
using Assets.Scripts.Interfaces;
using System.Linq;
using System.Text;
using Assets.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Gets the singleton.
    /// </summary>
    /// <value>
    /// The instance.
    /// </value>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Gets or sets the player controller.
    /// </summary>
    /// <value>
    /// The player controller.
    /// </value>
    public IPlayerController PlayerController { get; set; }

    public Collider[] IgnoreFireBallColliders
    {
        get
        {
            return GameObject.FindGameObjectsWithTag("IgnoreFireBallCollider")
                .Select(g => g.GetComponent<Collider>()).ToArray();
        }
    }

    public Vector3[] PossibleNavMeshPoints { get; private set; }

    [SerializeField]
    private AudioSource _bgMusic;

    [SerializeField]
    private GameObject _deathPopup;

    [SerializeField]
    private GameObject _winPopup;

    /// <summary>
    /// The player spawn
    /// </summary>
    private PlayerSpawnController _playerSpawn;

    /// <summary>
    /// The spawns
    /// </summary>
    [SerializeField] private SpawnBaseController<INpc>[] _npcSpawns;

    /// <summary>
    /// The UI controllers
    /// </summary>
    private IUiController[] _uiControllers;

    /// <summary>
    /// The enemy count
    /// </summary>
    private int _enemyCount;

    /// <summary>
    /// Restarts game.
    /// </summary>
    public void Restart() => SceneManager.LoadScene(0);

    /// <summary>
    /// Quit application.
    /// </summary>
    public void Quit() => Application.Quit();

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        PossibleNavMeshPoints = NavMesh.CalculateTriangulation().vertices;
        _playerSpawn = FindObjectOfType<PlayerSpawnController>();
        _uiControllers = FindObjectOfType<Canvas>().GetComponentsInChildren<IUiController>();
        _npcSpawns = FindObjectsOfType<SpawnBaseController<INpc>>();

        // notify manager that player have been already spawned
        _playerSpawn.OnSpawn.AddListener((c)=>
        {
            PlayerController = (IPlayerController)c;
            PlayerController.OnDie.AddListener((ch)=>_deathPopup.SetActive(true));
            SpawnNpcs();

            // start job of UI controllers
            StartAllUi();
        });

        // spawn player
        _playerSpawn.Spawn();
    }
    private void StartAllUi()
    {
        // run UI controllers
        foreach (var uiController in _uiControllers)
        {
            uiController.StartUI();
        }
    }

    private void SpawnNpcs()
    {
        // spawn npc
        foreach (var spawn in _npcSpawns)
        {
            // Register new enemy in player controller
            spawn.OnSpawn.AddListener((e) =>
            {
                PlayerController.AddEnemy((e.Transform.GetComponent<INpc>()));
                _enemyCount++;
            });

            // Remove enemy from player when it dies
            spawn.OnSpawn.AddListener((e) =>
                e.OnDie.AddListener((npc) =>
                {
                    PlayerController.RemoveEnemy((npc.Transform.GetComponent<INpc>()));
                    ReduceEnemiesCount();
                }));
            spawn.Spawn();
        }
    }

    /// <summary>
    /// Reduces the enemies count.
    /// </summary>
    private void ReduceEnemiesCount()
    {
        _enemyCount--;
        if ((_npcSpawns.Length - _enemyCount) == _npcSpawns.Length)
        {
            _bgMusic.Stop();
            _winPopup.SetActive(true);
        }
    }

}