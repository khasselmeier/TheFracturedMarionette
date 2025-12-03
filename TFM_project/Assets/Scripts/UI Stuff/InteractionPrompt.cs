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

    private Outline objectOutline;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (promptText != null)
            promptText.gameObject.SetActive(false);

        objectOutline = GetComponent<Outline>();

        DisableOutline();
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (!isPlayerNearby && distance <= detectionRadius)
        {
            isPlayerNearby = true;
            ShowPrompt();
            EnableOutline();
        }

        else if (isPlayerNearby && distance > detectionRadius)
        {
            isPlayerNearby = false;
            HidePrompt();
            DisableOutline();
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

        if (promptText != null && currentPrompt != "")
        {
            promptText.text = currentPrompt;
            promptText.gameObject.SetActive(true);
        }
    }

    private void HidePrompt()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void EnableOutline()
    {
        if (objectOutline == null) return;

        objectOutline.enabled = true;
        objectOutline.OutlineWidth = 5;   // set your normal value
        objectOutline.OutlineMode = Outline.Mode.OutlineAll;
    }

    private void DisableOutline()
    {
        if (objectOutline == null) return;

        objectOutline.OutlineWidth = 0;
        objectOutline.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

