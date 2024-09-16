using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Lobby_CfFindWayMiniMap : BaseView
{
    public Data.TypeOfIconMiniMap type { get; set; }
    [SerializeField] TextMeshProUGUI contentText;
    private void Awake()
    {
        base.Awake();
    }


    public void OpenFindWayMiniMapPanel(Data.TypeOfIconMiniMap datatype, string content)
    {
        type = datatype;
        contentText.text = content;
    }
    

    public void ClickConfirmPlay()
    {
        
        Lobby_UIController.instance.lobby_uiTop.ClickOutMiniMapButton();
        if (type == Data.TypeOfIconMiniMap.FootBall)
        {
            
            Lobby_Controller.instance.lobbyPlayer.FindToTheTarget(Lobby_Controller.instance.footballboth);
        }
        else if (type == Data.TypeOfIconMiniMap.BasketBall)
        {
            //Debug.Log("Choi basket ball");
            //SceneManager.LoadScene("_game");
            Lobby_Controller.instance.lobbyPlayer.FindToTheTarget(Lobby_Controller.instance.basketballboth);
        }
        hide();
    }

    
}
