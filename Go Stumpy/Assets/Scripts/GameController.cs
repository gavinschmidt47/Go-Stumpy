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
    public float pauseTimeScale = 0.1f;
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public Button resumeButton;
    public Button optionsButton;
    public Button backButton;
    public Toggle invincibleToggle;
    public TMP_Dropdown abilityDropdown;

    //Private Variables

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

        gameInfo.paused = false;
    }

    // Update is called once per frame
    void Update()
    {
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

        //Resume Button
        resumeButton.onClick.AddListener(Unpause);
        optionsButton.onClick.AddListener(Options);
        if (backButton.onClick.GetPersistentEventCount() > 0)
        {
            backButton.onClick.RemoveAllListeners();
        }
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
    }

    private void Options()
    {
        //set Panel
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);

        //Set Options
        invincibleToggle.isOn = gameInfo.invincible;
        abilityDropdown.value = gameInfo.currAbility == "Speedy" ? 1 : gameInfo.currAbility == "Jumpy" ? 2 : 0;

        //Back Button
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(Pause);
    }
}
