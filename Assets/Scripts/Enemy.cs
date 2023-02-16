using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int mHealth = 20;
    virtual protected int health{ get { return mHealth; } set { } }

    virtual protected int pointValue { get { return 50; } }
    virtual protected float speed { get { return 1.0f; } }
    virtual protected float gunCooldownMin {  get { return .9f; } }
    virtual protected float gunCooldownMax { get { return 1.1f; } }
    private float gunCooldown;
    private bool isColliding;

    public GameObject healthObject;

    [SerializeField] public GameObject projectile;

    private GameManager gameManager;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        gunCooldown = Random.Range(0.0f, 1.0f);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        isColliding = false;
        Move();
        Shoot();
        if(mHealth <= 0)
        {
            int healthSpawn = Random.Range(0, 10);
            if(healthSpawn == 0)
            {
                Debug.Log("Spawn health");
                Instantiate(healthObject, transform.position, transform.rotation);
            }
            gameManager.addScore(pointValue);
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

    private void Shoot()
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
        Debug.Log("trigger enter " + other.name);
        if (other.CompareTag("Projectile") && !isColliding)
        {
            Debug.Log("Compare projectile");
            isColliding = true;
            Destroy(other.gameObject);
            mHealth--;
        }
    }

}
