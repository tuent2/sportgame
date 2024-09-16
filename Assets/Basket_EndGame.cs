using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Basket_EndGame : BaseView
{
    [SerializeField] TextMeshProUGUI Text_NotiWhoWin;

    [SerializeField] TextMeshProUGUI Text_EnemyName;
    [SerializeField] TextMeshProUGUI Text_EnemyScore;
    [SerializeField] TextMeshProUGUI Text_PlayerScore;
    private void Awake()
    {
        base.Awake();
    }

   

   
    public void UpdateFinishedGameUI()
    {
       // Debug.Log("!23");
        Text_EnemyName.text =  Basket_GamePlayController.instance.enemy_Name;
        Text_EnemyScore.text = Basket_UIController.instance.ingameScore.enemy_Score.ToString();
        Text_PlayerScore.text = Basket_UIController.instance.ingameScore.player_Score.ToString();
        if (Basket_UIController.instance.ingameScore.player_Score > Basket_UIController.instance.ingameScore.enemy_Score)
        {
            Text_NotiWhoWin.text = Basket_GamePlayController.instance.player_Name + " Win!";
        }
        else if (Basket_UIController.instance.ingameScore.player_Score < Basket_UIController.instance.ingameScore.enemy_Score)
        {
            Text_NotiWhoWin.text = Basket_GamePlayController.instance.enemy_Name + " Win!";
        }
        else if (Basket_UIController.instance.ingameScore.player_Score == Basket_UIController.instance.ingameScore.enemy_Score)
        {
            Text_NotiWhoWin.text = "DRAW!";
        }


    }



    public void PlayAgainButtonClick()
    {
        //Basket_GamePlayController.instance.StartNewGame();
        this.hide();
        Basket_UIController.instance.ingame.gameObject.SetActive(false);
        Basket_UIController.instance.StartFindEnemy();
    }
}
