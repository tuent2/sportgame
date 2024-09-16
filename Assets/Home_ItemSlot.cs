using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class Home_ItemSlot : MonoBehaviour
{
    [SerializeField] Image LockImage;
    [SerializeField] Image FocusImage;
    [SerializeField] Image IconImage;
    [SerializeField] GameObject LockGold;
    [SerializeField] TextMeshProUGUI LockCoinText;
    [SerializeField] GameObject LockAds;
    [SerializeField] Button button;
    public NewSlotData newSlotData;
    StateOfNewSlot stateOfNewSlot;
    UnityAction<NewSlotData> callbackClick;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, .2f);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
        DOTween.Kill(transform);
        DOTween.Complete(gameObject.GetInstanceID() + "UnlockItem");
        // Debug.Log(gameObject.GetInstanceID() + "UnlockItem");
        UnPickItem();
        //if (tapItemAP)
        //{
        //    tapItemAP.Stop();
        //}
    }

    void Start()
    {
        button.onClick.AddListener(() =>
        {
            //AudioManager.Instance.PlayOneShot(AudioManager.Instance.audioClipData.tapButtonAudioClip);
            //GameManager.Instance.gameplayUI.AbortItemsEarnCoin();
            string nameSave = newSlotData.typeOfNewBody.ToString().ToUpper() + "_" + newSlotData.id;
            bool isUnlock = PlayerPrefs.GetInt(nameSave, -1) == 0;
            if (isUnlock)
            {
                //if (tapItemAP)
                //{
                //    tapItemAP.Stop();
                //}
                //tapItemAP = AudioManager.Instance.PlayOneShotReturnRef(AudioManager.Instance.audioClipData.tapItemsAudioClip[UnityEngine.Random.Range(0, AudioManager.Instance.audioClipData.tapItemsAudioClip.Length)], callback: () =>
                //{
                //    tapItemAP = null;
                //});
                //GameManager.Instance.ShowInterTapEveryWhere();

                callbackClick.Invoke(newSlotData);
            }
            else
            {
                SetStateUpdate();
            }
        });
    }

    public void SetStateUpdate()
    {
        //AudioManager.Instance.PlayOneShot(AudioManager.Instance.audioClipData.tapButtonAudioClip);
        string nameSave = newSlotData.typeOfNewBody.ToString().ToUpper() + "_" + newSlotData.id;
        switch (stateOfNewSlot)
        {
            case StateOfNewSlot.Ads:
                //Ads
                // FirebasePushEvent.intance.LogEvent(string.Format(DataGame.fbADS_REWARD_CLICK_xxx, slotData.name));
                Debug.Log("Show Ads");
               // PlayerPrefs.SetInt(nameSave, (int)StateOfNewSlot.Unlock);
                //PlayerPrefs.Save();
                UnlockItem();
                //AdsIronSourceMediation.Instance.ShowRewardedAd((bool isWatched) =>
                //{
                //    if (isWatched)
                //    {
                //        FirebasePushEvent.intance.LogEvent(string.Format(DataGame.fbADS_REWARD_COMPLETED_xxx, slotData.name));
                //        PlayerPrefs.SetInt(nameSave, (int)StateOfSlot.Unlock);
                //        PlayerPrefs.Save();

                //        UnlockItem();
                //    }
                //});
                break;
            case StateOfNewSlot.Gold:
                //Gold
                //if (GameManager.THIS.coinTotal >= newSlotData.priceGold)
                //{
                //    //FirebasePushEvent.intance.LogEvent(string.Format(DataGame.fbBUY_NAMEITEM, slotData.name.ToString()));
                //    GameManager.THIS.AddValueCoin(-newSlotData.priceGold);
                //    PlayerPrefs.SetInt(nameSave, (int)StateOfSlot.Unlock);
                //    PlayerPrefs.Save();

                //    UnlockItem();
                //}
                //else
                //{
                //    Debug.Log("Need More Coin");
                //    //GameManager.Instance.notificationPopup.ShowWithText("Need more coins!");
                //    //earnGoldObject.SetActive(true);
                //    //GameManager.Instance.earnCoinsPopup.Show();
                //}
                break;
            default:
                break;
        }
    }

    public void UnlockItem(bool isSelected = true)
    {
        // AudioManager.Instance.PlayOneShot(AudioManager.Instance.audioClipData.itemUnlockAudioClip);
        //Vibration.Vibrate(DataGame.numberPowerVibration);

        //NewGameManager.THIS.slotDatasAllItemNotOwer.Remove(newSlotData);
        //switch (newSlotData.typeOfNewBody)
        //{
        //    case TypeOfNewBody.Default:
        //        NewGameManager.THIS.slotDatasDefaultItemOwer.Add(newSlotData);
        //        break;
        //    case TypeOfNewBody.Hair:
        //        NewGameManager.THIS.slotDatasHairItemOwer.Add(newSlotData);
        //        break;
        //    case TypeOfNewBody.Eye:
        //        NewGameManager.THIS.slotDatasEyeItemOwer.Add(newSlotData);
        //        break;
        //    case TypeOfNewBody.Dress:
        //        NewGameManager.THIS.slotDatasDressItemOwer.Add(newSlotData);
        //        break;
        //    case TypeOfNewBody.Shoe:
        //        NewGameManager.THIS.slotDatasShoeItemOwer.Add(newSlotData);
        //        break;
        //    default:
        //        break;
        //}
        //LockImage.gameObject.SetActive(false);
        //FocusImage.gameObject.SetActive(false);
        //LockGold.SetActive(false);
        //LockAds.SetActive(false);
        //// UnlockItem.ga.SetActive(true); them vao cho dep
        ////earnGoldObject.SetActive(false);
        ////DOTween.Sequence()
        ////    .SetId(gameObject.GetInstanceID() + "UnlockItem")
        ////    .AppendInterval(2)
        ////    .AppendCallback(() =>
        ////    {
        ////        unlockObject.SetActive(false);
        ////    });
        //if (isSelected)
        //{
        //    callbackClick?.Invoke(newSlotData);
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UnPickItem()
    {
        FocusImage.gameObject.SetActive(false);
        transform.GetChild(0).transform.localScale = Vector3.one;
        DOTween.Kill(gameObject.GetInstanceID() + "PickItem");
    }

    public void PickItem()
    {
        LockImage.gameObject.SetActive(false);
        FocusImage.gameObject.SetActive(true);
        LockGold.SetActive(false);
        LockAds.SetActive(false);
        DOTween.Sequence()
            .SetId(gameObject.GetInstanceID() + "PickItem")
            .Append(transform.GetChild(0).transform.DOScale(1.05f, .5f).SetEase(Ease.Linear))
            .Append(transform.GetChild(0).transform.DOScale(1f, .5f).SetEase(Ease.Linear))
            .SetLoops(-1, LoopType.Yoyo);
        //unlockObject.SetActive(true);
    }

    public void SetData(NewSlotData newSlotData, UnityAction<NewSlotData> callbackClick)
    {
        this.newSlotData = newSlotData;
        this.callbackClick = callbackClick;
        //SetInteractive(false);
        IconImage.sprite = newSlotData.sprite;
        LockCoinText.text = newSlotData.priceGold.ToString();
        string nameSave = newSlotData.typeOfNewBody.ToString().ToUpper() + "_" + newSlotData.id;
        bool isUnlock = PlayerPrefs.GetInt(nameSave, -1) == 0;
        if (isUnlock)
        {
            stateOfNewSlot = StateOfNewSlot.Unlock;
            SetStateFirst();

            return;
        }
        stateOfNewSlot = newSlotData.stateOfNewSlot;

        if (stateOfNewSlot == StateOfNewSlot.Unlock && PlayerPrefs.HasKey(nameSave) == false)
        {
            PlayerPrefs.SetInt(nameSave, (int)StateOfNewSlot.Unlock);
            PlayerPrefs.Save();
        }
        SetStateFirst();
    }

    public void SetStateFirst()
    {
        switch (stateOfNewSlot)
        {
            case StateOfNewSlot.Unlock:
                LockImage.gameObject.SetActive(false);
                FocusImage.gameObject.SetActive(false);
                LockGold.SetActive(false);
                LockAds.SetActive(false);
                //unlockObject.SetActive(false);
                //earnGoldObject.SetActive(false);
                break;
            case StateOfNewSlot.Ads:
                LockImage.gameObject.SetActive(true);
                FocusImage.gameObject.SetActive(false);
                LockGold.SetActive(false);
                LockAds.SetActive(true);
                //unlockObject.SetActive(false);
                //earnGoldObject.SetActive(false);
                break;
            case StateOfNewSlot.Gold:
                LockImage.gameObject.SetActive(true);
                FocusImage.gameObject.SetActive(false);
                LockGold.SetActive(true);
                LockAds.SetActive(false);
                //unlockObject.SetActive(false);
                //earnGoldObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
