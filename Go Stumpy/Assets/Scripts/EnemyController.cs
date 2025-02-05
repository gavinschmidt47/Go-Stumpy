using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Public Variables
    public GameInfo gameInfo;
    public ParticleSystem deathParticles;

    //Private Variables
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private string ID;
    private Transform player;
    private Vector2 currVel;
    private float thisSpeed;
    private float thisJump;

    // Start is called before the first frame update
    void Start()
    {
        ID = gameObject.name;   
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        player = GameObject.Find("Stumpy").transform;

        if (ID == "Jumpy")
        {
            thisSpeed = gameInfo.jumpyS;
            thisJump = gameInfo.jumpyH;
        }
        else if (ID == "Speedy")
        {
            thisSpeed = gameInfo.speedyS;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = (player.position - transform.position).normalized;
        
        if (ID == "Jumpy")
        {
            if (IsGrounded())
            {
                Debug.Log("Jump");
                rb.AddForce(Vector2.up * thisJump, ForceMode2D.Impulse);
            }
            rb.velocity = new Vector2(moveDirection.x * thisSpeed, rb.velocity.y);
        }
        else if (ID == "Speedy")
        {
            rb.velocity = new Vector2(moveDirection.x * thisSpeed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Mathf.Abs(rb.velocity.y) < 0.001f;
    }
}
