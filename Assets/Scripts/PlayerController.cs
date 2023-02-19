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
    private float missileCooldown = .4f;

    private int health = 10;
    private int maxHealth = 10;
    [SerializeField] private static int powerLevel = 1;

    private bool powerupShield = true;

    public GameObject bullet;
    public GameManager gameManager;
    public GameObject missile;
    public AudioClip laserSound;
    public AudioClip missileSound;

    private AudioSource playerAudio;

    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
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
        if(missileCooldown > 0)
        {
            missileCooldown -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space) && gunCooldown <= 0)
        {
            gunCooldown = .1f;
            InstantiateBullets();
            if (powerLevel >= 4)
            {
                missileCooldown = .4f;
                playerAudio.PlayOneShot(missileSound);
                Instantiate(missile, new Vector3(transform.position.x, transform.position.y + .8f, transform.position.z), Quaternion.Euler(0, 0, 0));
            }
        }

    }

    private void InstantiateBullets()
    {
        playerAudio.PlayOneShot(laserSound);
        Instantiate(bullet, new Vector3(transform.position.x - .7f, transform.position.y+.5f, transform.position.z), Quaternion.Euler(0,0,0));
        Instantiate(bullet, new Vector3(transform.position.x + .7f, transform.position.y+.5f, transform.position.z), Quaternion.Euler(0, 0, 0));
        if(powerLevel >= 2)
        {
            Instantiate(bullet, new Vector3(transform.position.x - .75f, transform.position.y + .5f, transform.position.z), Quaternion.Euler(0, 0, 0));
            Instantiate(bullet, new Vector3(transform.position.x + .75f, transform.position.y + .5f, transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        if(powerLevel >= 3)
        {
            Instantiate(bullet, new Vector3(transform.position.x - .75f, transform.position.y + .5f, transform.position.z), Quaternion.Euler(0, 0, 30));
            Instantiate(bullet, new Vector3(transform.position.x + .75f, transform.position.y + .5f, transform.position.z), Quaternion.Euler(0, 0, -30));
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("oncollision " + collision.gameObject.name);
        Vector3 otherPosition = collision.gameObject.transform.position;
        Vector3 direction = transform.position - otherPosition;
        Debug.Log("direction: " + direction);
        LoseHealth(1);
        //playerRb.AddForce(direction*500.0f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            Destroy(other.gameObject);
            LoseHealth(1);
            if (!powerupShield && powerLevel > 1)
            {
                powerLevel--;
                
            }
            powerupShield = false;
            Invoke("ResetPowerupShield", 2.0f);
        }
        if (other.CompareTag("Bomb"))
        {
            Destroy(other.gameObject);
            LoseHealth(2);
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
        if (other.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            if(powerLevel < 4)
            {
                powerLevel++;
            }
        }
    }

    private void ResetPowerupShield()
    {
        powerupShield = true;
    }

    private void LoseHealth(int damage)
    {
        health -= damage;
        gameManager.UpdateHealth(health);
        if (health <= 0)
        {
            
            gameManager.ShipDestroyed();
            gameObject.SetActive(false);
            Invoke("ResetShip", 2.0f);
        }
    }

    private void ResetShip()
    {
        powerLevel = 1;
        transform.position = new Vector3(0, -4, 0);
        gameObject.SetActive(true);
        health = 10;
        gameManager.UpdateHealth(health);
    }
}
