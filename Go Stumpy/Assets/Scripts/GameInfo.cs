using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "GameInfo", order = 0)]
public class GameInfo : ScriptableObject {
    //Stumpy Variables
    public float baseSpeed = 5f;
    public float baseJumpHeight = 5f;
    public float currSpeed;
    public float currJump;
    public bool abilityOn = false;
    public string currAbility;
    public bool invincible = false;

    //Jumpy Variables
    public float jumpyH = 10f;
    public float jumpyS = 10f;

    //Speedy Variables
    public float speedyS = 10f;

    //Game Variables
    public bool paused = false;
}