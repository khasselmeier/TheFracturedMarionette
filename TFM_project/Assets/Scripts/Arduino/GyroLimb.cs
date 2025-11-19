using UnityEngine;

public class GyroLimb : MonoBehaviour
{
    [Header("Serial Manager")]
    public string sensorLabel = "MPU0";

    [Header("Calibration")]
    private Quaternion rotationOffset = Quaternion.identity;
    private Quaternion calibrationOffset = Quaternion.identity;
    private bool isCalibrated = false;

    [Header("Bar Raising Detection")]
    public float raiseThreshold = 0.60f;
    public float heightValue;
    public bool isRaised;

    private Quaternion currentQuat = Quaternion.identity;

    void Start()
    {
        rotationOffset = Quaternion.identity;
    }

    void LateUpdate()
    {
        if (!GyroParse.Instance) return;

        if (GyroParse.Instance.TryGetQuaternion(sensorLabel, out Quaternion rawQuat))
        {
            if (!isCalibrated)
            {
                calibrationOffset = Quaternion.Inverse(rawQuat);
                isCalibrated = true;
                Debug.Log($"{sensorLabel} calibrated");
            }

            Quaternion calibrated = calibrationOffset * rawQuat;
            currentQuat = calibrated * rotationOffset;

            // compute height
            Vector3 upDir = currentQuat * Vector3.up;

            heightValue = upDir.y;
            isRaised = heightValue > raiseThreshold;
        }
    }
}
