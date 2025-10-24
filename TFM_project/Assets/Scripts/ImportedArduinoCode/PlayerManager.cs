/*
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int _healthpoints = 100;
    [SerializeField] private bool isDead;
    [SerializeField] private int maxSpeed;
    [SerializeField] private bool canClimb;
    [SerializeField] private int maxLimbHealth;

    [Header("Limb Health")]
    [SerializeField] private int[] LimbHealth;
    // 0-3
    //0 = left arm
    //1 = right arm
    //2 = left leg
    //3 = right leg
    //check when taking damage what limb the enemy collides with else take damage on random limb, set the gyro component inactive and then detach limb from parent in hierarchy


    [Header("Limb OBJ")]
    [SerializeField] private GyroArm LArm;
    [SerializeField] private GyroArm RArm;
    [SerializeField] private GyroLeg LLeg;
    [SerializeField] private GyroLeg RLeg;
    public GameObject body;
    //bools checking limbs alive


    public void Start()
    {
        LimbHealth = new int[] { maxLimbHealth, maxLimbHealth, maxLimbHealth, maxLimbHealth };
    ;
    }    

    public void TakeHit(int limb, int damageAmt)
    {
        Debug.Log("PlayerTaking hit");
        if(_healthpoints <= 0)
        {
            isDead = true;
            Die();
        }

        //if limb is dead, detach from body

        if(LimbHealth[limb] < 0)
        {
            LimbHealth[limb] -= damageAmt;
        }
        else if(LimbHealth[limb] <= 0)
        {
            _healthpoints -= 25;
            if (limb == 0) DetachLimb(LArm.gameObject, "arm");
            else if (limb == 1) DetachLimb(RArm.gameObject, "arm");
            else if (limb == 2) DetachLimb(LLeg.gameObject, "leg");
            else if (limb == 3) DetachLimb(RLeg.gameObject, "leg");
        }
    }

    private void Die()
    {
        //animation or ragdoll?
        Debug.Log("player Died");
        //if all limbs die then player dies
        //Destroy(gameObject);
    }

    public void DetachLimb(GameObject Limb, string limbType)
    {
        //destory limb obj and instantiate ridgid body asset and add force to fling off
        Debug.Log("limb Died");
        Limb.SetActive(false);

        //based on what limb was just destroyed - either change speed and remove jump or remove can climb
        if (limbType == "leg")
        {
            //decrease speed
            //if arm remove can climb forever
        }
    }
}
*/