using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{

    private Vector3 startPos;
    private float width;
    private float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        width = GetComponent<BoxCollider>().size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);

        if (transform.position.y < startPos.y - width)
        {
            transform.position = startPos;
        }
    }
}
