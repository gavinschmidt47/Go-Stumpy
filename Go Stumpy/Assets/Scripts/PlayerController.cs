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
    public GameObject losePanel;
    public GameObject gameController;
    public AudioSource playSound;
    public SpriteRenderer _spriteRenderer;

    //Private Variables
    private bool usingAbility = false;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 faceDir = Vector2.right;
    private float savedSpeed;
    private float savedJump;
    private Animator animator;
    private GameObject music;
    

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

        //Set Speed and Jump Or Abilities
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

        //Get Animator
        animator = GetComponent<Animator>();

        //Get Music
        music = GameObject.FindGameObjectWithTag("Music");
    }

    void Update()
    {
        //Read Input
        moveDirection = movement.ReadValue<Vector2>();
        if (jump.triggered && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(Vector2.up * gameInfo.currJump, ForceMode2D.Impulse);
            playSound.Play();

        }

        
        //Animation
        if (moveDirection != Vector2.zero && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        
        //Facing Direction
        if (moveDirection.x > 0)
        {
            faceDir = Vector2.right;
            suckParticles.transform.rotation = Quaternion.Euler(180, -90, 0);
            _spriteRenderer.flipX = true;
        }
        else if (moveDirection.x < 0)
        {
            faceDir = Vector2.left;
            suckParticles.transform.rotation = Quaternion.Euler(0, -90, 0);
            _spriteRenderer.flipX = false;
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
        
        //Not Invincible
        if (gameInfo.abilityOn && !gameInfo.invincible)   
        {
            if (other.gameObject.CompareTag("Jumpy") || other.gameObject.CompareTag("Speedy") || other.gameObject.CompareTag("Boss") || other.gameObject.CompareTag("Acorn"))
            {
                gameInfo.abilityOn = false;
                ResetAbility();
            }
        }
        else if (!gameInfo.invincible)
        {
            if (other.gameObject.CompareTag("Jumpy") || other.gameObject.CompareTag("Speedy") || other.gameObject.CompareTag("Boss") || other.gameObject.CompareTag("Acorn"))
            {
                Death();
            }
        }
        //Invincible
        else if (gameInfo.invincible)
        {
            if (other.gameObject.CompareTag("Jumpy") || other.gameObject.CompareTag("Speedy") || other.gameObject.CompareTag("Acorn"))
                Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("KillZone"))
        {
            {
                Death();
            }
        }
        //Stage Change
        if (other.gameObject.CompareTag("BossStageChange"))
        {
            SceneManager.LoadScene("Boss");
        }
    }

    void Death()
    {
        //Pause Time
        Time.timeScale = 0;
        
        //Set Panel
        losePanel.SetActive(true);
        gameController.GetComponent<GameController>().enabled = false;
        
        //Set Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //End Music
        Destroy(music);

        //Disable Camera
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamera != null)
        {
            mainCamera.GetComponent<CameraController>().enabled = false;
        }

        // Disable all EnemyControllers
        GameObject[] jumpyEnemies = GameObject.FindGameObjectsWithTag("Jumpy");
        GameObject[] speedyEnemies = GameObject.FindGameObjectsWithTag("Speedy");

        foreach (GameObject enemy in jumpyEnemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
            enemyController.enabled = false;
            }
        }

        foreach (GameObject enemy in speedyEnemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
            enemyController.enabled = false;
            }
        }

        //Destroy Player
        Destroy(gameObject);
    }
}