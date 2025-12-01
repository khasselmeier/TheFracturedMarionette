using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YoinkScore : MonoBehaviour
{
    public string userName;
    public float score;
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

    public void NameEntry()
    {
        userName = inputField.text;
    }

    public void OnSubmitName()
    {
        if (userName == null)
        {

        } else
        {
            scoreboardScreen.SetActive(true);
            nameEntryScreen.SetActive(false);
        }
    }
}
