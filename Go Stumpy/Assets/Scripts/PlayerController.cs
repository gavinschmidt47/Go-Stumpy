using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Public Variables
    public float baseSpeed = 5f;
    public float baseJumpHeight = 5f;
    public float maxSuckTime = 10f;
    public InputAction movement;
    public InputAction jump;
    public InputAction useAbility;
    public AbilitiesScript abilities;

    //Private Variables
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool abilityOn = false;
    private bool usingAbility = false;
    private float speed;
    private float jumpHeight;

    //Enable Input Actions
    private void OnEnable()
    {
        movement.Enable();
        jump.Enable();
        useAbility.Enable();
    }

    //Disable Input Actions
    private void OnDisable()
    {
        movement.Disable();
        jump.Disable();
        useAbility.Disable();
    }

    //Start and Update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Disable Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //set speed and jump height
        speed = baseSpeed;
        jumpHeight = baseJumpHeight;
    }

    void Update()
    {
        //Read Input
        moveDirection = movement.ReadValue<Vector2>();
        if (jump.triggered && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
        
        if (useAbility.IsPressed())
        {
            StartCoroutine(UseAbility());
            usingAbility = true;
        }
        else
        {
            usingAbility = false;
        }
    }

    void FixedUpdate()
    {
        //Move Player
        rb.velocity = new Vector2(moveDirection.x * speed, rb.velocity.y);
    }
    
    //Kirby Ability
    private IEnumerator UseAbility()
    {
        Debug.Log("Ability Used");
        //Detect Enemy
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 10f);

        //Deactivate Previous Ability
        if (abilityOn)
        {
            abilityOn = false;
            speed = baseSpeed;
            jumpHeight = baseJumpHeight;
        }

        //Activate Ability
        if (hit.collider != null && hit.collider.CompareTag("Jumpy"))
        {
            jumpHeight += abilities.jumpBoost;
            abilities.jumpParticles.Play();
            abilityOn = true;
        }
        else if (hit.collider != null && hit.collider.CompareTag("Speedy"))
        {
            speed += abilities.speedBoost;
            abilities.speedParticles.Play();
            abilityOn = true;
        }

        yield return new WaitForSeconds(maxSuckTime);
    }
}
