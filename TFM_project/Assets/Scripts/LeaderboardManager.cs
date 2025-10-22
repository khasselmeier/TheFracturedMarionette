using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;

    public LeaderboardEntry(string name, int score)
    {
        playerName = name;
        this.score = score;
    }
}

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;

    [Header("Leaderboard UI")]
    public GameObject leaderboardPanel;          // Main leaderboard panel
    public GameObject nameEntryPanel;            // Panel for entering name
    public TMP_InputField nameInputField;        // Input field for typing name
    public GameObject[] leaderboardEntries;      // Each entry panel (5 total)
    public TMP_Text scoreShowcase;

    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();
    private int pendingScore = -1;
    private const int MaxEntries = 5;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        scoreShowcase.text = "Time: 00.00";
    }

    // Call this when the game ends
    public void TryAddScore(int score)
    {
        if (leaderboard.Count < MaxEntries || score > leaderboard[leaderboard.Count - 1].score)
        {
            pendingScore = score;
            ShowNameInput();
        }
        else
        {
            ShowLeaderboard();
        }
    }

    void ShowNameInput()
    {
        leaderboardPanel.SetActive(false);
        nameEntryPanel.SetActive(true);
        nameInputField.text = "";
    }

    public void OnSubmitName()
    {
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
            playerName = "Player";

        AddEntry(new LeaderboardEntry(playerName, pendingScore));
        pendingScore = -1;

        nameEntryPanel.SetActive(false);
        ShowLeaderboard();
    }

    void AddEntry(LeaderboardEntry newEntry)
    {
        leaderboard.Add(newEntry);
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        if (leaderboard.Count > MaxEntries)
            leaderboard.RemoveAt(leaderboard.Count - 1);

        UpdateLeaderboardUI();
    }

    void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        UpdateLeaderboardUI();
    }

    void UpdateLeaderboardUI()
    {
        for (int i = 0; i < leaderboardEntries.Length; i++)
        {
            if (i < leaderboard.Count)
            {
                // Find the two text objects under each panel
                TextMeshProUGUI[] texts = leaderboardEntries[i].GetComponentsInChildren<TextMeshProUGUI>();

                if (texts.Length >= 2)
                {
                    texts[0].text = leaderboard[i].playerName; // name text
                    texts[1].text = leaderboard[i].score.ToString(); // score text
                }
            }
            else
            {
                // Clear unused panels
                TextMeshProUGUI[] texts = leaderboardEntries[i].GetComponentsInChildren<TextMeshProUGUI>();
                if (texts.Length >= 2)
                {
                    texts[0].text = "---";
                    texts[1].text = "-";
                }
            }
        }
    }
}
