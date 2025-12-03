using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public float time; // total play time in seconds

    public LeaderboardEntry(string name, float time)
    {
        playerName = name;
        this.time = time;
    }
}

public class LeaderboardManager : MonoBehaviourPunCallbacks
{
    public static LeaderboardManager instance;

    [Header("Leaderboard UI")]
    public GameObject leaderboardPanel;      // Main leaderboard panel
    public GameObject nameEntryPanel;        // Panel for entering name
    public TMP_InputField nameInputField;    // Input field for typing name
    public GameObject[] leaderboardEntries;  // Each entry panel (5 total)
    public TMP_Text scoreShowcase;           // Displayed after game ends

    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();
    private float pendingTime = -1f;
    private const int MaxEntries = 5;

    private const string LEADERBOARD_KEY = "LeaderboardData"; // Photon property key

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
            return;
        }

        scoreShowcase.text = "Time: 00:00";
    }

    //called from GameManager when the player wins/loses
    public void TryAddTime(float time)
    {
        if (leaderboard.Count < MaxEntries || time > leaderboard[leaderboard.Count - 1].time)
        {
            pendingTime = time;
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

        AddEntry(new LeaderboardEntry(playerName, pendingTime));
        pendingTime = -1f;

        nameEntryPanel.SetActive(false);
        ShowLeaderboard();
    }

    public void AddExternalScore(string playerName, float score)
    {
        AddEntry(new LeaderboardEntry(playerName, score));
        ShowLeaderboard();
    }

    void AddEntry(LeaderboardEntry newEntry)
    {
        leaderboard.Add(newEntry);
        leaderboard.Sort((a, b) => b.time.CompareTo(a.time)); // Sort descending (best time = longest play)

        if (leaderboard.Count > MaxEntries)
            leaderboard.RemoveAt(leaderboard.Count - 1);

        UpdateLeaderboardUI();
        SaveLeaderboardToPhoton();
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
            TextMeshProUGUI[] texts = leaderboardEntries[i].GetComponentsInChildren<TextMeshProUGUI>();

            if (i < leaderboard.Count && texts.Length >= 2)
            {
                texts[0].text = leaderboard[i].playerName;
                texts[1].text = FormatTime(leaderboard[i].time);
            }
            else if (texts.Length >= 2)
            {
                texts[0].text = "---";
                texts[1].text = "--:--";
            }
        }
    }

    string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{minutes:00}:{secs:00}";
    }

    // -------------------- PHOTON INTEGRATION -------------------- //

    void SaveLeaderboardToPhoton()
    {
        if (!PhotonNetwork.InRoom) return;

        List<string> serialized = new List<string>();
        foreach (var entry in leaderboard)
            serialized.Add($"{entry.playerName}|{entry.time}");

        Hashtable props = new Hashtable { { LEADERBOARD_KEY, serialized.ToArray() } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(LEADERBOARD_KEY))
        {
            object[] serialized = (object[])propertiesThatChanged[LEADERBOARD_KEY];
            DeserializeLeaderboard(serialized);
        }
    }

    void DeserializeLeaderboard(object[] serialized)
    {
        leaderboard.Clear();
        foreach (var obj in serialized)
        {
            string[] parts = obj.ToString().Split('|');
            if (parts.Length == 2 && float.TryParse(parts[1], out float time))
                leaderboard.Add(new LeaderboardEntry(parts[0], time));
        }

        leaderboard.Sort((a, b) => b.time.CompareTo(a.time));
        UpdateLeaderboardUI();
    }
}