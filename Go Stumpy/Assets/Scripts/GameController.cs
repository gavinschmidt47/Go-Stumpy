using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    //Public Variables
    public InputAction pauseButton;
    public GameObject stumpy;

    //Private Variables
    private bool paused = false;
    private bool playerCBool;

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
        playerCBool = stumpy.GetComponent<PlayerController>().enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseButton.triggered)
        {

            stumpy.GetComponent<PlayerController>().enabled = !(stumpy.GetComponent<PlayerController>().enabled);
        }
    }
}
