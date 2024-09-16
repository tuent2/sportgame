using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Lobby_UIController : MonoBehaviour
{
    public static Lobby_UIController instance;
    [Header("Camera")]
    public Camera lobby_PlayerFollowCamera;
    public Camera lobby_MiniMapCamera;
    [Header("Joystick")]
    public Joystick lobby_joystick;
    [Header("UI")]
    public Lobby_UITop lobby_uiTop;
    public Lobby_CfFindWayMiniMap lobby_CfFindWayMiniMap;
    public Lobby_CfMode lobby_CfMode;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lobby_MiniMapCamera.gameObject.SetActive(false);
        lobby_PlayerFollowCamera.gameObject.SetActive(true);
    }

    public void ClickMiniMap()
    {
        lobby_joystick.gameObject.SetActive(false);
        //------------------Camera--------------------------
        lobby_PlayerFollowCamera.gameObject.SetActive(false);
        lobby_MiniMapCamera.gameObject.SetActive(true);
        lobby_MiniMapCamera.gameObject.transform.localPosition= new Vector3(-22f, 18f, -20.29f);
        lobby_MiniMapCamera.transform.DOMove(new Vector3(-25f, 18f, -14.4f), 1f);
    }

    public void ClickOutMiniMap()
    {
        lobby_joystick.gameObject.SetActive(true);
        //------------------Camera--------------------------
        lobby_PlayerFollowCamera.gameObject.SetActive(true);
        lobby_MiniMapCamera.gameObject.SetActive(false);
        
    }

    

}
