using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 10;
    protected float speed = 1.0f;
    public float gunCooldown = .8f;

    [SerializeField] public GameObject projectile;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        if(health <= 0)
        {
            Destroy(gameObject);
        }else if(transform.position.y < -6)
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
            gunCooldown = 1f;
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y-1, transform.position.z), Quaternion.Euler(180, 0, 0));
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter");
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            health--;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision enter");
    }
}
