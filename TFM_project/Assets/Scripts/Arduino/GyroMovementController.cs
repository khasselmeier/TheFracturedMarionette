using UnityEngine;

public class GyroMovementController : MonoBehaviour
{
    [Header("Limb References")]
    public GyroLimb leftLimb;


    [Header("Animation")]
    public Animator anim;

    [Header("Limb Settings")]
    public string pairType;
    public float heightDiffThreshold;  // Height threshold to trigger movement
    public bool isAlive;
    public float climbThreshold = 0.5f;


    void Update()
    {

        // Get the Y-axis height values from both gyros
        float leftHeight = leftLimb.heightValue;

        // Check if both limbs are raised similarly
        /////////////////////////////ARMS///////////////////////////////////////////////////////
        if(pairType == "arm")
        {
            if(leftHeight >= climbThreshold)
            {
                Debug.Log("trigger climb");
            }
        }
        ////////////////////////////LEGS///////////////////////////////////////////////////////
        // Left limb is higher

            if(pairType == "leg" && leftLimb.isRaised)
            {
                Debug.Log("Left half step");
                // anim.SetTrigger("LeftHalfStep");
            }
       
        // Right limb is higher (because left limb is lower)

            if (pairType == "leg" && leftLimb.isLowered)
            {
                Debug.Log("Right half step");
                // anim.SetTrigger("RightHalfStep");
            }
    }
}
