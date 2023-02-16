using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    public float speed = 5.0f;

    private float maxXPos = 8.0f;
    private float maxYPos = 4.0f;
    private float gunCooldown = .1f;

    private int health = 10;
    private int maxHealth = 10;

    public GameObject bullet;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime +
            Vector3.forward * verticalInput * speed * Time.deltaTime);
        if (transform.position.x < -maxXPos)
        {
            transform.position = new Vector3(-maxXPos, transform.position.y, transform.position.z);
        }
        if (transform.position.x > maxXPos)
        {
            transform.position = new Vector3(maxXPos, transform.position.y, transform.position.z);
        }
        if (transform.position.y > maxYPos)
        {
            transform.position = new Vector3(transform.position.x, maxYPos, transform.position.z);
        }
        if (transform.position.y < -maxYPos)
        {
            transform.position = new Vector3(transform.position.x, -maxYPos, transform.position.z);
        }
    }

    private void HandleShooting()
    {
        if(gunCooldown > 0)
        {
            gunCooldown -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space) && gunCooldown <= 0)
        {
            gunCooldown = .1f;
            InstantiateBullets();
        }
    }

    private void InstantiateBullets()
    {
        Instantiate(bullet, new Vector3(transform.position.x - .7f, transform.position.y+.5f, transform.position.z), Quaternion.Euler(0,0,0));
        Instantiate(bullet, new Vector3(transform.position.x + .7f, transform.position.y+.5f, transform.position.z), Quaternion.Euler(0, 0, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            Destroy(other.gameObject);

            health--;
            gameManager.UpdateHealth(health);

            if (health <= 0)
            {
                gameManager.ShipDestroyed();
                Destroy(gameObject);
            }
        }
        if (other.CompareTag("Health"))
        {
            Debug.Log("Health Pickup");
            Destroy(other.gameObject);
            health += 2;
            if(health > maxHealth)
            {
                health = maxHealth;
            }
            gameManager.UpdateHealth(health);
        }
    }
}
