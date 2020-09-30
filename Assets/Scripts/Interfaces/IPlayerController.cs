using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface IPlayerController : ICharacterController
    {
        /// <summary>
        /// Adds the enemy.
        /// </summary>
        /// <param name="npcEnemy">The NPC enemy.</param>
        void AddEnemy(INpc npcEnemy);

        /// <summary>
        /// Removes the enemy.
        /// </summary>
        /// <param name="npcEnemy">The NPC enemy.</param>
        void RemoveEnemy(INpc npcEnemy);
    } 
}
