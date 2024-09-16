using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class UI_Ingame : MonoBehaviour
{
    [Space]
    [SerializeField] GameObject foots;
    [SerializeField] GameObject skipReplay;
    [Space]
    [SerializeField] Button LeftFoot;
    [SerializeField] Button RightFoot;
    
    //[Space]
    //[SerializeField] 
    [Space]
    [SerializeField] List<UI_ItemScoreShow> homeItemScoreShow;
    [SerializeField] List<UI_ItemScoreShow> competitorItemScoreShow;
    private void Start()
    {
        LeftFoot.interactable = true;
        RightFoot.interactable = false;
    }
    public void ChangeFootToShot()
    {
        if (PlayerController.instance.isRightFoot)
        {
           // Debug.Log("Sang trái");
            LeftFoot.interactable = false;
            RightFoot.interactable = true;

            PlayerController.instance.isRightFoot = false;

            
        }
        else
        {
           // Debug.Log("Sang phải");
            RightFoot.interactable = false;
            LeftFoot.interactable = true;

            //PlayerController.instance.PlayerAniPlay("RunSideRight");

           
            PlayerController.instance.isRightFoot = true;
        }

      //  PlayerController.instance.PlayerWalkAni();
    }

    public void ChangeItemScoreShow()
    {
        if (GamePhaseController.instance.isHomeTurn)
        {
            homeItemScoreShow[GamePhaseController.instance.ShotTurn].SetIsGoal();

        }
        else
        {
            competitorItemScoreShow[GamePhaseController.instance.ShotTurn].SetIsGoal();
            if (GamePhaseController.instance.ShotTurn < 4)
                GamePhaseController.instance.ShotTurn++;
            else
                GameUiController.instance.OpenGameOver();
        }
        
        GamePhaseController.instance.isHomeTurn = !GamePhaseController.instance.isHomeTurn;

        if (GamePhaseController.instance.isHomeTurn)
        {
            homeItemScoreShow[GamePhaseController.instance.ShotTurn].SetIsSelected(true);
        }
        else
        {
            competitorItemScoreShow[GamePhaseController.instance.ShotTurn].SetIsSelected(true);
        }
    }

    //void SetImageAlpha(Image img, float alpha)
    //{
    //    // Lấy màu hiện tại của ảnh
    //    Color currentColor = img.color;

    //    // Tạo một màu mới với giá trị alpha mới
    //    Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

    //    // Gán màu mới lại cho ảnh
    //    img.color = newColor;
    //}

    public void SetUpUIWhenReplayOrNot(bool isReplay)
    {       
        if (!GamePhaseController.instance.isShoterTurn)
        {
            foots.SetActive(false);
        }
        else
        {
            foots.SetActive(!isReplay);
        }
            //foots.SetActive(!isReplay);
            skipReplay.SetActive(isReplay);
    }

    public void SkipRecordAction()
    {

        Football_BallController.instance.SetUpReadyShot();
        GamePhaseController.instance.isSkipRecording = true;
    }
}
