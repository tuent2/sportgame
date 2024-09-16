using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FootBall_UIGameFinish : BaseView
{
    //public static FinishedGameCanvas instance;

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
        Debug.Log("!23");
        //Text_EnemyName.text = GamePlayController.instance.enemy_Name;
        //Text_EnemyScore.text = CanvasScoreController.instance.enemy_Score.ToString();
        //Text_PlayerScore.text = CanvasScoreController.instance.player_Score.ToString();
        //if (CanvasScoreController.instance.player_Score > CanvasScoreController.instance.enemy_Score)
        //{
        //    Text_NotiWhoWin.text = GamePlayController.instance.player_Name + " Win!";
        //}
        //else if (CanvasScoreController.instance.player_Score < CanvasScoreController.instance.enemy_Score)
        //{
        //    Text_NotiWhoWin.text = GamePlayController.instance.enemy_Name + " Win!";
        //}
        //else if (CanvasScoreController.instance.player_Score == CanvasScoreController.instance.enemy_Score)
        //{
        //    Text_NotiWhoWin.text = "DRAW!";
        //}


    }



    public void PlayAgainButtonClick()
    {
       // GamePlayController.instance.StartNewGame();
    }
}
