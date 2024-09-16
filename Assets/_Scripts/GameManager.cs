using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public enum EFFECT_POPUP
    {
        NONE,
        SCALE,
        MOVE_LEFT,
        MOVE_RIGHT,
        MOVE_UP,
        MOVE_DOWN
    }

    public enum GAME_PHASE
    {
        Home,
        InGame
    }

    public enum GAME_MODE
    {
        Penalty,
        FreeKick
    }

    public enum PENALTY_TYPE
    {
        Shotter,
        GoalKeeper
    }

    public static GAME_PHASE game_phase = GAME_PHASE.Home;
    public static GAME_MODE game_mode = GAME_MODE.Penalty;
    public static PENALTY_TYPE penalty_type = PENALTY_TYPE.Shotter; 
    public static bool isSound
    {
        get { return PlayerPrefs.GetInt("IsSound", 1) == 1; }
        set { PlayerPrefs.SetInt("IsSound", value ? 1 : 0); }
    }
    public static bool isMusic
    {
        get { return PlayerPrefs.GetInt("IsMusic", 1) == 1; }
        set { PlayerPrefs.SetInt("IsMusic", value ? 1 : 0); }
    }



  
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        
    }

   
    
}
