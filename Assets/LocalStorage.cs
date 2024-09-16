using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalStorage : MonoBehaviour
{
    public const string highestScore = "HighestScore";
    public const string UserId = "userId";
    public static int GetIntLocalStorage(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }

    public static void SetIntLocalStorage(string key, int score)
    {
        PlayerPrefs.SetInt(key, score);
        PlayerPrefs.Save();
    }

    

    public static int GetUserId()
    {
        return PlayerPrefs.GetInt(UserId, -1);
    }

    public void SetUsedId(int id)
    {
        PlayerPrefs.SetInt(UserId, id);
    }
}
