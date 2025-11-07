using UnityEngine;

public class LimbHealthUI : MonoBehaviour
{
    //goes on each BASE limb UI icon
    public GameObject[] healthIndicator; //green yellow and red
    [SerializeField]
    int healthIndex = -1;

    public void Start()
    {
        healthIndex = -1;
        ChangeUI();// starts green
    }

    //set all ui inactive and activate curHealth indicator and incriment for next time limb takes damage
    public void ChangeUI()
    {
        if(healthIndex <= healthIndicator.Length)
        {
            healthIndex++;
            foreach (GameObject ui in healthIndicator)
            {
                ui.SetActive(false);
            }
            healthIndicator[healthIndex].SetActive(true);
        }

    }
}