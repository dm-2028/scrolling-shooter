using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    private float maxXMovement = 2.0f;
    private float maxXPosition;
    private float minXPosition;
    private int direction = 1;

    private void Start()
    {
        speed = 2.0f;
        gunCooldown = .6f;
        health = 15;
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
