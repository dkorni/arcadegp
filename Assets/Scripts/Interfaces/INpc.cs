using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INpc : ICharacterController, IComparable
{
    /// <summary>
    /// Gets or sets the enemy target.
    /// </summary>
    /// <value>
    /// The enemy target.
    /// </value>
    ICharacterController EnemyTarget { get; set; }

    /// <summary>
    /// Stops moving of npc.
    /// </summary>
    void StopMove();
}
