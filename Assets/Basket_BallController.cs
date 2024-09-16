using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Basket_BallController : MonoBehaviour
{
    public bool isBallOfEnemy;
    public bool canSelected = true;
    public bool isSelected = false;
    [SerializeField] private Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    [SerializeField] private float touchTimeFinish, timeInterval, swpiteDistance; // to calculate swipe time to sontrol throw force in Z direction
    public float touchTimeStart;
    [SerializeField]
    float throwForceInXandY = 1f; // to control throw force in X and Y directions

    private Vector3 angle;
    private float BallSpeed = 0;
    public float BallMaxSpeed = 350;
    private float BallVelocity = 1;

    [SerializeField]
    float throwForceInZ = 50f; // to control throw force in Z direction

    Rigidbody rb;
    private Vector3 newPositon;
    Camera cam;

    public TrailRenderer trailRenderer;
    public ParticleSystem FireEffect;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Basket_GamePlayController.instance.mainInGameCamera;
        //Physics.gravity = new Vector3(-0.001f, -59.81f,0);
        //Debug.Log("Gravity:" + (Physics.gravity));
       
        rb.isKinematic = true;
        canSelected = true;
       // rb.gravityScale = 2f


    }

    public void AIForBallThrow()
    {
        
        int hitpersent = Random.Range(0, 1);
        //if (hitpersent > 0.3)
        //{
            var position = new Vector3(Random.Range(-3.9f, 3.9f), -1f, -21.5f);
            //Debug.Log(position);
            transform.DOMove(position, 1f).OnComplete(() =>
            {
                Vector3 direction = Basket_GamePlayController.instance.targetPosition.position - position;
                rb.isKinematic = false;
                rb.useGravity = true;
               // rb.AddForce(direction.normalized * 59.81f );
                rb.AddForce(new Vector3(direction.normalized.x *5 , direction.normalized.y * 59.81f *5f  , 80.00f) , ForceMode.Impulse);
                if (Basket_GamePlayController.instance.enemy_BallThrowInTurn < 5)
                {
                    Basket_GamePlayController.instance.IncreateBallThrowInTurn(false);

                }
                Destroy(gameObject, 3f);
                
                //Debug.Log(new Vector3(direction.normalized.x, direction.normalized.y * 59.81f * 5f, 83.00f));
            });
            
        //}
    }
    
    
    

    [Button("Them Luc")]
    public void TestLuc()
    {
        rb.AddForce(new Vector3(-0.01f, 83.95f, 87.00f), ForceMode.Impulse);
    }
    public void StartPlay()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public void StartFireEffect()
    {
        FireEffect.gameObject.SetActive(true);
        FireEffect.Play();
    }

    public void StopFireEffect()
    {
        FireEffect.gameObject.SetActive(false);
        FireEffect.Stop();
    }

    void PickUpTheBall()
    {
       
        Vector3 mousePos = Input.mousePosition;
        //Debug.Log("kkk" + mousePos);
        mousePos.z = cam.nearClipPlane * 30f;
        newPositon = cam.ScreenToWorldPoint(mousePos);
        //Debug.Log(newPositon);
        gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.position, newPositon, 160 * Time.deltaTime);
        gameObject.transform.localPosition = new Vector3(gameObject.transform.position.x, Mathf.Clamp(gameObject.transform.position.y, -2.34f, gameObject.transform.position.y), -23.5f);
        rb.useGravity = false;

        // isSelected = true;
    }
    float currentTime;
    public float mouseVelocity = 0f;
    float timeDelta;
    Vector2 mouseDelta;
    // Update is called once per frame
    void Update()
    {
        
        if (isSelected == true)
        {

            PickUpTheBall();
        }
        if (Basket_GamePlayController.instance.isGameStart /*&& GamePlayController.instance.isPlayerTurn*/ && canSelected && isBallOfEnemy == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;


                if (Physics.Raycast(ray, out hit, 20f))
                {

                    // có thể dùng tag và layer ở đây
                    if (hit.transform == gameObject.transform)
                    {
                        touchTimeStart = Time.time;
                        startPos = Input.mousePosition;

                        endPos = startPos;
                        touchTimeFinish = touchTimeStart;
                        isSelected = true;
                        PickUpTheBall();

                    }
                }

            }
            if (Input.GetMouseButton(0))
            {
                if (isSelected)
                {
                    Vector2 currentMousePosition = Input.mousePosition;
                    float currentTime = Time.time;

                    mouseDelta = currentMousePosition - endPos;

                    timeDelta = Mathf.Clamp(currentTime - touchTimeFinish, currentTime - touchTimeFinish, 0.25f);
                    mouseVelocity = Mathf.Clamp((mouseDelta.magnitude / timeDelta), (mouseDelta.magnitude / timeDelta), 14999f);
                    endPos = currentMousePosition;
                    // Debug.Log(mouseVelocity);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (isSelected == true)
                {
                    touchTimeFinish = Time.time;
                    endPos = Input.mousePosition;
                    swpiteDistance = (endPos - startPos).magnitude;
                    // Vector3 Distan3 = endPos - startPos;
                    timeInterval = touchTimeFinish - touchTimeStart;
                    trailRenderer.enabled = true;
                    //rb.useGravity = true;
                    //isSelected = false;
                   // if (mouseVelocity != 30)
                    // if (swipeTime < 0.5f && swpiteDistance > 30f)
                    //{
                        CalSpeed();
                        CalAgle();
                    Vector3 Force = new Vector3(mouseDelta.x / (touchTimeFinish - currentTime) / 10, Mathf.Clamp((angle.y * BallSpeed) / 45f, (angle.y * BallSpeed) / 45f, 98f), 80f);
                    Debug.Log(Force);
                    Debug.Log(Force.normalized);
                     rb.AddForce(Force,ForceMode.Impulse);

                    //}
                    // rb.AddForce(new Vector3(0f, 0f, 0f), ForceMode.Impulse);
                    rb.useGravity = true;
                    isSelected = false;
                    //rb.AddForce(new Vector3(-0.01f, 83.95f, 87.00f), ForceMode.Impulse);
                    Basket_GamePlayController.instance.IncreateBallThrowInTurn(true);
                    canSelected = false;
                    Destroy(gameObject, 3f);
                    Basket_GamePlayController.instance.playerBallControllers.Remove(gameObject);
                }


            }
        } 
    }

    private void CalAgle()
    {
        angle = cam.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y + 800f, (cam.nearClipPlane + 30)));
       // Debug.Log(angle.z);
    }

    void CalSpeed()
    {
        if (timeInterval > 0)
            BallVelocity = swpiteDistance / (swpiteDistance - timeInterval);

        BallSpeed = BallVelocity * 40;
        if (BallSpeed <= BallMaxSpeed)
        {
            BallSpeed = BallMaxSpeed;
        }
        //if (BallSpeed <= BallMaxSpeed)
        //{
        //    BallSpeed = BallMaxSpeed += 40;
        //}
        timeInterval = 0;
    }


    private void OnDestroy()
    {
        // BallManager.instance.Spawn();
    }
  
    public int Touch;

    private void OnCollisionEnter(Collision collision)
    {
        
        trailRenderer.enabled = false;
        trailRenderer.Clear();
    }
   
}