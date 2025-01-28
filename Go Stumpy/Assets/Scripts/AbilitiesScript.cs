using UnityEngine;

[CreateAssetMenu(fileName = "AbilitiesScript", menuName = "AbilitiesScript", order = 0)]
public class AbilitiesScript : ScriptableObject {
    //Jumpy ability
    public float jumpBoost = 5f;
    public ParticleSystem jumpParticles;


    //Speedy ability
    public float speedBoost = 5f;
    public ParticleSystem speedParticles;
}