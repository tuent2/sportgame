using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;

public class Basket_IngameScore : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI PlayerScore;
    [SerializeField] TextMeshProUGUI TimeCount;
    [SerializeField] TextMeshProUGUI EnemyScore;
    [SerializeField] TextMeshProUGUI Turn;
   // [SerializeField] TextMeshProUGUI PlayerName;
  //  [SerializeField] TextMeshProUGUI EnemyName;
   
    [SerializeField] float timeNextTextLoading = 1f;
    [SerializeField] float timeCount = 30f;

    public int countTurn = 0;
    public bool isFinalTime = false;
    public int player_Score = 0;
    public int enemy_Score = 0;

    
    private void Awake()
    {
       
    }
    
    void Start()
    {
        ResetUIandData();
    }
    public void ResetUIandData()
    {
        Turn.text ="Turn: " +countTurn;
        TimeCount.text = "00:30";
        PlayerScore.text = "0";
        EnemyScore.text = "0";
        player_Score = 0;
        enemy_Score = 0;
    }
    public void StopCountTime()
    {
        StopCoroutine(HandleShowTextLoading());
    }
    public void StartCountTimeInturn()
    {
        StartCoroutine(HandleShowTextLoading());
    }

    public void UpdateScore(int value,bool isPlayer)
    {
        if(isPlayer)
        {
            player_Score += value;
            PlayerScore.text = player_Score.ToString();
        }
        else
        {
            enemy_Score += value;
            EnemyScore.text = enemy_Score.ToString();
        }
        
    }

    IEnumerator HandleShowTextLoading()
    {
       
            for (int i = (int)timeCount; i >= 0; i--)
            {
                int minutes = i / 60;
                int seconds = i % 60;
                TimeCount.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                yield return new WaitForSecondsRealtime(timeNextTextLoading);
                if (i <= 10)
                {
                   // GamePlayController.instance.StartFireEffect();
                    isFinalTime = true;
                    TimeCount.color = Color.red;
                }
                else
                {
                   // GamePlayController.instance.StopFireEffect();
                    isFinalTime = false;
                    TimeCount.color = Color.green;
                }

                if (i <= 0)
                {
                    Basket_GamePlayController.instance.WinTheGame();
                }
            }
        
        
    }
}
