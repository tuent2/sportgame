using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class Football_BallController : MonoBehaviour
{
  //  public static event Action ClickDone;
    public static Football_BallController instance;
   // public bool isCanShot = false;
    //[SerializeField] private Vector2 startPos, endPos;
    [SerializeField] private Vector2 direction; // touch start position, touch end position, swipe direction
    [SerializeField] private float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction
    [SerializeField]
    private Vector3 startPos, endPos;
    [SerializeField]
    float throwForceInXandY = 1f; // to control throw force in X and Y directions

    [SerializeField]
    float throwForceInZ = 200f; // to control throw force in Z direction
    
    //[SerializeField] AudioSource ballAudio;
    //[SerializeField] AudioClip Catch;
    //[SerializeField] AudioClip Kick;

    //[SerializeField] TrailRenderer train;
    Rigidbody rb;
    [SerializeField] GameObject trainMove;
    [SerializeField] Camera ShoterCamera;
    //bool isFollow = false;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (!GamePhaseController.instance.isCanShot || !GamePhaseController.instance.isShoterTurn) return;

        if (Input.GetMouseButtonDown(0))
        {   
            
            touchTimeStart = Time.time;
            startPos = Input.mousePosition;
            //isFollow = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //isFollow = false;
            
            touchTimeFinish = Time.time;

           
            endPos = Input.mousePosition;

            direction = startPos - endPos;

            if (Vector3.Distance(startPos, endPos) >= 1.5f)
            {
                GamePhaseController.instance.isCanShot = false;
                PlayerController.instance.PlayerAniShotAni();
               
            }    
             
        }



    }
    bool isKick = false;
    public void BallShotKick()
    {
        if (isKick ) return;

        isKick = true;
        SoundController.instance?.PlayKickClip();

        
        timeInterval = 0.15f;
         rb.AddForce(throwForceInZ / 0.15f, -direction.y * throwForceInXandY,  direction.x * throwForceInXandY);

        if (startPos.x - endPos.x > 40 && startPos.x - endPos.x < 300)
        {
            GoalKeeperController.instance.PlayAnimation("JumpA1Right");
        }
        else if (endPos.x - startPos.x > 40 && endPos.x - startPos.x < 300)
        {
            GoalKeeperController.instance.PlayAnimation("JumpA1Left");  
        }
        else if (startPos.x - endPos.x > 0 && startPos.x - endPos.x <= 40)
        {
            
        }
        else if (endPos.x - startPos.x > 0 && endPos.x - startPos.x <= 40)
        {

        }
        else if (startPos.x - endPos.x >= 300)
        {

            GoalKeeperController.instance.PlayAnimation("JumpB1_2Right");
        }
        else if (endPos.x - startPos.x >= 300)
        {
            GoalKeeperController.instance.PlayAnimation("JumpB1_2Left");
        }

        StartCoroutine(PlaySoundAfterDelay());
    }
    IEnumerator PlaySoundAfterDelay()
    {
        
        yield return new WaitForSeconds(4f);

        //PlayerController.instance.gameObject.transform.localPosition = new Vector3(36.56f, 0, 1.07f);
        //transform.localPosition = new Vector3(41.5f, 0.142f, 0);
        // PlayerController.instance.PlayerAniPlay("Warm_1");
        PlayerController.instance.gameObject.SetActive(false);
        PlayerController.instance.SetToTheFristLeftPosition();
        GoalKeeperController.instance.gameObject.SetActive(false);
        GoalKeeperController.instance.SetToTheFristLeftPosition();

        SetToTheFristPosition();

        isKick = false;
        if (GamePhaseController.instance.isRecording)
        { 
            UI_AniCam.instance.AniRecordWhenGetScore();
        }
        else
            UI_AniCam.instance.AniWhenGetReadyShot(false);
    }

    public void SetUpReadyShot()
    {
        PlayerController.instance.gameObject.SetActive(false);
        PlayerController.instance.SetToTheFristLeftPosition();
        GoalKeeperController.instance.gameObject.SetActive(false);
        GoalKeeperController.instance.SetToTheFristLeftPosition();
        SetToTheFristPosition();
        isKick = false;
        GamePhaseController.instance.isRecording = false;
        UI_AniCam.instance.AniWhenGetReadyShot(false);
        Time.timeScale = 1f;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GoalKeeper"))
        {
            //SoundController.instance.PlayCatchClip();
        }

    }

    [Button("test")]
    public void SetToTheFristPosition()
    {
        transform.localPosition = new Vector3(41.5f, 0.1427501f, 0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Debug.Log(transform.localPosition);
    }
    public void PlayerAniShotAni()
    {

        StartCoroutine(WaitPlayerShotAuto());
    }

    IEnumerator WaitPlayerShotAuto()
    {

        yield return new WaitForSeconds(2f);
        if (GamePhaseController.instance.isRecording == false)
        {
            GoalKeeperController.instance.IsStartCountTimeAni();
        }
        direction = new Vector3(UnityEngine.Random.Range(-400, 400), UnityEngine.Random.Range(-350, 350), 0.00f);
        PlayerController.instance.PlayerAniShotAni();
    }

}
