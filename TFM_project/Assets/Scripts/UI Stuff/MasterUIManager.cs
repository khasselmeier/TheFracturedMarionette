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
    public GameObject optionsUI;

    //called when the Play button is pressed
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGameAct1");
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
        Debug.Log("Show How to Play UI");

        mainMenuUI.SetActive(false);
        howToPlayUI.SetActive(true);
        returnButton.SetActive(true);
    }

    //called when the Options button is pressed
    public void ShowOptions()
    {
        Debug.Log("Show Options UI");

        mainMenuUI.SetActive(false);
        optionsUI.SetActive(true);
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
}
