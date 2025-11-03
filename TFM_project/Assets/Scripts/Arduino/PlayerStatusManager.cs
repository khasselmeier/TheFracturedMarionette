using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int _healthpoints = 100;
    [SerializeField] private bool isDead;
    [SerializeField] private int maxSpeed;
    [SerializeField] private int maxLimbHealth;
    [SerializeField] private int limbDamageAmt;
    public bool canClimb;
    public bool canJump;

    [Header("Limb Health")] 
    [Tooltip("MPU0 = left arm\r\n MPU1 = right arm\r\n MPU2 = left leg\r\n MPU3 = right leg")]
    [SerializeField] private int[] LimbHealth;

    [Header("Limb OBJ")]
    [SerializeField] private GyroLimb LArm;
    [SerializeField] private GyroLimb RArm;
    [SerializeField] private GyroLimb LLeg;
    [SerializeField] private GyroLimb RLeg;
    public GameObject body;

    private SFXManager sfxManager;

    public void Start()
    {
        LimbHealth = new int[] { maxLimbHealth, maxLimbHealth, maxLimbHealth, maxLimbHealth };
    }    

    public void TakeHit(int limb)
    {
        Debug.Log("Player Taking hit");
        if(_healthpoints <= 0)
        {
            isDead = true;
            Die();
        }

        //take damage on limb
        //if limb is dead, detach from body

        if(LimbHealth[limb] == 20)
        {
            LimbHealth[limb] -= limbDamageAmt;
        }
        else if (LimbHealth[limb] == 10)
        {
            LimbHealth[limb] -= limbDamageAmt;
            if (limb == 0) DetachLimb(LArm, "arm");
            else if (limb == 1) DetachLimb(RArm, "arm");
            else if (limb == 2) DetachLimb(LLeg, "leg");
            else if (limb == 3) DetachLimb(RLeg, "leg");
        }
    }

    private void Die()
    {
        Debug.Log("player Died");
        //call game manager to set lose screen and reset game
        //Destroy(gameObject);
    }

    //disable limb script 
    public void DetachLimb(GyroLimb Limb, string limbType)
    {
        if(Limb.isAlive)
        {
            Limb.isAlive = false;
            Debug.Log("limb Died");
            _healthpoints -= 25; //take away from body health fourths for each limb
            Limb.GetComponent<GyroLimb>().enabled = false;


            //based on what limb was just destroyed - either change speed or disable jump or climb based on other limbs health
            if (limbType == "leg")
            {
                //decrease speed
                maxSpeed -= 5;
                if(Limb.otherLimb.isAlive == false)
                {
                    canJump = false;
                }

            }
            else if(limbType == "arm")
            {
                maxSpeed -= 5;
                if (Limb.otherLimb.isAlive == false)
                {
                    canClimb = false;
                }
            }
        }
        else
        {
            Debug.Log("missed");
        }
    }
}
