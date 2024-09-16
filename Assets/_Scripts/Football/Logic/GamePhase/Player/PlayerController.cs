using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] Animator PlayerAni;
    public bool isRightFoot { get; set; }
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        isRightFoot = true;
        PlayerAni.Play("Warm_1");
    }

    public void PlayerWalkAni()
    {
        PlayerAni.SetBool("Moving", true);
        PlayerAni.SetFloat("DiagonalAngle", 30);
        if (isRightFoot)
        {
            PlayerAni.SetFloat("X", 0.9f);
            PlayerAni.SetFloat("Y", 0f);
        }
    }

    
   
    [Button("Shot")]
    public void PlayerAniShotAni()
    {
        transform.localPosition = new Vector3(36.5f, 0.015f, 1.75f);
        //PlayerAni.Sto
        if (isRightFoot)
        {
            PlayerAni.SetTrigger("Kick_2");
            PlayerAni.SetInteger("Foot", 2);
        }
        else
        {

        }
        StartCoroutine(CheckGoalOrNot());
    }

    int randomAniSad;
    IEnumerator CheckGoalOrNot()
    {
        yield return new WaitForSeconds(2f);
        if (!GamePhaseController.instance.isGoal)
        {
            randomAniSad = Random.Range(1, 5);
            if (randomAniSad == 0) yield break;
            else if (randomAniSad == 1)
            {
                PlayerAni.SetTrigger("Sadness_1");
            }
            else if (randomAniSad == 2)
            {
                PlayerAni.SetTrigger("Sadness_2");
            }
            else if (randomAniSad == 3)
            {
                PlayerAni.SetTrigger("Sadness_3");
            }
            else if (randomAniSad == 4)
            {
                PlayerAni.SetTrigger("Sadness_4");
            }
        }
    }
        public void SetToTheFristLeftPosition()
    {
        
        transform.localPosition = new Vector3(36.5f, 0.015f, 1.75f);
        transform.localRotation = Quaternion.Euler(0, 111f, 0);
       // Debug.Log(transform.localPosition);
        gameObject.SetActive(true);
    }
}
