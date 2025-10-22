using System.Collections;
using UnityEngine;

public class GyroLimb : MonoBehaviour
{
    [Header("Serial Manager")]
    [Tooltip("Label matching the sensor in SerialManager (e.g. MPU0, MPU1)")]
    public string sensorLabel = "MPU0";

    [Header("Rotation Calibration")]
    [Tooltip("Adjust this to fix physical sensor alignment differences")]
    public Quaternion rotationOffset = Quaternion.identity;
    private Quaternion calibrationOffset = Quaternion.identity;
    private bool isCalibrated = false;

    [Header("Move Param")]
    public float movementThreshold;
    public float maxSpeed = 3f;
    public bool isMoving = false;
    public GameObject body;
    public Vector3 moveAmtDir;
    public int cooldownTime;
    public bool onCooldown = false;

    [Header("Legs")]
    public float pitch;
    public GameObject leftLeg;
    public GameObject rightLeg;
    public bool CanJump;

    [Header("Player")]
    public PlayerManager player;

    private Quaternion currentQuat = Quaternion.identity;

    void Start()
    {
        if (GyroParse.Instance == null)
        {
            Debug.LogError("SerialManager instance not found! Make sure SerialManager is in the scene.");
        }
        //Calibrate();
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Calibrate();
        }

        if (GyroParse.Instance != null)
        {
            if (GyroParse.Instance.TryGetQuaternion(sensorLabel, out Quaternion rawQuat))
            {
                // Apply calibration and rotation offset
                Quaternion calibratedQuat = isCalibrated ? calibrationOffset * rawQuat : rawQuat;
                currentQuat = calibratedQuat * rotationOffset;

                // Apply rotation to the GameObject
                transform.rotation = currentQuat;

                // Calculate pitch based on quaternion for walking logic
                // Here I assume pitch calculation from quaternion components (you can adjust as needed)
                pitch = Mathf.Asin(2f * (currentQuat.w * currentQuat.x + currentQuat.y * currentQuat.z)) * Mathf.Rad2Deg;
            }
            else { Debug.Log(" Oop fetching no quat - Restart Arduino if cont error"); }
        }

            HandleWalk();
    }

    public void HandleWalk()
    {
        if (onCooldown)
        {
            StartCoroutine(Cooldown());
        }

        if (pitch >= movementThreshold && pitch <= movementThreshold + 10)
        {
            body.GetComponent<Rigidbody>().transform.position += moveAmtDir;
            onCooldown = true;
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }

    /// <summary>
    /// Sets the current sensor rotation as the zero reference.
    /// Call this to calibrate.
    /// </summary>
    public void Calibrate()
    {
        // Invert the current raw quaternion to use as calibration offset
        calibrationOffset = Quaternion.Inverse(currentQuat);
        isCalibrated = true;
        Debug.Log($"GyroLeg ({sensorLabel}) calibrated.");
    }
}
