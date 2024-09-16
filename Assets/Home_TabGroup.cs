using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Home_TabGroup : MonoBehaviour
{
    // These must be 1 to 1, same order in hierarchy
    [HideInInspector]
    public List<Home_TabButton> tabButtons = new List<Home_TabButton>();
    //public List<GameObject> tabPages = new List<GameObject>();
    //public GameObject tabPages;
    //In case I need to sort the lists by GetSiblingIndex
    //objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));

    //public Color tabIdleColor;
    //public Color tabHoverColor;
    //public Color tabSelectedColor;
    /*private*/
    public Home_TabButton selectedTab;
    public TypeOfNewBody typeOfNewBody;
    //  [SerializeField] RectTransform MainContent;
    public void Start()
    {
        // Select first tab
        

            OnTabSelected(selectedTab);
            UpdateDataItemOfSlot();
            // OnTabSelected(tabButton);
        
    }

    private void OnEnable()
    {
        //foreach (Home_TabButton tabButton in tabButtons)
        //{
        //    if (tabButton.transform.GetSiblingIndex() == 0)
        //    {
        //        OnTabSelected(tabButton);
        //        UpdateDataItemOfSlot();
        //    }
        //    // OnTabSelected(tabButton);
        //}
    }

    public void Subscribe(Home_TabButton tabButton)
    {
        tabButtons.Add(tabButton);
        // Sort by order in hierarchy
        tabButtons.Sort((x, y) => x.transform.GetSiblingIndex().CompareTo(y.transform.GetSiblingIndex()));
    }

    public void OnTabEnter(Home_TabButton tabButton)
    {
        ResetTabs();
        //if ((selectedTab == null) || (tabButton != selectedTab))
        //tabButton.IsSelected(true);
    }

    public void OnTabExit(Home_TabButton tabButton)
    {
        ResetTabs();
    }

    public void OnTabSelected(Home_TabButton tabButton)
    {
        if (selectedTab != null && selectedTab.id == tabButton.id)
        {

            //MainContent.DOAnchorPosY(-508f, 0.5f);
            //selectedTab = null;
            //Debug.Log("TurnOf");
            return;
        }

        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
        //if (MainContent != null)
        //{
        //   // MainContent.DOAnchorPosY(-508f, 0f);
        //    //MainContent.DOAnchorPosY(720f, 0.5f);
        //}
        //MainContent.DOAnchorPosY(-508f, 0f);
        //MainContent.DOAnchorPosY(720f, 0.5f);
        selectedTab = tabButton;

        selectedTab.Select();

        ResetTabs();
        tabButton.IsSelected(true);
        int index = tabButton.transform.GetSiblingIndex();
        typeOfNewBody = (TypeOfNewBody)index;
        //for (int i = 0; i < tabPages.Count; i++)
        //{
        //    if (i == index)
        //    {
        //        tabPages[i].SetActive(true);
        //    }
        //    else
        //    {
        //        tabPages[i].SetActive(false);
        //    }
        //}

        UpdateDataItemOfSlot();
        // TabAnimation.Play();
    }

    public void ResetTabs()
    {
        foreach (Home_TabButton tabButton in tabButtons)
        {
            if ((selectedTab != null) && (tabButton == selectedTab))
                continue;
            tabButton.IsSelected(false);
        }
    }
    //[ConditionalHide] public TypeOfNewBody typeOfNewBody;
    [SerializeField] Home_ItemSlot newItemOfSlotPrefab;
    [SerializeField] Sprite noneItemOfSlotSprite;
    List<Home_ItemSlot> newItemOfSlotsPooling = new List<Home_ItemSlot>();
    List<Home_ItemSlot> newItemOfSlotsCurrent = new List<Home_ItemSlot>();
    Dictionary<TypeOfNewBody, int> stepDictionary = new Dictionary<TypeOfNewBody, int>();
    Home_ItemSlot newItemOfSlotSave;
    public void UpdateDataItemOfSlot()
    {
        if (handleUpdateDataItemOfSlotCoroutine != null)
        {
            StopCoroutine(handleUpdateDataItemOfSlotCoroutine);
        }
        handleUpdateDataItemOfSlotCoroutine = StartCoroutine(IEnumHandleUpdateDataItemOfSlot());
    }
    Coroutine handleUpdateDataItemOfSlotCoroutine;
    IEnumerator IEnumHandleUpdateDataItemOfSlot()
    {
        //itemOfSlotShowing.Clear();
        Home_ItemSlot newItemOfSlot;
        for (int i = newItemOfSlotsCurrent.Count - 1; i >= 0; i--)
        {
            newItemOfSlot = newItemOfSlotsCurrent[i];
            newItemOfSlot.gameObject.SetActive(false);
            newItemOfSlotsPooling.Add(newItemOfSlot);
            newItemOfSlotsCurrent.RemoveAt(i);
        }
        yield return new WaitForSeconds(.01f);
        NewSlotData newSlotData;
        newItemOfSlotSave = null;
        if (typeOfNewBody != TypeOfNewBody.Head && typeOfNewBody != TypeOfNewBody.Custom)
        {
            newSlotData = new NewSlotData();
            newSlotData.typeOfNewBody = typeOfNewBody;
            newSlotData.sprite = noneItemOfSlotSprite;
            Home_ItemSlot newItemOfSlotNone = GetItemOfSlot();
            newItemOfSlotNone.SetData(newSlotData, delegate (NewSlotData newSlotDataCallback)
            {
                if (newItemOfSlotSave == null || newSlotDataCallback.id != newItemOfSlotSave.newSlotData.id)
                {
                    //GameManager.Instance.characterManager.ResetDirect();

                    if (newItemOfSlotSave)
                        newItemOfSlotSave.UnPickItem();
                    newItemOfSlotSave = newItemOfSlotNone;
                }
                newItemOfSlotNone.PickItem();
               // NewGameManager.THIS.characterManager.StepEdit(newSlotDataCallback);
                //if (newItemOfSlotNone.newSlotData.typeOfNewBody != TypeOfNewBody.Shoe)
                //{
                //    if (nextButton.gameObject.activeSelf == false)
                //    {
                //        nextButton.gameObject.SetActive(true);
                //    }
                //    if (ChangeCollorButton.gameObject.activeSelf == false)
                //    {
                //        ChangeCollorButton.gameObject.SetActive(true);
                //    }
                //}
                //else
                //{
                //    if (completeButton.gameObject.activeSelf == false)
                //    {
                //        completeButton.gameObject.SetActive(true);
                //    }
                //}

                if (stepDictionary.ContainsKey(typeOfNewBody) == false)
                {
                    stepDictionary.Add(typeOfNewBody, newItemOfSlotSave.newSlotData.id);
                }
                else
                {
                    stepDictionary[typeOfNewBody] = newItemOfSlotSave.newSlotData.id;
                }
            });
            if (newItemOfSlotSave == null && stepDictionary.TryGetValue(typeOfNewBody, out int indexItemSave))

            {
                newItemOfSlotSave = newItemOfSlotNone.newSlotData.id == indexItemSave ? newItemOfSlotNone : null;
                if (newItemOfSlotSave)
                {
                    //itemOfSlotSave.slotData = itemOfSlotNone.slotData;
                    newItemOfSlotSave.PickItem();
                }
            }
            //itemOfSlotShowing.Add(itemOfSlotNone);
            yield return new WaitForSeconds(.02f);
        }

        List<NewSlotData> slotDataTypeCurrent = PlayerModelControll.instance.allDataMonstersRemoteState.GetSlotDatas(typeOfNewBody);
        for (int i = 0; i < slotDataTypeCurrent.Count; i++)
        {
            newSlotData = slotDataTypeCurrent[i];
            Home_ItemSlot itemOfSlotSelect = GetItemOfSlot();
            itemOfSlotSelect.SetData(newSlotData, delegate (NewSlotData newSlotDataCallback)
            {
                if (newItemOfSlotSave == null || newSlotDataCallback.id != newItemOfSlotSave.newSlotData.id)
                {
                    // NewGameManager.THIS.characterManager.ResetDirect();

                    if (newItemOfSlotSave)
                        newItemOfSlotSave.UnPickItem();
                    newItemOfSlotSave = itemOfSlotSelect;
                }
                itemOfSlotSelect.PickItem();
               // NewGameManager.THIS.characterManager.StepEdit(newSlotDataCallback);

                //if (itemOfSlotSelect.newSlotData.typeOfNewBody != TypeOfNewBody.Shoe)
                //{
                //    if (nextButton.gameObject.activeSelf == false)
                //    {
                //        nextButton.gameObject.SetActive(true);
                //    }
                //    if (ChangeCollorButton.gameObject.activeSelf == false)
                //    {
                //        ChangeCollorButton.gameObject.SetActive(true);
                //    }
                //}
                //else
                //{
                //    if (completeButton.gameObject.activeSelf == false)
                //    {
                //        completeButton.gameObject.SetActive(true);
                //    }
                //}

                if (stepDictionary.ContainsKey(typeOfNewBody) == false)
                {
                    stepDictionary.Add(typeOfNewBody, newItemOfSlotSave.newSlotData.id);
                }
                else
                {
                    stepDictionary[typeOfNewBody] = newItemOfSlotSave.newSlotData.id;
                }
            });
            if (newItemOfSlotSave == null && stepDictionary.TryGetValue(typeOfNewBody, out int indexItemSave))

            {
                newItemOfSlotSave = itemOfSlotSelect.newSlotData.id == indexItemSave ? itemOfSlotSelect : null;
                if (newItemOfSlotSave)
                {
                    //itemOfSlotSave.slotData = itemOfSlotSelect.slotData;
                    newItemOfSlotSave.PickItem();
                }
            }
            //itemOfSlotShowing.Add(itemOfSlotSelect);

            yield return new WaitForSeconds(.02f);
        }

        //for (int i = 0; i < itemOfSlotShowing.Count; i++)
        //{
        //    itemOfSlotShowing[i].SetInteractive(true);
        //}

    }

    Home_ItemSlot GetItemOfSlot()
    {
        Home_ItemSlot newItemOfSlot = null;
        for (int i = newItemOfSlotsPooling.Count - 1; i >= 0; i--)
        {
            if (newItemOfSlotsPooling[i].gameObject.activeSelf == false)
            {
                newItemOfSlot = newItemOfSlotsPooling[i];
                newItemOfSlotsPooling.RemoveAt(i);
                break;
            }
        }
        if (!newItemOfSlot)
        {
            if (newItemOfSlotPrefab.gameObject.activeSelf)
            {
                newItemOfSlotPrefab.gameObject.SetActive(false);
            }
            newItemOfSlot = Instantiate(newItemOfSlotPrefab, newItemOfSlotPrefab.transform.parent);
        }
        newItemOfSlotsCurrent.Add(newItemOfSlot);

        newItemOfSlot.gameObject.SetActive(true);
        return newItemOfSlot;
    }
}
