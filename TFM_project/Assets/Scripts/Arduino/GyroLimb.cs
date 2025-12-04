using UnityEngine;

public class GyroLimb : MonoBehaviour
{
    public string label;  // Identifier for the gyro sensor
    public float heightValue;  // Representing the height or vertical displacement
    public float Hthreshold;
    public float Lthreshold;
    public bool isRaised;  // above HighLvl threshold
    public bool isNeutral;    public bool isLowered; //below LowLvl threshold


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Hthreshold = heightValue + 0.1f;
            Lthreshold = heightValue - 0.1f;
        }
        // Assuming you have access to the gyro data via the GyroParse
        if (GyroParse.Instance.TryGetQuaternion(label, out Quaternion quat))
        {
            // Get the height component (you could use the Y component of the quaternion)
            heightValue = quat.y;  // or adjust this based on your sensor data's axis alignment

            // Determine if the limb is "raised"
            isRaised = heightValue >= Hthreshold;  // Example threshold for being "raised"
            isLowered = heightValue <= Lthreshold;

        }
    }

}
