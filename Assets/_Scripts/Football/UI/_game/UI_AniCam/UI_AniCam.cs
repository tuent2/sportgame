using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI_AniCam : MonoBehaviour
{
    public static UI_AniCam instance;
    [SerializeField] Image BgImage;

    [SerializeField] Camera shotCamera;
    [SerializeField] Camera recordCamera;
    [SerializeField] Camera GoalKeeperCamera;
    private void Awake()
    {
        instance = this;
        
    }
    void Start()
    {
        AniWhenGetReadyShot(true);
    }

    public void AniWhenGetReadyShot(bool isFirstTime)
    {
        //Football_BallController.instance.SetToTheFristPosition();
        //BallController.instance.SetToTheFristPosition();
        if (GamePhaseController.instance.isSkipRecording == true) return;
        if (!isFirstTime)
        {
            if (!GamePhaseController.instance.isRecording)
            {
                GameUiController.instance.ui_Ingame.ChangeItemScoreShow();
                GamePhaseController.instance.isShoterTurn = !GamePhaseController.instance.isShoterTurn;
            }
            //GameUiController.instance.ui_Ingame.ChangeItemScoreShow();
            GamePhaseController.instance.isRecording = false;
        }


        //Time.timeScale = 1f;
        if (GamePhaseController.instance.isShoterTurn )
        {
            shotCamera.gameObject.SetActive(true);
            recordCamera.gameObject.SetActive(false);
            GoalKeeperCamera.gameObject.SetActive(false);
            BgImage.DOFade(1f, 0);
            BgImage.gameObject.SetActive(true);
            shotCamera.fieldOfView = 60;
            shotCamera.DOFieldOfView(43, 5.25f);

            shotCamera.transform.localPosition = new Vector3(22.8f, 0.94f, 0);
            BgImage.DOFade(0f, 5.25f);
            //ShotCam = (29,1.85,0);
            
            shotCamera.transform.DOMove(new Vector3(29, 1.85f, 0), 5.25f).OnComplete(() => {
                BgImage.gameObject.SetActive(false);
                GamePhaseController.instance.isCanShot = true;
                GamePhaseController.instance.isGoal = false;
                GamePhaseController.instance.isSkipRecording = false;
            });
        }
        else
        {
            
            GoalKeeperCamera.gameObject.SetActive(true);
            recordCamera.gameObject.SetActive(false);
            shotCamera.gameObject.SetActive(false);
            BgImage.DOFade(1f, 0);
            BgImage.DOFade(0f, 5.25f);
            BgImage.gameObject.SetActive(true);
            GoalKeeperCamera.fieldOfView = 55;
            GoalKeeperCamera.DOFieldOfView(50, 5.25f);
            GoalKeeperCamera.transform.localPosition = new Vector3(75f, 2f, -1.28f);
            GoalKeeperCamera.transform.DOMove(new Vector3(70, 2f, -1.28f), 5.25f).OnComplete(() => {
                BgImage.gameObject.SetActive(false);
                GamePhaseController.instance.isCanShot = true;
                GamePhaseController.instance.isGoal = false;
                Football_BallController.instance.PlayerAniShotAni();
                GamePhaseController.instance.isSkipRecording = false;
            });

        }

        GameUiController.instance.ui_Ingame.SetUpUIWhenReplayOrNot(false);
    }

    public void AniRecordWhenGetScore()
    {
        Football_BallController.instance.SetToTheFristPosition();
        if (GamePhaseController.instance.isSkipRecording == true) return;
        GameUiController.instance.ui_Ingame.SetUpUIWhenReplayOrNot(true);
       // BallController.instance.SetToTheFristPosition();
        shotCamera.gameObject.SetActive(false);
        recordCamera.gameObject.SetActive(true);

        BgImage.DOFade(1f, 0);
        BgImage.gameObject.SetActive(true);
        BgImage.DOFade(0f, 2f);
        BgImage.gameObject.SetActive(true);
        recordCamera.transform.DOMove(new Vector3(27, 4, 0), 2f).OnComplete(() => {
            BgImage.gameObject.SetActive(false);
            PlayerController.instance.PlayerAniShotAni();
            GoalKeeperController.instance.AniWhenRecord();
            //GoalKeeperController.instance.PlayAnimation("");
            GamePhaseController.instance.isGoal = false;
            Time.timeScale = 0.4f;
            GamePhaseController.instance.isSkipRecording = false;
        });
    }
}
