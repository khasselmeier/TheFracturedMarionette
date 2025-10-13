using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MasterUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuUI;
    public GameObject howToPlayUI;
    public GameObject returnButton;

    [Header("Image Fade Settings")]
    public Image fadeImage;
    public float fadeImageDuration = 2.0f;

    //called when the Play button is pressed
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    //called when the Restart button is pressed
    public void RestartToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //called when the Quit button is pressed
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    //called when the How To Play button is pressed
    public void ShowHowToPlay()
    {
        Debug.Log("Show How to Play");

        mainMenuUI.SetActive(false);
        howToPlayUI.SetActive(true);
        returnButton.SetActive(true);
    }

    //called when the Return button is pressed
    public void ReturnToMainMenuUI()
    {
        Debug.Log("Hide How to Play");

        howToPlayUI.SetActive(false);
        mainMenuUI.SetActive(true);
        returnButton.SetActive(false);
    }

    public void FadeInImage()
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeInImageRoutine());
        }
    }

    private IEnumerator FadeInImageRoutine()
    {
        fadeImage.canvasRenderer.SetAlpha(0f);
        fadeImage.gameObject.SetActive(true);
        fadeImage.CrossFadeAlpha(1f, fadeImageDuration, false);
        yield return new WaitForSeconds(fadeImageDuration);
    }
}
