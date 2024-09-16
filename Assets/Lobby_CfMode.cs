using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Lobby_CfMode : BaseView
{
    [SerializeField] TextMeshProUGUI Title;
    [SerializeField] Image ContentImage;
    [SerializeField] Data.TypeOfIconMiniMap Type;
    [SerializeField] Sprite FootBallContent;
    [SerializeField] Sprite BasketBallContent;
    private void Awake()
    {
        base.Awake();
    }

    public void UpdateUI(Data.TypeOfIconMiniMap typeSelected)
    {
        Type = typeSelected;

        if (Type == Data.TypeOfIconMiniMap.FootBall)
        {
            Title.text = "Football";
            ContentImage.sprite = FootBallContent;
        }
        else
        {
            Title.text = "Basketball";
            ContentImage.sprite = BasketBallContent;
        }

    }

    public void ClickPlay()
    {
        if (Type == Data.TypeOfIconMiniMap.FootBall)
        {
            SceneManager.LoadScene("_football");
        }
        else
        {
            SceneManager.LoadScene("_basketball");
           // Debug.Log("Load Sence basketball");
        }
    }
}
