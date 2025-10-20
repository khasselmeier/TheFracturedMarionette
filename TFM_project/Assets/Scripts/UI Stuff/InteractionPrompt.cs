using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI promptText;     // Assign in Inspector
    public float detectionRadius = 2f;     // Distance for showing prompt

    private Transform player;
    private string currentPrompt = "";

    private void Start()
    {
        // Find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Hide text at start
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (player == null || promptText == null)
            return;

        // Measure distance between player and this object
        float distance = Vector3.Distance(player.position, transform.position);

        // If within detection range
        if (distance <= detectionRadius)
        {
            ShowPrompt();
        }
        else
        {
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if (!promptText.gameObject.activeSelf)
            promptText.gameObject.SetActive(true);

        // Set prompt based on object tag
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

        promptText.text = currentPrompt;
    }

    private void HidePrompt()
    {
        if (promptText.gameObject.activeSelf)
            promptText.gameObject.SetActive(false);
    }

    //visualize detection radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}