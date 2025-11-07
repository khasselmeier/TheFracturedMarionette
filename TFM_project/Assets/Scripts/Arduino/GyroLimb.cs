using JetBrains.Rider.Unity.Editor;
using System.Collections;
using UnityEngine;

public class GyroLimb : MonoBehaviour
{
    [Header("Serial Manager")]
    [Tooltip("Matches label on Multiplexor")]
    public string sensorLabel = "MPU0";

    [Header("Rotation Calibration")]
    [Tooltip("Fix physical sensor alignment differences - C key to callibrate")]
    private Quaternion rotationOffset = Quaternion.identity;
    private Quaternion calibrationOffset = Quaternion.identity;
    private bool isCalibrated = false;
    public float x;
    public float y;
    public float z;

    [Header("Move Param")]
    public float movementThreshold;
    public float maxSpeed = 3f;
    public bool isMoving = false;
    [SerializeField] private Vector3 moveAmtDir;
    [SerializeField] private int cooldownTime;
    [SerializeField] private bool onCooldown = false;

    [Header("Limb Movement Info")]
    [Tooltip("Pitch affects degree range needed to move player")]
    [SerializeField] private float pitch;
    [SerializeField] private string limbType;

    public bool isUp;
    
    [Header("Limb Health Info")]
    public bool isAlive;

    [Header("References")]
    public PlayerStatusManager player;
    public GyroLimb otherLimb; 
    public GameObject body;
    [SerializeField] private Rigidbody rb;

    private Quaternion currentQuat = Quaternion.identity;

    void Start()
    {
        if (GyroParse.Instance == null)
        {
            Debug.LogError("SerialManager instance not found! Make sure SerialManager is in the scene.");
        }
        rotationOffset = Quaternion.Euler(x, y, z); //set on type of limb based on how we orient the gyro on the doll
        //Calibrate();
    }
    void LateUpdate()
    {
        if (GyroParse.Instance == null) return;

        if (GyroParse.Instance.TryGetQuaternion(sensorLabel, out Quaternion rawQuat))
        {
            // Convert and store
            Quaternion calibratedQuat;

            if (!isCalibrated)
            {
                // First valid reading: set this as zero reference
                calibrationOffset = Quaternion.Inverse(rawQuat);
                isCalibrated = true;
                Debug.Log($"{sensorLabel} auto-calibrated at startup");
            }

            // Apply calibration + rotation offset
            calibratedQuat = calibrationOffset * rawQuat;
            currentQuat = calibratedQuat * rotationOffset;

            // Apply rotation to this limb (local so it’s relative to the body)
            transform.localRotation = currentQuat;
        }
        if (isAlive)
        {
            HandleMoveUp();
            HandleWalk();
        }
    }


    /*
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (GyroParse.Instance.TryGetQuaternion(sensorLabel, out currentQuat)){

                Calibrate();
            }else{
                Debug.LogWarning("No data yet—move sensor to initialize before calibrating.");
            }
        }

        if (GyroParse.Instance != null)
        {
            if (GyroParse.Instance.TryGetQuaternion(sensorLabel, out Quaternion rawQuat))
            {
                // Apply calibration and rotation offset
                Quaternion calibratedQuat = isCalibrated ? calibrationOffset * rawQuat : rawQuat;
                currentQuat = calibratedQuat * rotationOffset;

                // Apply rotation to the GameObject
                transform.rotation.x = currentQuat.x;

                // Calculate pitch based on quaternion for degree of motion needed to move player
                pitch = Mathf.Asin(2f * (currentQuat.w * currentQuat.x + currentQuat.y * currentQuat.z)) * Mathf.Rad2Deg;
            }
            else { Debug.Log(" Oop fetching no quat - Restart Arduino if cont error"); }
        }
        if (isAlive)
        {
            HandleMoveUp();
            HandleWalk();
        }
    }
    */
    public void HandleWalk()
    {
        if (onCooldown)
        {
            StartCoroutine(Cooldown());
        }

        if (pitch >= movementThreshold && pitch <= movementThreshold + 10)
        {
            isUp = true;
            //rb.transform.position += moveAmtDir;
            rb.MovePosition(rb.position + Vector3.forward * moveAmtDir.magnitude);
            onCooldown = true;
        }
        else {  isUp = false; }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }
    //if the other matching limb is still alive & both are in range then go up 
    public void HandleMoveUp()
    {
        if (onCooldown)
        {
            StartCoroutine(Cooldown());
        }

        if (otherLimb.isAlive && otherLimb.isUp)
        {
            if(limbType == "arm" && player.canClimb || limbType == "leg" && player.canJump)
            {
                rb.MovePosition(rb.position + Vector3.up * moveAmtDir.magnitude);
                onCooldown = true;
            }
        }
    }

    // Sets the current sensor rotation as the zero reference.
    public void Calibrate()
    {
        // Invert the current raw quaternion to use as calibration offset
        calibrationOffset = Quaternion.Inverse(currentQuat);
        isCalibrated = true;
        Debug.Log($"GyroLimb ({sensorLabel}) calibrated.");
    }
}
