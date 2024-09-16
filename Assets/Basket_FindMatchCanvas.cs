using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Basket_FindMatchCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_finding;
    [SerializeField] TextMeshProUGUI text_enemyName;
    
    [SerializeField] Animator ani_finding;
    [SerializeField] Image image_magnifyGlass;
    [SerializeField] Image image_enemyAvatarCover;
    [SerializeField] Image image_avatar;
    [SerializeField] Button button_cancelFind;
    [SerializeField] Image image_enemyScore;
    [SerializeField] Image image_LeftBGFound;
    [SerializeField] Image image_RightBGFound;

    Coroutine findingCoroutine;

    void Start()
    {
        
    }

    IEnumerator FindingTextAni(bool isContinues)
    {
        while (isContinues)
        {
            yield return new WaitForSecondsRealtime(0.75f);
            text_finding.text = "Finding Opppent";
            yield return new WaitForSecondsRealtime(0.75f);
            // Debug.Log("2");
            text_finding.text = "Finding Opppent .";
            yield return new WaitForSecondsRealtime(0.75f);
            //Debug.Log("1");
            text_finding.text = "Finding Opppent ..";
            yield return new WaitForSecondsRealtime(0.75f);
            //Debug.Log("0");
            text_finding.text = "Finding Opppent ...";
           
           // gameObject.SetActive(false);
           //GameController.intance.logicGamePlay.isGamestart = true;
        }
        
    }

    public void StartFindingEnemy()
    {
        findingCoroutine = StartCoroutine(FindingTextAni(true));
        ani_finding.enabled = true;
        image_avatar.gameObject.SetActive(false);
        image_enemyScore.gameObject.SetActive(false);
        text_enemyName.text = "...";
        image_LeftBGFound.gameObject.SetActive(false);
        image_RightBGFound.gameObject.SetActive(false);

        //DOVirtual.DelayedCall(3f, Basket_GamePlayController.instance.FindedEnemy);
        StartCoroutine(FindEnemy());
    }

    IEnumerator FindEnemy()
    {
        yield return new WaitForSeconds(3f);
        FoundEnemy();
        yield return new WaitForSeconds(3f);
        //DOVirtual.DelayedCall(3f, StartNewGame);
        gameObject.SetActive(false);
        Basket_GamePlayController.instance.StartNewGame();
    }
    public void FoundEnemy()
    {

        image_LeftBGFound.rectTransform.localPosition = new Vector3(-Screen.width, image_LeftBGFound.rectTransform.localPosition.y, image_LeftBGFound.rectTransform.localPosition.z);
        image_RightBGFound.rectTransform.localPosition = new Vector3(Screen.width, image_LeftBGFound.rectTransform.localPosition.y, image_LeftBGFound.rectTransform.localPosition.z);
        image_LeftBGFound.gameObject.SetActive(true);
        image_RightBGFound.gameObject.SetActive(true);

        image_LeftBGFound.rectTransform.DOLocalMoveX(0, 0.35f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            ani_finding.enabled = false;
            image_magnifyGlass.gameObject.SetActive(false);
            button_cancelFind.gameObject.SetActive(false);
            image_avatar.gameObject.SetActive(true);
            image_enemyScore.gameObject.SetActive(true);
            text_enemyName.text = Basket_GamePlayController.instance.enemy_Name;
            if (findingCoroutine != null)
            {
                FindingTextAni(false);
                StopCoroutine(findingCoroutine);
            }
            //StopCoroutine(findingCoroutine);
            text_finding.text = "Loading Game ...";
        });
        image_RightBGFound.rectTransform.DOLocalMoveX(0, 0.35f).SetEase(Ease.InQuad);
    }


    public void ClickCancelFindingMatch()
    {
        gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 1f).OnComplete(()=> {
            gameObject.SetActive(false);
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        });
    }

    
}
