using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreenUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI totalTimeText;
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        if (totalTimeText == null)
        {
            Debug.LogWarning("TotalTimeText not assigned");
            return;
        }

        //get total time from GameManager
        float totalTime = GameManager.Instance != null ? GameManager.Instance.totalPlayTime : 0f;

        //determine scene type
        string sceneName = SceneManager.GetActiveScene().name.ToLower();

        if (sceneName.Contains("winscreen"))
        {
            totalTimeText.text = $"Total Time: {FormatTime(totalTime)}";
        }
        else if (sceneName.Contains("losescreen"))
        {
            totalTimeText.text = $"Total Time Survived: {FormatTime(totalTime)}";
        }
        else
        {
            totalTimeText.text = "";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    //format time as mm:ss
    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{minutes:00}:{secs:00}";
    }
}