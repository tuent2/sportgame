using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class GoalKeeperController : MonoBehaviour
{
    public static GoalKeeperController instance;
    [SerializeField] Animator EnemyAni;
    [SerializeField] private Vector2 direction; // touch start position, touch end position, swipe direction
    [SerializeField] private float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction
    [SerializeField]
    private Vector3 startPos, endPos;
    int AniType;

    private float elapsedTime;
    private bool isRunning = false;
    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isRunning)
        {
            
            elapsedTime += Time.deltaTime;
        }

        if (!GamePhaseController.instance.isCanShot || GamePhaseController.instance.isShoterTurn) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Ghi lại thời gian khi chuột được nhấn và vị trí ban đầu
            touchTimeStart = Time.time;
            startPos = Input.mousePosition;
        }

        // Kiểm tra nếu chuột được thả ra
        if (Input.GetMouseButtonUp(0))
        {

            // Ghi lại thời gian khi chuột được thả ra
            touchTimeFinish = Time.time;

            // Tính toán khoảng thời gian giữa hai lần nhấn chuột
            // timeInterval = touchTimeFinish - touchTimeStart;

            // Lấy vị trí chuột khi được thả ra
            endPos = Input.mousePosition;

            //endPos = Camera.main.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y, Camera.main.transform.position.y));
            //startPos = Camera.main.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, Camera.main.transform.position.y));
            //endPos.                                                                                   
            //float a =  Mathf.Clamp(endPos.y, 0, 600f);
            //endPos = new Vector3(endPos.x, a, endPos.y);
            // Tính toán hướng di chuyển của chuột trong không gian 2D
            direction = startPos - endPos;

            //float a = Mathf.Clamp(direction.x, -500f, 1000f);
            //float b = Mathf.Clamp(direction.y, -500f, 1000f);
            //direction = new Vector2(a, b);

            // Thêm lực cho rigidbody của đối tượng 3D dựa trên hướng và lực ném
            // rb.isKinematic = false;
            if (Vector3.Distance(startPos, endPos) >= 1.5f)
            {
                isRunning = false;
                Debug.Log("Time la: " + elapsedTime);
                //GamePhaseController.instance.isCanShot = false;
                //PlayerController.instance.PlayerAniShotAni();
                if (startPos.x - endPos.x > 40 && startPos.x - endPos.x < 300)
                {
                    AniType = 1;
                    EnemyAni.Play("JumpA1Left");
                }
                else if (endPos.x - startPos.x > 40 && endPos.x - startPos.x < 300)
                {
                    AniType = -1;
                    EnemyAni.Play("JumpA1Right");
                }
                else if (startPos.x - endPos.x > 0 && startPos.x - endPos.x <= 40)
                {

                }
                else if (endPos.x - startPos.x > 0 && endPos.x - startPos.x <= 40)
                {

                }
                else if (startPos.x - endPos.x >= 300)
                {
                    AniType = 2;
                    EnemyAni.Play("JumpB1_2Left");

                }
                else if (endPos.x - startPos.x >= 300)
                {
                    AniType = -2;
                    EnemyAni.Play("JumpB1_2Right");
                }
            }

            // Destroy(gameObject, 3f);
           

            //StartCoroutine(PlaySoundAfterDelay(3f));
        }
    }
    public void IsStartCountTimeAni()
    {
       
            elapsedTime = 0;
            isRunning = true;
        
    }
    public void PlayAnimation(string name)
    {
        if (GamePhaseController.instance.isShoterTurn )
        EnemyAni.Play(name);
        // StartCoroutine(CheckGoalOrNot());
        //else if (GamePhaseController.instance.isRecording == true && !GamePhaseController.instance.isShoterTurn)
        //{
        //    StartCoroutine(AniGoalKeeperWhenRecord());
        //}
    }

    public void AniWhenRecord()
    {
        if (GamePhaseController.instance.isRecording == true && !GamePhaseController.instance.isShoterTurn)
        {
            StartCoroutine(AniGoalKeeperWhenRecord());
        }
    }

    IEnumerator AniGoalKeeperWhenRecord()
    {
        yield return new WaitForSeconds(elapsedTime );
        if (AniType == 1)
        {
            //AniType = 1;
            EnemyAni.Play("JumpA1Left");
        }
        else if (AniType == -1)
        {
            //AniType = -1;
            EnemyAni.Play("JumpA1Right");
        }
        else if (startPos.x - endPos.x > 0 && startPos.x - endPos.x <= 40)
        {

        }
        else if (endPos.x - startPos.x > 0 && endPos.x - startPos.x <= 40)
        {

        }
        else if (AniType == 2)
        {
            //AniType = 2;
            EnemyAni.Play("JumpB1_2Left");

        }
        else if (AniType == -2)
        {
            //AniType = -2;
            EnemyAni.Play("JumpB1_2Right");
        }
        AniType = 0;
    }
    
    int randomAniSad;
    IEnumerator CheckGoalOrNot()
    {
        yield return new WaitForSeconds(2f);
        //Debug.Log("thu mon" + GamePhaseController.instance.isGoal);
        if (GamePhaseController.instance.isGoal)
        {
            randomAniSad = Random.Range(1, 2);
            if (randomAniSad == 0) yield break;
            else if (randomAniSad == 1)
            {
                //Debug.Log("thu mon1233" + GamePhaseController.instance.isGoal);
                EnemyAni.SetTrigger("Sadness_1");
                //Debug.Log("thu mon3333" + GamePhaseController.instance.isGoal);
            }
            else if (randomAniSad == 2)
            {
                //Debug.Log("thu mon123" + GamePhaseController.instance.isGoal);
                EnemyAni.SetTrigger("Sadness_2");
                //Debug.Log("thu mon333" + GamePhaseController.instance.isGoal);
            }
            else if (randomAniSad == 3)
            {
                EnemyAni.SetTrigger("Sadness_3");
            }
            else if (randomAniSad == 4)
            {
                EnemyAni.SetTrigger("Sadness_4");
            }
        }
    }

    public void SetToTheFristLeftPosition()
    {

        transform.localPosition = new Vector3(51.77f, 0,0);
        transform.localRotation = Quaternion.Euler(0, -90f, 0);
       
        gameObject.SetActive(true);
    }

    public void FadeGoalKeeper()
    {

    }
}
