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
    public bool isAlive;

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
            if (L >= (R - heightDiffThreshold) && L <= (R + heightDiffThreshold)){
                if (pairType == "arm")
                    Debug.Log("Climb");
                //anim.SetTrigger("Climb");
                else
                    Debug.Log("Jump");
                //anim.SetTrigger("Jump");

                return;
            }
        }
        //
        // IF one limb higher than the other THEN trigger half-step or one-arm raise animations
        if (L < R)
        {
            if (pairType == "arm")
                Debug.Log("Left arm raise");
            //anim.SetTrigger("LeftArmRaise");
            else
                Debug.Log("left half step");
            //anim.SetTrigger("LeftHalfStep");

            return;
        }

        if (L > R)
        {
            if (pairType == "arm")
                Debug.Log("Right arm raise");
            //anim.SetTrigger("RightArmRaise");
            else
                Debug.Log("Right half step");
            //anim.SetTrigger("RightHalfStep");

            return;
        }

    }
}
