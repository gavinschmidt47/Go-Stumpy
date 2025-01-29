using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Public Variables
    public float baseSpeed = 5f;
    public float baseJumpHeight = 5f;
    public float abilityDivisor = 2f;
    public InputAction movement;
    public InputAction jump;
    public InputAction useAbility;
    public AbilitiesScript abilities;
    public ParticleSystem suckParticles;

    //Private Variables
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool abilityOn = false;
    private bool usingAbility = false;
    private float speed;
    private float jumpHeight;
    private Vector2 faceDir = Vector2.right;

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
        
        //Facing Direction
        if (moveDirection.x > 0)
        {
            faceDir = Vector2.right;
            suckParticles.transform.rotation = Quaternion.Euler(180, -90, 0);
        }
        else if (moveDirection.x < 0)
        {
            faceDir = Vector2.left;
            suckParticles.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    void FixedUpdate()
    {
        //Move Player
        rb.velocity = new Vector2(moveDirection.x * speed, rb.velocity.y);

        //Use Ability
        if (useAbility.IsPressed())
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, faceDir, 10f);

            //First Ability Frame
            if (!usingAbility)
            {   
                usingAbility = true;
                suckParticles.Play();
                speed /= abilityDivisor;
                jumpHeight /= abilityDivisor;
            }
            
            //Activate Ability
            /*if (hit.collider != null && hit.collider.CompareTag("Jumpy"))
            {
                if(abilityOn)
                    ResetAbility();
                jumpHeight = abilities.jumpBoost + baseJumpHeight;
                abilities.jumpParticles.Play();
                abilityOn = true;
            }
            else if (hit.collider != null && hit.collider.CompareTag("Speedy"))
            {
                if(abilityOn)
                    ResetAbility();
                speed = abilities.speedBoost + baseSpeed;
                abilities.speedParticles.Play();
                abilityOn = true;
            }*/
        }
        else
        {
            suckParticles.Stop();
            usingAbility = false;
            if(!abilityOn)
            {
                speed = baseSpeed;
                jumpHeight = baseJumpHeight;
            }
        }
    }

    void ResetAbility()
    {
        speed = baseSpeed;
        jumpHeight = baseJumpHeight;
        abilityOn = false;
        abilities.jumpParticles.Stop();
        abilities.speedParticles.Stop();
    }
}