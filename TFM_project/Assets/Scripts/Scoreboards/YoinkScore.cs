using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DapperDino.Scoreboards;

public class YoinkScore : MonoBehaviour
{
    public string userName;
    public float score = 100000f;
    public TMP_InputField inputField;

    public GameObject scoreboardScreen;
    public GameObject nameEntryScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreboardScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void formatTime()
    {
        float tempscore = GameManager.Instance.totalPlayTime;
        score = Mathf.Round(tempscore * 100f) / 100f;
        Debug.Log(score);
    }

    public void NameEntry()
    {
        userName = inputField.text;
    }

    public void OnSubmitName()
    {
        if (userName == null)
        {
            return;
        }
        Scoreboard.instance.testEntryName = userName;
        Scoreboard.instance.testEntryScore = score;
        Scoreboard.instance.AddTestEntry();

        scoreboardScreen.SetActive(true);
        nameEntryScreen.SetActive(false);
    }
}
