using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket_StartGameWhenCoinAniOver : MonoBehaviour
{
    public void StartGameWhenAniEnd()
    {
        //Debug.Log("123");
        Basket_GamePlayController.instance.isGameStart = true;
        Basket_UIController.instance.ingame.StartGame() ;
        foreach (GameObject ball in Basket_GamePlayController.instance.playerBallControllers)
        {
            ball.GetComponent<Basket_BallController>().StartPlay();

        }
    }
}
