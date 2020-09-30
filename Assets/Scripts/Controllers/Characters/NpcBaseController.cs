using System;
using UnityEngine;

namespace Assets.Scripts.Controllers.Characters
{
    public abstract class NpcBaseController : CharacterBaseController, INpc
    {
        /// <summary>
        /// Gets or sets the enemy target.
        /// </summary>
        /// <value>
        /// The enemy target.
        /// </value>
        public ICharacterController EnemyTarget { get; set; }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops moving of npc.
        /// </summary>
        public virtual void StopMove(){}

        public override void Attack()
        {
            throw new NotImplementedException();
        }

        public override void Die()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compares this npc distance to enemy with other npc.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int CompareTo(object otherNpc)
        {
            if (otherNpc is INpc npc)
            {
                var thisDistance = Vector3.Distance(transform.position, EnemyTarget.Transform.position);
                var otherDistance = Vector3.Distance(npc.Transform.position, EnemyTarget.Transform.position);

                int result = 0; 

                if (thisDistance < otherDistance)
                    result = -1;
                if (thisDistance > otherDistance)
                    result = 1;

                return result;
            }
            else
            {
                throw new InvalidCastException("Compare object is not implements INpc");
            }
        }
    }
}
