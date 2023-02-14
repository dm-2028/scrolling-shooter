using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * 5.0f * Time.deltaTime);
        if (transform.position.y > 5.0f)// || transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }
    }
}
