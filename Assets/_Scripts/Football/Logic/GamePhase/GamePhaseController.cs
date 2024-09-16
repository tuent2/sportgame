using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GamePhaseController : MonoBehaviour
{
    public static GamePhaseController instance;
   // [SerializeField] GameObject UIAni;
   
   
    public bool isRecording { set; get; }
    public bool isSkipRecording { set; get; }
    public bool isGoal { set; get; }

    public bool isCanShot { set; get; }
    public bool isCanCatch { set; get; }
    public int ShotTurn { set; get; }
    public bool isHomeTurn { set; get; }
    public bool isShoterTurn { set; get; }

    public int HomeScore { set; get; }
    public int RefereeScore { set; get; }


    private void Awake()
    {
        instance = this;
       /// UIAni.gameObject.SetActive(true);
        isRecording = false;
        isCanShot = false;
        ShotTurn = 0;
        isHomeTurn = true;
        isShoterTurn = true;
        isSkipRecording = false;

        HomeScore = 0;
        RefereeScore = 0;
       // BallController.ClickDone += Show;
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    private void OnDisable()
    {
       // BallController.ClickDone -= Show;
    }
    public void Show()
    {
        Debug.Log("Test");
    }
    private void OnEnable()
    {
        Globals.game_phase = Globals.GAME_PHASE.InGame;
    }

    void Start()
    {
        if (SoundController.instance != null)
            SoundController.instance.PlayAudiemtClip();
    }

    IEnumerator WaitToChange()
    {

        yield return new WaitForSeconds(0.2f);
        isCanShot = true;
    }

    public void ChangeIsCanShot()
    {
        StartCoroutine(WaitToChange());
    }
}
