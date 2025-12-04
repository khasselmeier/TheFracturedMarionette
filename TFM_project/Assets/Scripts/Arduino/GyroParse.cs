using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class GyroParse : MonoBehaviour
{
    public static GyroParse Instance;

    public string targetPort = "COM5";
    public int baudRate = 115200;

    private SerialPort arduinoPort;
    private string serialBuffer = "";

    // Store latest quaternion per sensor label
    private Dictionary<string, Quaternion> quatMap = new Dictionary<string, Quaternion>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        arduinoPort = new SerialPort(targetPort, baudRate);
        arduinoPort.ReadTimeout = 50;

        try
        {
            arduinoPort.Open();
            Debug.Log("Opened port: " + targetPort);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open port " + targetPort + ": " + e.Message);
        }
    }

    void Update()
    {
        if (arduinoPort != null && arduinoPort.IsOpen && arduinoPort.BytesToRead > 0)
        {
            try
            {
                serialBuffer += arduinoPort.ReadExisting();

                int newLineIndex;
                while ((newLineIndex = serialBuffer.IndexOf('\n')) >= 0)
                {
                    string line = serialBuffer.Substring(0, newLineIndex).Trim();
                    serialBuffer = serialBuffer.Substring(newLineIndex + 1);
                    ProcessLine(line);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Serial read error: " + e.Message);
            }
        }
    }

    void ProcessLine(string line)
    {
        if (string.IsNullOrEmpty(line)) return;

        var parts = line.Split(':');
        if (parts.Length != 2) return;

        string label = parts[0].Trim();  // e.g. "MPU0"
        string data = parts[1].Trim();   // e.g. "0.9985,0.005,0.055,0.02"

        var values = data.Split(',');
        if (values.Length != 4) return;

        if (float.TryParse(values[0], out float w) &&
            float.TryParse(values[1], out float x) &&
            float.TryParse(values[2], out float y) &&
            float.TryParse(values[3], out float z))
        {
            // Convert from MPU6050 right-handed Y-forward, Z-up to Unity left-handed Y-up, Z-forward
            quatMap[label] = new Quaternion(x, z, -y, w);
        }
    }

    // Public method for limb scripts to get quaternion by label
    public bool TryGetQuaternion(string label, out Quaternion quat)
    {
        return quatMap.TryGetValue(label, out quat);
    }

    private void OnApplicationQuit()
    {
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
            Debug.Log("Closed port: " + targetPort);
        }
    }
}
