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

    //called when the Play button is pressed
    public void PlayBookGame()
    {
        SceneManager.LoadScene("BookIntroduction");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainGameAct1");
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
        mainMenuUI.SetActive(false);
        howToPlayUI.SetActive(true);
        returnButton.SetActive(true);
    }

    //called when the Return button is pressed
    public void ReturnToMainMenuUI()
    {
        howToPlayUI.SetActive(false);
        mainMenuUI.SetActive(true);
        returnButton.SetActive(false);
    }
}
