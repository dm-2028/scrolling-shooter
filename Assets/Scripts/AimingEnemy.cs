using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingEnemy : Enemy
{
    private int mHealth = 25;
    protected override int health { get { return mHealth; } set { } }
    protected override int pointValue { get { return 100; } }
    protected override float speed { get { return 1.5f; } }
    protected override float gunCooldownMin { get { return .8f; } }
    protected override float gunCooldownMax { get { return 1f; } }
    private GameObject player;

    private float gunCooldown = .8f;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Shoot()
    {
        if (gunCooldown > 0)
        {
            gunCooldown -= Time.deltaTime;
        }
        else
        {
            if (transform.position.y > player.transform.position.y)
            {
                gunCooldown = Random.Range(gunCooldownMin, gunCooldownMax);
                Vector3 launchPosition = new(transform.position.x, transform.position.y - 1, transform.position.z);
                Vector3 direction = player.transform.position - launchPosition;

                Instantiate(projectile, launchPosition, Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0));
            }
        }

    }
}
