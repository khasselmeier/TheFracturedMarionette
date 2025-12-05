using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreboardButtonLoader : MonoBehaviour
{
    public void LoadScoreboard()
    {
        SceneManager.LoadScene("Scoreboard");
    }
}