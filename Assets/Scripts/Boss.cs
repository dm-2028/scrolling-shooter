using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Boss : MonoBehaviour
{
    private float maxXPosition = 5.0f;
    private float speed = 3.0f;

    public GameObject bombPrefab;
    public GameObject laserPrefab;

    private int health = 400;
    private int pointValue = 10000;
    private float bombCooldown;
    private bool leftGun;
    private float laserCooldown;
    private float seekingLaserCooldown = .4f;
    private float laserCooldownMin = .8f;
    private float laserCooldownMax = 1.1f;
    private Vector3 launchPosition;
    private bool isColliding;

    private GameObject player;
    private GameManager gameManager;

    public ParticleSystem explosion;

    public bool isActive { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isActive)
        {
            isColliding = false;
            Move();
            Shoot();
            if(health <= 0)
            {
                gameManager.addScore(pointValue);
                isActive = false;
                StartCoroutine("FadeOut");
            }
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position.x > maxXPosition || transform.position.x < -maxXPosition)
        {
            speed = -speed;
        }
    }

    private void Shoot()
    {
        if(bombCooldown > 0)
        {
            bombCooldown -= Time.deltaTime;
        }
        else
        {
            Instantiate(bombPrefab, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.Euler(180, 0, 0));
            bombCooldown = 1.5f;
        }
        if (laserCooldown > 0)
        {
            laserCooldown -= Time.deltaTime;
        }
        else 
        { 
            Instantiate(laserPrefab, new Vector3(transform.position.x + 1, transform.position.y - 1, transform.position.z), Quaternion.Euler(180, 0, 0));
            Instantiate(laserPrefab, new Vector3(transform.position.x - 1, transform.position.y - 1, transform.position.z), Quaternion.Euler(180, 0, 0));
            laserCooldown = Random.Range(laserCooldownMin, laserCooldownMax);
        }
        if (seekingLaserCooldown > 0)
        {
            seekingLaserCooldown -= Time.deltaTime;
        }
        else
        {
            if (leftGun)
            {
                launchPosition = new(transform.position.x+2.2f, transform.position.y - 1f, transform.position.z);
            }
            else
            {
                launchPosition = new(transform.position.x - 2.2f, transform.position.y - 1f, transform.position.z);
            }
            Vector3 direction = player.transform.position - launchPosition;
            leftGun = !leftGun;
            seekingLaserCooldown = Random.Range(.3f, .5f);
            Instantiate(laserPrefab, launchPosition, Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0));
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile") && !isColliding)
        {
            isColliding = true;
            Destroy(other.gameObject);
            health--;
        }
        if (other.CompareTag("Missile") && !isColliding)
        {
                isColliding = true;
                Destroy(other.gameObject);
                health -= 5;
        }
    }

    private IEnumerator FadeOut()
    {
        Debug.Log("Fading out");
        Color mColor = GetComponent<MeshRenderer>().material.color;
        float fadeLevel = mColor.a;
        float xValMax = transform.position.x + 3;
        float xValMin = transform.position.x - 3;
        float yValMin = transform.position.y + 1;
        float yValMax = transform.position.y - 2;
        
        while(fadeLevel > 0)
        {

            if (Random.Range(0, 20) == 0)
            {
                Instantiate(explosion, new Vector3(Random.Range(xValMin, xValMax), Random.Range(yValMin, yValMax), 0), transform.rotation);
            }
            Debug.Log("Fade Level " + fadeLevel);
            fadeLevel = fadeLevel - Time.deltaTime*.3f;
            GetComponent<MeshRenderer>().material.color = new Color(mColor.r, mColor.g, mColor.b, fadeLevel);
            yield return null;
        }

        gameManager.StartCoroutine("FinishLevel");
    }
}
