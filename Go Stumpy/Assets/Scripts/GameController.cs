using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    //Public Variables
    public InputAction pauseButton;
    public GameObject stumpy;
    public GameInfo gameInfo;
    public float pauseTimeScale = 0.1f;

    //Private Variables

    void onEnable()
    {
        pauseButton.Enable();
    }

    void onDisable()
    {
        pauseButton.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        stumpy.GetComponent<PlayerController>().enabled = true;
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
    }

    private void Unpause()
    {
        Time.timeScale = 1f;
        gameInfo.paused = false;

        //Diable Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
