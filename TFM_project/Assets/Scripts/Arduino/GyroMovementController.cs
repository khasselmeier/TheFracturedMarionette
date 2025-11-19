using UnityEngine;

public class GyroMovementController : MonoBehaviour
{
    [Header("Limb References")]
    public GyroLimb leftLimb;
    public GyroLimb rightLimb;

    [Header("Animation")]
    public Animator anim; //Anims allow root motion to apply physics

    [Header("Limb Settings")]
    public string pairType = "arm";  
    public float heightDiffThreshold = 0.15f;

    void Update()
    {
        if (!leftLimb || !rightLimb || !anim) return;

        float L = leftLimb.heightValue;
        float R = rightLimb.heightValue;

        bool leftUp = leftLimb.isRaised;
        bool rightUp = rightLimb.isRaised;

        // IF both limbs raised trigger jump or climb anim 
        if (leftUp && rightUp)
        {
            if (pairType == "arm")
                anim.SetTrigger("Climb");
            else
                anim.SetTrigger("Jump");

            return;
        }

        // IF one limb higher than the other THEN trigger half-step or one-arm raise animations
        if (L > R + heightDiffThreshold)
        {
            if (pairType == "arm")
                anim.SetTrigger("LeftArmRaise");
            else
                anim.SetTrigger("LeftHalfStep");

            return;
        }

        if (R > L + heightDiffThreshold)
        {
            if (pairType == "arm")
                anim.SetTrigger("RightArmRaise");
            else
                anim.SetTrigger("RightHalfStep");

            return;
        }
    }
}
