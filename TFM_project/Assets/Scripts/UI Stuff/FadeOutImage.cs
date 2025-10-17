using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ImageFadeOut : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image targetImageBG;        // Assign UI Image
    public RawImage targetImageBG2;    // Assign UI Image
    public TextMeshProUGUI targetText; // Assign UI text 
    public Canvas GameUICanvas;        // Assign In-Game UI Canvas

    [Header("Timing")]
    public float fadeDurationBG = 3f;
    public float fadeDurationBG2 = 5f;
    public float fadeDurationText = 1f;
    public float delayBeforeFade = 10f;
    public float gameUIFadeDuration = 3f;

    private Canvas parentCanvas; // Intro canvas
    private CanvasGroup gameUICanvasGroup; // Controls Game UI fade

    private void Start()
    {
        //get the parent canvas (so we can disable it later)
        parentCanvas = targetImageBG.GetComponentInParent<Canvas>();

        //ensure Game UI Canvas starts disabled
        gameUICanvasGroup = GameUICanvas.GetComponent<CanvasGroup>();
        if (gameUICanvasGroup == null)
        {
            gameUICanvasGroup = GameUICanvas.gameObject.AddComponent<CanvasGroup>();
        }

        gameUICanvasGroup.alpha = 0f;
        GameUICanvas.gameObject.SetActive(true); // Must be active for fading

        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        //ensure the image & text starts fully visible
        targetImageBG.canvasRenderer.SetAlpha(1f);
        targetImageBG2.canvasRenderer.SetAlpha(1f);
        targetText.canvasRenderer.SetAlpha(1f);

        //wait before fading
        yield return new WaitForSeconds(delayBeforeFade);

        //start fading Game UI in parallel
        StartCoroutine(FadeInGameUI());

        //fade out
        targetImageBG.CrossFadeAlpha(0f, fadeDurationBG, false);
        targetImageBG2.CrossFadeAlpha(0f, fadeDurationBG2, false);
        targetText.CrossFadeAlpha(0f, fadeDurationText, false);

        float maxFadeDuration = Mathf.Max(fadeDurationBG, fadeDurationBG2, fadeDurationText);
        yield return new WaitForSeconds(maxFadeDuration);

        //wait for fade to complete
        /*yield return new WaitForSeconds(fadeDurationBG);
        yield return new WaitForSeconds(fadeDurationBG2);
        yield return new WaitForSeconds(fadeDurationText);*/

        //disable the parent canvas after fade
        if (parentCanvas != null)
            parentCanvas.gameObject.SetActive(false);
        else
            Debug.LogWarning("No Canvas found to disable");
    }

    private IEnumerator FadeInGameUI()
    {
        float elapsed = 0f;
        while (elapsed < gameUIFadeDuration)
        {
            float t = elapsed / gameUIFadeDuration;
            gameUICanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameUICanvasGroup.alpha = 1f; // ensure fully visible
    }
}