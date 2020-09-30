using Assets.Scripts;
using Events;
using UnityEngine;

public abstract class SpawnBaseController<T> : MonoBehaviour where T:ICharacterController
{
    public CharacterEvent OnSpawn = new CharacterEvent();

    public abstract CharacterStateInfo StateInfo { get; set; }

    /// <summary>
    /// Spawns specific character and notify subscribers about it.
    /// </summary>
    public void Spawn()
    {
        var character = CreateCharacterController();

        // setup 
        character.Health = StateInfo.Health;
        character.Damage = StateInfo.Damage;
        character.Force = StateInfo.Force;
        character.Speed = StateInfo.Speed;

        // Inject dependencies
        character.Animator = character.Transform.GetComponent<Animator>();

        // notify subscribers that character is created
        OnSpawn.Invoke(character);
    }

    /// <summary>
    /// Creates the character on the scene and returns controller.
    /// </summary>
    /// <returns></returns>
    protected abstract T CreateCharacterController();
}
