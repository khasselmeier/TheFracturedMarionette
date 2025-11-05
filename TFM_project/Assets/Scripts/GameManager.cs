using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Time Tracking")]
    public float totalPlayTime = 0f;
    private bool isCounting = false;

    void Awake()
    {
        //singleton pattern —> ensures only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //persist between scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //listen for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (isCounting)
            totalPlayTime += Time.deltaTime;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name.ToLower();

        //stop counting in win/lose scenes
        if (sceneName.Contains("winscreen") || sceneName.Contains("losescreen"))
        {
            isCounting = false;
            Debug.Log($"Game Over. Total Play Time: {totalPlayTime:F2} seconds");

            TrySubmitToLeaderboard();
        }
    }

    //called when player presses Start in the main menu
    public void StartTimeCount()
    {
        totalPlayTime = 0f;
        isCounting = true;
    }

    private void TrySubmitToLeaderboard()
    {
        if (LeaderboardManager.instance != null)
        {
            LeaderboardManager.instance.TryAddTime(totalPlayTime);
        }
        else
        {
            Debug.LogWarning("LeaderboardManager not found in scene");
        }
    }
}