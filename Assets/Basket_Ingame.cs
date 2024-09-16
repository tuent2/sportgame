using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Basket_Ingame : MonoBehaviour
{
    public TextMeshProUGUI noti;
    public Animator coinAni;
    [SerializeField] float timeNextTextLoading = 1f;
    [SerializeField] float timeCount = 60f;

    [SerializeField] Image CoinImage;
    private void Awake()
    {
    }
    void Start()
    {
        
    }
    public void StartGame()
    {
        CoinImage.gameObject.SetActive(false);
    }
    public void StartNotiWhenPlay()
    {
        int isPlayerTurn = Random.Range(1, 2);
        if (isPlayerTurn == 1)
        {
            coinAni.SetBool("isPlayerWin", false);
        }
        else
        {
            coinAni.SetBool("isPlayerWin", true);
        }

        StartCoroutine(StartCount());
    }

    IEnumerator StartCount()
    {
        yield return new WaitForSecondsRealtime(6f);
        //for (int i = (int)timeCount; i >= 0; i--)
        //{
        //    //int minutes = i / 60;
        //    //int seconds = i % 60;
        //    //  noti.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //    noti.text = i.ToString();
        //    yield return new WaitForSecondsRealtime(timeNextTextLoading);
        //    if (i == 0)
        //    {
        //        noti.text = "Start!";
        //        yield return new WaitForSecondsRealtime(timeNextTextLoading);
        //        noti.gameObject.SetActive(false);
        //        GamePlayController.instance.isGameStart = true;

        //        CanvasScoreController.instance.StartCountTimeInturn();
        //        foreach (GameObject ball in GamePlayController.instance.playerBallControllers)
        //        {
        //            ball.GetComponent<BallController>().StartPlay();

        //        }

        //    }
        //}
        Basket_GamePlayController.instance.isGameStart = true;
       // Home_UIController.instance.canvasNoti.gameObject.SetActive(false);
        foreach (GameObject ball in Basket_GamePlayController.instance.playerBallControllers)
        {
            ball.GetComponent<Basket_BallController>().StartPlay();

        }
    }

    private void OnDisable()
    {
      //  StopCoroutine(StartCount(1.5f));
    }
}