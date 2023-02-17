using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : Enemy
{
    private int mHealth = 40;
    override protected int health { get { return mHealth; } set { } }
    override protected int pointValue { get { return 150; } }
    override protected float speed { get { return .8f; } }
    override protected float gunCooldownMin { get { return 1.8f; } }
    override protected float gunCooldownMax { get { return 2.1f; } }

}
