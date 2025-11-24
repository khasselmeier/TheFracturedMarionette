using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform endPoint;
    public Slider progressBar;

    private float totalDistance;

    void Start()
    {
        if (player == null || endPoint == null || progressBar == null)
        {
            Debug.LogError("DistanceProgressBar: Assign all references in the Inspector.");
            enabled = false;
            return;
        }

        // Store the original full distance at the start
        totalDistance = Vector3.Distance(player.position, endPoint.position);
    }

    void Update()
    {
        float currentDistance = Vector3.Distance(player.position, endPoint.position);

        // Calculate progress as a percentage (1 = completed)
        float progress = 1f - (currentDistance / totalDistance);

        // Clamp to avoid overshoot
        progress = Mathf.Clamp01(progress);

        progressBar.value = progress;
    }
}
