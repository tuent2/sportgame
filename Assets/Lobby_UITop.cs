using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Lobby_UITop : MonoBehaviour
{
    [SerializeField] Button MiniMap;
    [SerializeField] Button OutMiniMap;
    
    private void Start()
    {
        MiniMap.gameObject.SetActive(true);
        OutMiniMap.gameObject.SetActive(false);
    }
    public void ClickMiniMapButton()
    {
        MiniMap.gameObject.SetActive(false);
        OutMiniMap.gameObject.SetActive(true);
        Lobby_UIController.instance.ClickMiniMap();
        

        //ClickButtonMinimap?.Invoke();
    }

    public void ClickOutMiniMapButton()
    {
        MiniMap.gameObject.SetActive(true);
        OutMiniMap.gameObject.SetActive(false);
        Lobby_UIController.instance.ClickOutMiniMap();
    }

    public void ClickBackToHomeScence()
    {
        SceneManager.LoadScene("_home");
    }


}
