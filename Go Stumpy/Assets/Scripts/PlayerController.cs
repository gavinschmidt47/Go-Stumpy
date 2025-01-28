using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Public Variables
    public float speed = 5f;
    public float jumpHeight = 5f;
    public InputAction movement;
    public InputAction jump;
    public InputAction useAbility;
    
    public AbilitiesScript abilities;

    //Private Variables
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    
    private void OnEnable()
    {
        movement.Enable();
        jump.Enable();
        useAbility.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        jump.Disable();
        useAbility.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDirection = movement.ReadValue<Vector2>();
        if (jump.triggered && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, rb.velocity.y);
    }
    
    //Kirby Ability
    private void UseAbility()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 10f);
        if (hit.collider != null && hit.collider.CompareTag("Jumpy"))
        {
            rb.velocity = new Vector2(rb.velocity.x, abilities.jumpBoost);
            abilities.jumpParticles.Play();
        }
        else if (hit.collider != null && hit.collider.CompareTag("Speedy"))
        {
            speed += abilities.speedBoost;
            abilities.speedParticles.Play();
        }
    }
}
