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
    [SerializeField] private GyroMovementController LArm;
    [SerializeField] private GyroMovementController RArm;
    [SerializeField] private GyroMovementController LLeg;
    [SerializeField] private GyroMovementController RLeg;
    public GameObject body;

    [Header("UI and Audio")]
    private SFXManager sfxManager;

    [SerializeField] private LimbHealthUI[] UIlimbIndicators;
    [Tooltip("MPU0 = left arm\r\n MPU1 = right arm\r\n MPU2 = left leg\r\n MPU3 = right leg")]

    public void Start()
    {
        LimbHealth = new int[] { maxLimbHealth, maxLimbHealth, maxLimbHealth, maxLimbHealth };
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeHit(Random.Range(0, 4));
        }
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
            UIlimbIndicators[limb].ChangeUI();
        }
        else if (LimbHealth[limb] == 10)
        {
            UIlimbIndicators[limb].ChangeUI();
            LimbHealth[limb] -= limbDamageAmt;
            if (limb == 0) DetachLimb(LArm);
            else if (limb == 1) DetachLimb(RArm);
            else if (limb == 2) DetachLimb(LLeg);
            else if (limb == 3) DetachLimb(RLeg);
        }
    }



    private void Die()
    {
        Debug.Log("player Died");
        //call game manager to set lose screen and reset game
        //Destroy(gameObject);
    }

    //disable limb script 
    public void DetachLimb(GyroMovementController Limb)
    {
        if(Limb.isAlive)
        {
            Limb.isAlive = false;
            Debug.Log("limb Died");
            _healthpoints -= 25; //take away from body health fourths for each limb
            Limb.GetComponent<GyroLimb>().enabled = false;

            /*
            //based on what limb was just destroyed - either change speed or disable jump or climb based on other limbs health
            if (Limb.pairType == "leg")
            {
                //decrease speed
                //maxSpeed -= 5;
                if(Limb.otherLimb.isAlive == false)
                {
                    canJump = false;
                }

            }
            else if(Limb.pairType == "arm")
            {
                //maxSpeed -= 5;
                if (Limb.otherLimb.isAlive == false)
                {
                    canClimb = false;
                }
            }
            */
        }
        else
        {
            Debug.Log("missed");
        }
    }
}
