using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public ParticleSystem jumpParticles;
    public ParticleSystem speedParticles;

    //Private Variables
    private bool usingAbility = false;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 faceDir = Vector2.right;
    private float savedSpeed;
    private float savedJump;
    

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

        // Freeze rotation
        rb.freezeRotation = true;

        //Disable Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (!gameInfo.abilityOn)
        {
            gameInfo.currJump = gameInfo.baseJumpHeight;
            gameInfo.currSpeed = gameInfo.baseSpeed;
        }
        else
        {
            if (gameInfo.currAbility == "Jumpy")
            {
                gameInfo.currJump = abilities.jumpBoost + gameInfo.baseJumpHeight;
                gameInfo.currSpeed = gameInfo.baseSpeed;
                jumpParticles.Play();
            }
            else if (gameInfo.currAbility == "Speedy")
            {
                gameInfo.currJump = gameInfo.baseJumpHeight;
                gameInfo.currSpeed = abilities.speedBoost + gameInfo.baseSpeed;
                speedParticles.Play();
            }
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
        rb.AddForce(new Vector2(moveDirection.x * gameInfo.currSpeed, 0));

        //Use Ability
        if (useAbility.IsPressed())
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, faceDir, 10f);

            //First Ability Frame
            if (!usingAbility)
            {   
                usingAbility = true;
                suckParticles.Play();
                savedSpeed = gameInfo.currSpeed;
                savedJump = gameInfo.currJump;
                gameInfo.currSpeed /= abilityDivisor;
                gameInfo.currJump /= abilityDivisor;
            }
            
            //Activate Ability
            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
                if (hit.collider.CompareTag("Jumpy"))
                {
                    if(gameInfo.abilityOn)
                        ResetAbility();
                    gameInfo.currJump = abilities.jumpBoost + gameInfo.baseJumpHeight;
                    savedJump = gameInfo.currJump;
                    jumpParticles.Play();
                    gameInfo.abilityOn = true;
                    gameInfo.currAbility = "Jumpy";
                    suckParticles.Stop();
                    usingAbility = false;
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider != null && hit.collider.CompareTag("Speedy"))
                {
                    if(gameInfo.abilityOn)
                        ResetAbility();
                    gameInfo.currSpeed = abilities.speedBoost + gameInfo.baseSpeed;
                    savedSpeed = gameInfo.currSpeed;
                    speedParticles.Play();
                    gameInfo.abilityOn = true;
                    gameInfo.currAbility = "Speedy";
                    suckParticles.Stop();
                    usingAbility = false;
                    Destroy(hit.collider.gameObject);
                }
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
                else if (gameInfo.currAbility == "Jumpy")
                {
                    gameInfo.currJump = savedJump;
                    gameInfo.currSpeed = gameInfo.baseSpeed;
                }
                else if (gameInfo.currAbility == "Speedy")
                {
                    gameInfo.currJump = gameInfo.baseJumpHeight;
                    gameInfo.currSpeed = savedSpeed;
                }
            }
        }
    }

    void ResetAbility()
    {
        gameInfo.currSpeed = gameInfo.baseSpeed;
        gameInfo.currJump = gameInfo.baseJumpHeight;
        gameInfo.abilityOn = false;
        jumpParticles.Stop();
        speedParticles.Stop();
    }

    //Collision
    void OnCollisionEnter2D(Collision2D other)
    {
        
        // Determine which side was hit
        if (gameInfo.abilityOn && !gameInfo.invincible)   
        {
            if (other.gameObject.CompareTag("Jumpy") || other.gameObject.CompareTag("Speedy"))
            {
                gameInfo.abilityOn = false;
                ResetAbility();
            }
        }
        else if (!gameInfo.invincible)
        {
            if (other.gameObject.CompareTag("Jumpy") || other.gameObject.CompareTag("Speedy"))
                SceneManager.LoadScene("MenuMain");
        }
        else if (gameInfo.invincible)
        {
            if (other.gameObject.CompareTag("Jumpy") || other.gameObject.CompareTag("Speedy"))
                Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("KillZone"))
        {
            SceneManager.LoadScene("MenuMain");
        }
    }
}