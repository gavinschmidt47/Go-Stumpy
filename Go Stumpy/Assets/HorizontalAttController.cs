using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAttController : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationalSpeed = 10.0f;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
        rb.angularVelocity = rotationalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
