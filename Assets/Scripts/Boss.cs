using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float maxXPosition = 5.0f;
    private float speed = 3.0f;

    public GameObject bombPrefab;
    public GameObject laserPrefab;

    private int health = 10;
    private float bombCooldown;
    private bool leftGun;
    private float laserCooldown;
    private float seekingLaserCooldown = .4f;
    private float laserCooldownMin = .8f;
    private float laserCooldownMax = 1.1f;
    private Vector3 launchPosition;
    private bool isColliding;

    private GameObject player;

    public bool isActive { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        player = GameObject.FindGameObjectWithTag("Player");
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
    }

    private IEnumerator FadeOut()
    {
        Debug.Log("Fading out");
        Color mColor = GetComponent<MeshRenderer>().material.color;
        float fadeLevel = mColor.a;
        
        while(fadeLevel > 0)
        {
            Debug.Log("Fade Level " + fadeLevel);
            fadeLevel = fadeLevel - Time.deltaTime;
            mColor = new Color(mColor.r, mColor.g, mColor.b, fadeLevel);
            yield return null;
        }
    }
}
