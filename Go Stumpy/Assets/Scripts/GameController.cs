using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    //Public Variables
    public InputAction pauseButton;
    public GameObject stumpy;
    public GameInfo gameInfo;
    public AbilitiesScript abilities;
    public float pauseTimeScale = 0.1f;
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public GameObject warningPanel;
    public Button resumeButton;
    public Button optionsButton;
    public Button backButton;
    public Button goBackButton;
    public Button ContinueButton;
    public ParticleSystem jumpParticles;
    public ParticleSystem speedParticles;
    public GameObject invincibleText;
    public GameObject jumpyText;
    public GameObject speedyText;

    //Private Variables
    private bool firstOpt = true;
    void OnEnable()
    {
        pauseButton.Enable();
    }

    void OnDisable()
    {
        pauseButton.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        stumpy.GetComponent<PlayerController>().enabled = true;

        //Set Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Set Panel
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        warningPanel.SetActive(false);

        gameInfo.paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Pause
        if (pauseButton.triggered)
        {
            stumpy.GetComponent<PlayerController>().enabled = !(stumpy.GetComponent<PlayerController>().enabled);
            if (!gameInfo.paused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }

        //Set Text
        if (gameInfo.invincible)
        {
            invincibleText.SetActive(true);
        }
        else
        {
            invincibleText.SetActive(false);
        }
        if (gameInfo.currAbility == "Jumpy")
        {
            jumpyText.SetActive(true);
            speedyText.SetActive(false);
        }
        else if (gameInfo.currAbility == "Speedy")
        {
            jumpyText.SetActive(false);
            speedyText.SetActive(true);
        }
        else
        {
            jumpyText.SetActive(false);
            speedyText.SetActive(false);
        }
    }

    private void Pause()
    {
        Time.timeScale = pauseTimeScale;
        gameInfo.paused = true;

        //Enable Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Set Panel
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);

        //Disable Player
        stumpy.GetComponent<PlayerController>().enabled = false;

        //Resume Button
        resumeButton.onClick.AddListener(Unpause);
        if (firstOpt)
            optionsButton.onClick.AddListener(Warning);
        else 
            optionsButton.onClick.AddListener(Options);
    }

    private void Unpause()
    {
        Time.timeScale = 1f;
        gameInfo.paused = false;

        //Diable Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Set Panel
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);

        //Resume Button
        resumeButton.onClick.RemoveListener(Unpause);
        optionsButton.onClick.RemoveListener(Options);

        //Enable Player
        stumpy.GetComponent<PlayerController>().enabled = true;


        //New Abilities
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

    private void Warning()
    {
        //set Panel
        warningPanel.SetActive(true);

        //Set Buttons
        goBackButton.onClick.AddListener(GoBack);
        ContinueButton.onClick.AddListener(Options);
    }

    private void Options()
    {
        //set Panel
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
        warningPanel.SetActive(false);

        //Reset Cheats
        if (firstOpt)
        {
            gameInfo.invincible = false;
            gameInfo.setCurrAbility(0);
            firstOpt = false;
        }

        //Set Buttons
        backButton.onClick.AddListener(Back);
        goBackButton.onClick.RemoveListener(GoBack);
        ContinueButton.onClick.RemoveListener(Options);
    }

    private void Back()
    {
        //set Panel
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        warningPanel.SetActive(false);

        //Set Buttons
        backButton.onClick.RemoveListener(Back);
    }
    private void GoBack()
    {
        //set Panel
        warningPanel.SetActive(false);

        //Set Buttons
        goBackButton.onClick.RemoveListener(GoBack);
    }
}
