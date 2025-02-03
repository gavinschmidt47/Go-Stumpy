using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Public Variables
    public float abilityDivisor = 2f;
    public InputAction movement;
    public InputAction jump;
    public InputAction useAbility;
    public AbilitiesScript abilities;
    public GameInfo gameInfo;
    public ParticleSystem suckParticles;

    //Private Variables
    private bool usingAbility = false;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
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

        if (gameInfo.abilityOn)
        {
            gameInfo.currJump = gameInfo.baseJumpHeight;
            gameInfo.currSpeed = gameInfo.baseSpeed;
        }
    }

    void Update()
    {
        //Read Input
        moveDirection = movement.ReadValue<Vector2>();
        if (jump.triggered && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(Vector2.up * gameInfo.currJump, ForceMode2D.Impulse);
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
        rb.velocity = new Vector2(moveDirection.x * gameInfo.currSpeed, rb.velocity.y);

        //Use Ability
        if (useAbility.IsPressed())
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, faceDir, 10f);

            //First Ability Frame
            if (!usingAbility)
            {   
                usingAbility = true;
                suckParticles.Play();
                gameInfo.currSpeed /= abilityDivisor;
                gameInfo.currJump /= abilityDivisor;
            }
            
            //Activate Ability
            if (hit.collider != null && hit.collider.CompareTag("Jumpy"))
            {
                if(gameInfo.abilityOn)
                    ResetAbility();
                gameInfo.currJump = abilities.jumpBoost + gameInfo.baseJumpHeight;
                abilities.jumpParticles.Play();
                gameInfo.abilityOn = true;
                gameInfo.currAbility = "Jumpy";
            }
            else if (hit.collider != null && hit.collider.CompareTag("Speedy"))
            {
                if(gameInfo.abilityOn)
                    ResetAbility();
                gameInfo.currSpeed = abilities.speedBoost + gameInfo.baseSpeed;
                abilities.speedParticles.Play();
                gameInfo.abilityOn = true;
                gameInfo.currAbility = "Speedy";
            }
        }
        else
        {
            //first Non Ability Frame
            if (usingAbility)
            {
                suckParticles.Stop();
                usingAbility = false;
                if(!gameInfo.abilityOn)
                {
                    gameInfo.currSpeed = gameInfo.baseSpeed;
                    gameInfo.currJump = gameInfo.baseJumpHeight;
                }
            }
        }
    }

    void ResetAbility()
    {
        gameInfo.currSpeed = gameInfo.baseSpeed;
        gameInfo.currJump = gameInfo.baseJumpHeight;
        gameInfo.abilityOn = false;
        abilities.jumpParticles.Stop();
        abilities.speedParticles.Stop();
    }

    //Collision
    void OnCollisionEnter2D(Collision2D other)
    {
        if (gameInfo.abilityOn && !gameInfo.invincible)   
        {
            if (other.gameObject.CompareTag("Jumpy") || other.gameObject.CompareTag("Speedy"))
            {
                gameInfo.abilityOn = false;
                ResetAbility();
            }
        }
    }
}