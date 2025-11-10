using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DisplayObstacleText : MonoBehaviour
{
    [Header("Text Settings")]
    [Tooltip("The message to display when triggered.")]
    public string message = "Text1";

    [Tooltip("How long the text should stay visible.")]
    public float displayDuration = 5f;

    [Tooltip("Time between each display")]
    public float interval = 10f;

    [Header("UI References")]
    public TextMeshProUGUI tmpText;

    private bool canDisplay = true;

    private void Start()
    {
        //hide text at the start
        if (tmpText != null) tmpText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canDisplay)
        {
            StartCoroutine(DisplayTextRoutine());
        }
    }

    private IEnumerator DisplayTextRoutine()
    {
        canDisplay = false;

        //show message
        if (tmpText != null)
            tmpText.text = message;

        //wait for display time
        yield return new WaitForSeconds(displayDuration);

        //hide text
        if (tmpText != null)
            tmpText.text = "";

        //wait for the cooldown interval before allowing next display
        yield return new WaitForSeconds(interval);
        canDisplay = true;
    }
}