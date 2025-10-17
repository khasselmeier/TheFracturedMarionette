using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FadeInButtons : MonoBehaviour
{
    [Header("Fade Settings")]
    public Button ButtonStart;
    public Button ButtonHTP;
    public Button ButtonQuit;

    [Header("Timing")]
    public float fadeInDuration = 1.0f;
    public float fadeInDelay = 2.0f;
    public float delayBetweenButtons = 0.2f;

    private Button[] buttons;

    private void Awake()
    {
        //gather all buttons into an array
        buttons = new Button[] { ButtonStart, ButtonHTP, ButtonQuit };

        foreach (Button btn in buttons)
        {
            //get image & TMP text
            Image btnImage = btn.GetComponent<Image>();
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();

            //start transparent
            if (btnImage != null) btnImage.canvasRenderer.SetAlpha(0f);
            if (btnText != null) btnText.canvasRenderer.SetAlpha(0f);
        }
    }

    private void Start()
    {
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        foreach (Button btn in buttons)
        {
            yield return new WaitForSeconds(delayBetweenButtons);

            //get image & TMP text
            Image btnImage = btn.GetComponent<Image>();
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();

            //fade in both
            if (btnImage != null) btnImage.CrossFadeAlpha(1f, fadeInDuration, false);
            if (btnText != null) btnText.CrossFadeAlpha(1f, fadeInDuration, false);

            //wait between buttons
            yield return new WaitForSeconds(delayBetweenButtons);
        }
    }
}