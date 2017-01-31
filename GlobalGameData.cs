using UnityEngine;
using System.Collections;

public class GlobalGameData : MonoBehaviour {
    static private GlobalGameData instance;

    public void Awake()
    {
        instance = this;
    }

    public void IncreaseWinScore()
    {
        if (PlayerPrefs.HasKey("wins"))
        {
            int wins = PlayerPrefs.GetInt("wins");
            PlayerPrefs.SetInt("wins", wins + 1);
        }
        else
            PlayerPrefs.SetInt("wins", 1);
    }

    public int GetWinScore()
    {
        return PlayerPrefs.HasKey("wins") ? PlayerPrefs.GetInt("wins") : -1;
    }

    public void ResetWinScore()
    {
        if (PlayerPrefs.HasKey("wins"))
        {
            PlayerPrefs.SetInt("wins", 0);
        }
    }

    static public GlobalGameData INSTANCE
    {
        get { return instance; }
    }
}
