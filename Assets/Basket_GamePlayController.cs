using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;
using UnityEngine.UIElements;
using DG.Tweening;


public class Basket_GamePlayController : MonoBehaviour
{
    public static Basket_GamePlayController instance;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject enemyBallPrefab;
    [SerializeField] Transform ballParent;
    [SerializeField] Basket_RingControll ringControll;
    public Camera mainInGameCamera;
    public Transform targetPosition;
    public List<GameObject> playerBallControllers;
    public List<GameObject> enemyBallControllers;
    public List<GameObject> modelBall;
   // public BallController ballController;

    public bool isTouchTopGoal = false;

    public bool isGameStart;
    public bool isPlayerTurn;

    public int Turn = 1;
    
    public int player_ballThrowInTurn;
    public int enemy_BallThrowInTurn;

    public string player_Name = "TaiTue";
    public string enemy_Name = "Minh";

   

    public bool isFoundEnemy = false;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        Physics.gravity = new Vector3(0, -9.81f*6, 0);
    }

    private void OnDisable()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
    }

   

    public void StartNewGame()
    {
       // UIController.Instance.findMatchCanvas.gameObject.SetActive(false);
        if (playerBallControllers != null)
        {
            foreach (var controller in playerBallControllers)
            {
                Destroy(controller);
            }
        }
        if (modelBall != null)
        {
            foreach (var controller in modelBall)
            {
                Destroy(controller);
            }
        }
        //Basket_UIController.instance.findMatchCanvas?.gameObject.SetActive(false);
        playerBallControllers.Clear();
       // SpawnBallPlayer();
        player_ballThrowInTurn = 0;
        enemy_BallThrowInTurn = 0;

        enemyBallControllers.Clear();
        
        Basket_UIController.instance.endgame.gameObject.SetActive(false);
        Basket_UIController.instance.home.gameObject.SetActive(false);
        Basket_UIController.instance.ingame.gameObject.SetActive(true);
        Basket_UIController.instance.gameCamera.gameObject.SetActive(true);
        Basket_UIController.instance.homeCamera.gameObject.SetActive(false);
        Basket_UIController.instance.ingame.StartNotiWhenPlay();

        SpawnBallPlayer();
        //UIController.Instance.canvasScoreController.ResetUIandData();

        //UIController.Instance.mainViewCanvas.gameObject.SetActive(false);
        // UIController.Instance.canvasNoti.gameObject.SetActive(false);
    }

    public void SpawnNewBallEnemy()
    {
        for (int i = 0; i < 3; i++)
        {
            var position = new Vector3(-1.7f + (i * 1.8f), -2.16f, -23.15f);
            GameObject ball = Instantiate(enemyBallPrefab, position, Quaternion.identity, ballParent);
            ball.GetComponent<Basket_BallController>().isBallOfEnemy = true;
            //ball.layer = LayerMask.NameToLayer("ballEnemy");
            enemyBallControllers.Add(ball);
           
        }
        StartCoroutine(StartThrowEnemyBall());
    }

    IEnumerator StartThrowEnemyBall()
    {
        int count = 0;
        foreach(var controller in enemyBallControllers)
        {
            count++;
            controller.gameObject.GetComponent<Basket_BallController>().AIForBallThrow();
            yield return new WaitForSeconds(3.2f);
            if (count == 3)
            {
                WinTheGame();
            }
        }
        
    }



    public void SpawnBallPlayer( )
    {

        for (int i = 0; i < 3; i++)
        {
            
            var position = new Vector3(-1.7f+(i*1.8f), -2.16f, -23.15f);
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity, ballParent);
            ball.name = i.ToString();
            ball.GetComponent<Basket_BallController>().isBallOfEnemy = false;
            playerBallControllers.Add(ball);
        }
    }

    public void IncreateBallThrowInTurn(bool isPlayer)
    {
        if (isPlayer)
        {
            player_ballThrowInTurn += 1;
        }
        else
        {
            enemy_BallThrowInTurn += 1;
        }

        if (player_ballThrowInTurn == 3 && isPlayer == true)
        {
            StartCoroutine(ShowMessageAfterDelay(3.2f, enemy_Name + "'s turn" + Turn )); 
        }

        if (enemy_BallThrowInTurn < 3 && isPlayer == false)
        {
           // SpawnNewBallEnemy(3);
        }

        if (player_ballThrowInTurn == 3 && enemy_BallThrowInTurn > 3)
        {
            if (Basket_UIController.instance.ingameScore.enemy_Score == Basket_UIController.instance.ingameScore.player_Score)
            {
                //Turn 
            } 
            else
            {
                WinTheGame();
            }
           // WinTheGame();
        }
    }

    IEnumerator ShowMessageAfterDelay(float delay ,string content)
    {
        yield return new WaitForSeconds(delay);
        SpawnNewBallEnemy();
        //UIController.Instance.canvasNoti.gameObject.SetActive(true);
        //UIController.Instance.canvasNoti.noti.text = content;
       // UIController.Instance.canvasNoti.notiAnimation.Play();

        // Đặt các hành động khác bạn muốn thực hiện sau khi đợi 3 giây ở đây
    }

    public void WinTheGame()
    {
        Basket_UIController.instance.endgame.gameObject.SetActive(true);
        Basket_UIController.instance.endgame.UpdateFinishedGameUI();
    }

    public void SetUpTurnAndWaveToPlay()
    {
        //isPlayerTurn = true;
        isTouchTopGoal = false;
        //Turn = 1;
        player_ballThrowInTurn = 0;
        enemy_BallThrowInTurn = 0;
    }

    //public void StartFireEffect()
    //{
    //    ballController.StartFireEffect();
    //}

    //public void StopFireEffect()
    //{
    //    ballController.StopFireEffect();
    //}
}
    

    
    

