using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUiController : MonoBehaviour
{
    public static GameUiController instance;

    public UI_AniCam ui_AniCam;
    public UI_Ingame ui_Ingame;
    //public UI_GameFinish ui_GameFinish;
    private void Awake()
    {
        instance = this;
        ui_AniCam.gameObject.SetActive(true);
    }

    public void OpenGameOver()
    {
        ui_AniCam.gameObject.SetActive(false);
        ui_Ingame.gameObject.SetActive(false);
        //ui_GameFinish.gameObject.SetActive(true);
    }
   
}
