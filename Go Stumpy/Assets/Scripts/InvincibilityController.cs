using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvincibilityController : MonoBehaviour
{
    //Public Variables
    public GameInfo gameInfo;

    //Private Variables
    private Toggle invincibleToggle;

    void Start()
    {
        invincibleToggle = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        gameInfo.invincible = invincibleToggle.isOn;
    }
}
