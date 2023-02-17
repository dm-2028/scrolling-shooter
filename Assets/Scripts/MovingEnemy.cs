using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    private int mHealth = 30;
    protected override int health { get { return mHealth; } set { } }
    protected override float speed { get { return 2.0f; } }
    protected override float gunCooldownMin { get { return .8f; } }
    protected override float gunCooldownMax { get { return 1f; } }
    protected override int pointValue { get { return 75; } }

    private float maxXMovement = 2.0f;
    private float maxXPosition;
    private float minXPosition;
    private int direction = 1;

    protected override void Start()
    {
        base.Start();
        maxXPosition = transform.position.x + maxXMovement;
        minXPosition = transform.position.x - maxXMovement;
    }

    protected override void Move()
    {
        base.Move();
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);
        if(transform.position.x > maxXPosition || transform.position.x < minXPosition)
        {
            direction = -direction;
        }

    }
}
