using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerModelControll : MonoBehaviour
{
    public static PlayerModelControll instance;
    public int coinTotal;
   // public CharacterManager characterManager;
    public List<NewSlotData> slotDatasAllItemNotOwer = new List<NewSlotData>();

    public SlotInBodyData allSlotsInBodyData;
    public Home_DataMonstersRemoteState allDataMonstersRemoteState;
    public List<NewSlotData> slotDatasHeadItemOwer = new List<NewSlotData>();
    public List<NewSlotData> slotDatasEyeItemOwer = new List<NewSlotData>();
    public List<NewSlotData> slotDatasMouthItemOwer = new List<NewSlotData>();
    public List<NewSlotData> slotDatasAccItemOwer = new List<NewSlotData>();
    public List<NewSlotData> slotDatasBodyItemOwer = new List<NewSlotData>();
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        allDataMonstersRemoteState.HandleDatasAllWithRemoteConfig();
        GenerateListSlotDataNotOwer();
        GenerateListSlotDataOwer();
    }

    public void AddValueCoin(int coinAdd, float duration = .2f)
    {
        if (coinAdd > 0)
        {
            Debug.Log("Coin > 0");
            //AudioManager.Instance.PlayOneShot(AudioManager.Instance.audioClipData.getCoinAudioClip);
            //Vibration.Vibrate(DataGame.numberPowerVibration);
        }
        float coinTotal = this.coinTotal;
        DOTween.Complete("Add Coin");
        DOTween.To(() => coinTotal, x => coinTotal = x, coinTotal + coinAdd, duration)
            .SetId("Add Coin")
            .SetUpdate(true)
            .OnUpdate(() =>
            {
                //callbackTextCoin?.Invoke((int)coinTotal);
                //SetValueCoin((int)coinTotal);
            })
            .OnComplete(() =>
            {
                //SetValueCoin((int)coinTotal);
            });
    }

    #region SlotData
    public void GenerateListSlotDataNotOwer()
    {
        if (slotDatasAllItemNotOwer.Count == 0)
        {
            slotDatasAllItemNotOwer.AddRange(GetListSlotDataNotOwerPartBody(allDataMonstersRemoteState.hairSlotsSorted));
            slotDatasAllItemNotOwer.AddRange(GetListSlotDataNotOwerPartBody(allDataMonstersRemoteState.eyeSlotsSorted));
            slotDatasAllItemNotOwer.AddRange(GetListSlotDataNotOwerPartBody(allDataMonstersRemoteState.dressSlotsSorted));
            slotDatasAllItemNotOwer.AddRange(GetListSlotDataNotOwerPartBody(allDataMonstersRemoteState.shoeSlotsSorted));
            //slotDatasAllItemNotOwer.AddRange(GetListSlotDataNotOwerPartBody(allDataMonstersRemoteState.bodySlotsSorted));
        }
    }
    public void GenerateListSlotDataOwer()
    {
        if (slotDatasHeadItemOwer.Count == 0)
        {
            slotDatasHeadItemOwer.AddRange(GetListSlotDataOwerPartBody(allDataMonstersRemoteState.hairSlotsSorted));
        }
        if (slotDatasEyeItemOwer.Count == 0)
        {
            slotDatasEyeItemOwer.AddRange(GetListSlotDataOwerPartBody(allDataMonstersRemoteState.eyeSlotsSorted));
        }
        if (slotDatasMouthItemOwer.Count == 0)
        {
            slotDatasMouthItemOwer.AddRange(GetListSlotDataOwerPartBody(allDataMonstersRemoteState.dressSlotsSorted));
        }
        if (slotDatasAccItemOwer.Count == 0)
        {
            slotDatasAccItemOwer.AddRange(GetListSlotDataOwerPartBody(allDataMonstersRemoteState.shoeSlotsSorted));
        }
       
    }
    List<NewSlotData> GetListSlotDataNotOwerPartBody(List<NewSlotData> slotDatasOrigin)
    {
        List<NewSlotData> slotsDataResult = new List<NewSlotData>();
        for (int i = 0; i < slotDatasOrigin.Count; i++)
        {
            if (slotDatasOrigin[i].stateOfNewSlot == StateOfNewSlot.Unlock)
            {
                continue;
            }
            else
            {
                string nameSave = slotDatasOrigin[i].typeOfNewBody.ToString().ToUpper() + "_" + slotDatasOrigin[i].id;
                bool isUnlock = PlayerPrefs.GetInt(nameSave, -1) == 0;
                if (isUnlock)
                {
                    continue;
                }
                else
                {
                    slotsDataResult.Add(slotDatasOrigin[i]);
                }
            }
        }
        return slotsDataResult;
    }
    List<NewSlotData> GetListSlotDataOwerPartBody(List<NewSlotData> slotDatasOrigin)
    {
        List<NewSlotData> slotsDataResult = new List<NewSlotData>();
        for (int i = 0; i < slotDatasOrigin.Count; i++)
        {
            if (slotDatasOrigin[i].stateOfNewSlot == StateOfNewSlot.Unlock)
            {
                slotsDataResult.Add(slotDatasOrigin[i]);
            }
            else
            {
                string nameSave = slotDatasOrigin[i].stateOfNewSlot.ToString().ToUpper() + "_" + slotDatasOrigin[i].id;
                bool isUnlock = PlayerPrefs.GetInt(nameSave, -1) == 0;
                if (isUnlock)
                {
                    slotsDataResult.Add(slotDatasOrigin[i]);
                }
            }
        }
        return slotsDataResult;
    }
    #endregion
}
