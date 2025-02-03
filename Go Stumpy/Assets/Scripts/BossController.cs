using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossController : MonoBehaviour
{
    //Public Variables
    public InputAction pauseButton;
    public GameObject stumpy;

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
        }
    }
}