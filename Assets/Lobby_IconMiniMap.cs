using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_IconMiniMap : MonoBehaviour
{
    [SerializeField] Data.TypeOfIconMiniMap type;

    private void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("Player"))
        {
            Lobby_Controller.instance.lobbyPlayer.Stop();
            Lobby_UIController.instance.lobby_CfMode.UpdateUI(type);
            Lobby_UIController.instance.lobby_CfMode.gameObject.SetActive(true);
        }
    }

    public void OnClickIcon()
    {
        Lobby_UIController.instance.lobby_CfFindWayMiniMap.gameObject.SetActive(true);
        
        switch (type)
        {
            case Data.TypeOfIconMiniMap.FootBall:

                Lobby_UIController.instance.lobby_CfFindWayMiniMap.OpenFindWayMiniMapPanel
                    (type, "Are you want to go Football game?");
                break;
            case Data.TypeOfIconMiniMap.BasketBall:

                Lobby_UIController.instance.lobby_CfFindWayMiniMap.OpenFindWayMiniMapPanel
                   (type, "Are you want to go BasketBall game?");
                break;
            default:
                Debug.Log("Unknown icon type.");
                break;
        }
    }

   
}
