using System.Collections;
using UnityEngine;

public class GyroMovementController : MonoBehaviour
{
    [Header("Limb References")]
    public GyroLimb leftLimb;
    public GameObject player;

    [Header("Animation")]
    public Animator anim;
    public float playDuration = 1f;
    public string ClipName;
    public Vector3 MoveAmt;

    [Header("Limb Settings")]
    public string pairType;
    public float heightDiffThreshold;  // Height threshold to trigger movement
    public bool isAlive;
    public float climbThreshold = 0.5f;

    private bool walking;
    private bool canWalk;
    private bool canJump;
    private bool jumping;

    void Update()
    {
            // Get the Y-axis height values from both gyros
            float leftHeight = leftLimb.heightValue;

        // Check if both limbs are raised similarly
        /////////////////////////////ARMS///////////////////////////////////////////////////////
        if(pairType == "arm")
        {
            if(leftHeight >= climbThreshold) // and colliding with climb-able area
            {
                if (canJump)
                {
                    //jumping = true;
                    StartCoroutine(PlayUpAnim());
                    canJump = false;
                }
            }
            else
            {
                canJump = true;
            }
        }
        ////////////////////////////LEGS///////////////////////////////////////////////////////
        // Left limb is higher

            if(pairType == "leg" && (leftLimb.isRaised || leftLimb.isLowered)) //not jumping
            {
                if (canWalk)
                {
                     walking = true;
                    //Debug.Log("Left half step");
                    StartCoroutine(PlayTakeStepAnim());
                    //anim.SetTrigger("TakeStep");
                    canWalk = false;
                }

            }
       
            if (!leftLimb.isLowered && !leftLimb.isRaised) //not jumping
            {
                anim.speed = 1;
                anim.SetTrigger("Idle");
                canWalk = true;
            }
        }
    IEnumerator PlayTakeStepAnim()
    {
        walking = true;
        anim.speed = 0.5f;
        anim.Play(ClipName);

        yield return new WaitForSeconds(0.5f);

        if (leftLimb.isNeutral && walking) //not jumping
        {
            Debug.Log("walk forward");
            player.transform.position += MoveAmt;
            walking = false;
        }
        yield return new WaitForSeconds(playDuration);
        anim.speed = 0;
        walking = false;
    }

    IEnumerator PlayUpAnim()
    {
        //walking = false;
        //jumping = true;
        //anim.Play("JumpAnim"); //or climb   
        Debug.Log("Jump Animation Here");
        yield return new WaitForSeconds(playDuration); //play entire duration
        //jumping = false;
    }

}
