using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;

    virtual protected int pointValue { get { return 50; } }
    virtual protected float speed { get { return 1.0f; } }
    virtual protected float gunCooldownMin {  get { return .9f; } }
    virtual protected float gunCooldownMax { get { return 1.1f; } }
    [SerializeField] protected float gunCooldown;
    private bool isColliding;

    public GameObject healthObject;
    public GameObject powerupObject;
    public ParticleSystem explosion;

    [SerializeField] public GameObject projectile;

    private GameManager gameManager;

    private void Reset()
    {
        health = 20;
        gunCooldown = Random.Range(0.0f, 1.0f);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Reset();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        isColliding = false;
        Move();
        Shoot();
        if(health <= 0)
        {
            int spawn = Random.Range(0, 25);
            if(spawn <= 2)
            {
                Debug.Log("Spawn health");
                Instantiate(healthObject, transform.position, transform.rotation);
            }else if(spawn <= 4){
                Instantiate(powerupObject, transform.position, transform.rotation);
            }
            gameManager.addScore(pointValue);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if(transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }


    protected virtual void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected virtual void Shoot()
    {
        if(gunCooldown > 0)
        {
            gunCooldown -= Time.deltaTime;
        }
        else
        {
            gunCooldown = Random.Range(gunCooldownMin, gunCooldownMax);
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y-1, transform.position.z), Quaternion.Euler(180, 0, 0));
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && !isColliding)
        {
            isColliding = true;
            Destroy(other.gameObject);
            health--;
        }
        if(other.CompareTag("Missile") && !isColliding)
        {
            isColliding = true;
            Destroy(other.gameObject);
            health -= 5;
        }
    }

}
