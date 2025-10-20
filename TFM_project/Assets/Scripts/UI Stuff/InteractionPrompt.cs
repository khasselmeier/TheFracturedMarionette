using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI promptText;
    public float detectionRadius = 2f;

    private Transform player;
    private string currentPrompt = "";
    private bool isPlayerNearby = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (player == null || promptText == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        // Add a small hysteresis buffer (prevents flicker near the threshold)
        if (!isPlayerNearby && distance <= detectionRadius)
        {
            isPlayerNearby = true;
            ShowPrompt();
        }
        else if (isPlayerNearby && distance > detectionRadius + 0.3f) // buffer of 0.3
        {
            isPlayerNearby = false;
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        switch (gameObject.tag)
        {
            case "Push":
                currentPrompt = "Push";
                break;
            case "Climb":
                currentPrompt = "Climb";
                break;
            default:
                currentPrompt = "";
                break;
        }

        if (!string.IsNullOrEmpty(currentPrompt))
        {
            promptText.text = currentPrompt;
            promptText.gameObject.SetActive(true);
        }
    }

    private void HidePrompt()
    {
        promptText.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}