using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject laserPrefab;

    public ParticleSystem explosion;
    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 targetPos = new(transform.position.x, transform.position.y - 2.5f, transform.position.z);
        coroutine = Move(transform.position, targetPos, 1.4f);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > 5.0f || transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }    
    }

    IEnumerator Move(Vector3 startPos, Vector3 targetPos, float speed)
    {
        float i = 0.0f;
        float rate = 1.0f / speed;
        while(i < 1.0)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, targetPos, i);
            yield return null;
        }
        Explode();
    }
    void Explode()
    {
        Instantiate(laserPrefab, transform.position, transform.rotation * Quaternion.Euler(45, 90, 0));
        Instantiate(laserPrefab, transform.position, transform.rotation * Quaternion.Euler(135, 90, 0));
        Instantiate(laserPrefab, transform.position, transform.rotation * Quaternion.Euler(225, 90, 0));
        Instantiate(laserPrefab, transform.position, transform.rotation * Quaternion.Euler(315, 90, 0));
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
