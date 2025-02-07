using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMainController : MonoBehaviour
{
    //Public Variables
    public Button playB;
    public Button optionsB;
    public Button aboutB;
    public Button exitB;
    public Button backB;
    public GameObject menuPanel;
    public GameObject aboutPanel;
    public GameObject optionsPanel;
    public GameObject back;
    public GameInfo gameInfo;
    

    void Start()
    {
        //Button Listeners
        playB.onClick.AddListener(Play);
        optionsB.onClick.AddListener(Options);
        aboutB.onClick.AddListener(About);
        exitB.onClick.AddListener(Exit);
        backB.onClick.AddListener(Back);

        //Set Panels
        menuPanel.SetActive(true);
        aboutPanel.SetActive(false);
        optionsPanel.SetActive(false);
        back.SetActive(false);

        //Set GameInfo
        gameInfo.paused = false;
        gameInfo.currSpeed = gameInfo.baseSpeed;
        gameInfo.currJump = gameInfo.baseJumpHeight;
        gameInfo.abilityOn = false;
        gameInfo.currAbility = "";

        //Set Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        //Back Button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuPanel.activeSelf)
            {
                Back();
            }
            else
            {
                Exit();
            }
        }
    }

    //Buttons
    void Play()
    {
        SceneManager.LoadScene("Main");
    }
    void Options()
    {
        //Set Panels
        menuPanel.SetActive(false);
        aboutPanel.SetActive(false);
        optionsPanel.SetActive(true);
        back.SetActive(true);
    }
    void About()
    {
        //Set Panels
        menuPanel.SetActive(false);
        aboutPanel.SetActive(true);
        optionsPanel.SetActive(false);
        back.SetActive(true);
    }
    void Exit()
    {
        Application.Quit();
    }
    void Back()
    {
        //Set Panels
        menuPanel.SetActive(true);
        aboutPanel.SetActive(false);
        optionsPanel.SetActive(false);
        back.SetActive(false);
    }
}
