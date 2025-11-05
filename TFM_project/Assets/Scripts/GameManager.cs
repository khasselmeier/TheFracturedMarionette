using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public MasterUIManager MasterUIManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    
}
