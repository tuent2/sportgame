using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class SwipeController : MonoBehaviour, IEndDragHandler
{
   // [SerializeField] Canvas canvas;
    [SerializeField] int maxPage;
    int currentPage;
    Vector3 targetPos;
    Vector3 pageStep;
    [SerializeField] RectTransform levelPageRect;
    [SerializeField] float tweenTime;
    
    float dragThreshold;
    [SerializeField] GameObject[] navButton;
    [SerializeField] GameObject[] levelPage;

    float sizeOfItem;


    private void Awake()
    {
         Debug.Log("screen width" + Screen.width);
         Debug.Log("screen height" + Screen.height);
        currentPage = 2;
        targetPos = new Vector3(levelPageRect.localPosition.x, 0, 0);
        dragThreshold = 1080 / 15;
        // UpdateNavButton();
        pageStep = new Vector3(1080, 0, 0);
        sizeOfItem = Screen.height - 150 ;
        Debug.Log(Screen.height - 150 );
        //  Vector2 size = levelPageRect.sizeDelta;
        //size.y = Screen.height -150 - 213;
        //levelPageRect.sizeDelta = size;

        foreach (GameObject a in levelPage)
        {
            RectTransform  rectTransform = a.GetComponent<RectTransform>();
            Vector2 size = rectTransform.sizeDelta;

            // Thay đổi chiều cao
            size.y = sizeOfItem;

            // Gán kích thước mới
            rectTransform.sizeDelta = size;
        }

        //levelPageRect.localPosition = new Vector3(levelPageRect.localPosition.x, 885, 0);
        //    Debug.Log("levelPageRect.localPosition.y " + levelPageRect.localPosition.y);


        //var canvasRect = canvas.GetComponent<RectTransform>();
        //float widthRatio = CanvasScale.instance.referenceWidth / Screen.width;
        //foreach (GameObject page in levelPage)
        //{
        //    var rectTransform = page.GetComponent<RectTransform>();
        //    var sizeDelta = rectTransform.sizeDelta;
        //    sizeDelta.y = canvasRect.rect.height * widthRatio;
        //    rectTransform.sizeDelta = sizeDelta;
        //}
        OnNavButton(currentPage);
    }
    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos -= pageStep;
            MovePage();
        }
    }
    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos += pageStep;
            MovePage();
        }
    }
    public void MovePage()
    {
        UpdateNavButton();
        levelPageRect.DOLocalMove(targetPos, tweenTime).SetEase(Ease.OutCubic);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshold)
        {
            if (eventData.position.x > eventData.pressPosition.x)
            {
                Previous();
            }
            else
            {
                Next();
            }
        }
        else
        {
            MovePage();
        }
    }

    public void UpdateNavButton()
    {
        for (int i = 0; i < navButton.Length; i++)
        {
            if (i == currentPage - 1)
            {
                navButton[i].transform.Find("Active").gameObject.SetActive(true);
                navButton[i].transform.Find("InActive").gameObject.SetActive(false);
            }
            else
            {
                navButton[i].transform.Find("Active").gameObject.SetActive(false);
                navButton[i].transform.Find("InActive").gameObject.SetActive(true);
            }
        }
    }

    public void OnNavButton(int index)
    {
        currentPage = index;
        targetPos = levelPageRect.localPosition;
        targetPos.x = -pageStep.x * (index - 1) - (pageStep.x / 2);
        MovePage();
    }
    private void Update()
    {
        // levelPageRect.
    }
}
