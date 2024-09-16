using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Home_DataMonstersRemoteState : MonoBehaviour
{
    //[ConditionalHide] public List<NewSlotData> defaultSlotsSorted = new List<NewSlotData>();
    [ConditionalHide] public List<NewSlotData> hairSlotsSorted = new List<NewSlotData>();
    [ConditionalHide] public List<NewSlotData> eyeSlotsSorted = new List<NewSlotData>();
    [ConditionalHide] public List<NewSlotData> dressSlotsSorted = new List<NewSlotData>();
    [ConditionalHide] public List<NewSlotData> shoeSlotsSorted = new List<NewSlotData>();
    
    public void HandleDatasAllWithRemoteConfig()
    {
        HandleDataWithRemoteConfig("1", PlayerModelControll.instance.allSlotsInBodyData.hairSlotsSorted, ref hairSlotsSorted);
        HandleDataWithRemoteConfig("2", PlayerModelControll.instance.allSlotsInBodyData.eyeSlotsSorted, ref eyeSlotsSorted);
        HandleDataWithRemoteConfig("3", PlayerModelControll.instance.allSlotsInBodyData.dressSlotsSorted, ref dressSlotsSorted);
        HandleDataWithRemoteConfig("4", PlayerModelControll.instance.allSlotsInBodyData.shoeSlotsSorted, ref shoeSlotsSorted);
        
    }
    void HandleDataWithRemoteConfig(string idPartsHide_RemoteConfig, List<NewSlotData> slotDatasListImport, ref List<NewSlotData> slotDatasListResult)
    {
        try
        {
            string idPartsHide_FBRemote = PlayerPrefs.GetString(idPartsHide_RemoteConfig, "");
            string[] idPartsHide = idPartsHide_FBRemote.Split(',');

            var idPartsHideList = idPartsHide.ToList();

            //Debug.Log(slotDatasListImport.Count);
            for (int i = 0; i < slotDatasListImport.Count; i++)
            {
                bool isSlotHided = false;
                for (int j = 0; j < idPartsHideList.Count; j++)
                {
                    if (!int.TryParse(idPartsHideList[j], out int id))
                    {
                        id = 0;
                    }
                    if (slotDatasListImport[i].id == id)
                    {
                        idPartsHideList.RemoveAt(j);
                        isSlotHided = true;
                        break;
                    }
                }
                if (!isSlotHided)
                {
                    slotDatasListResult.Add(slotDatasListImport[i]);
                }
            }
        }
        catch (System.Exception)
        {
            slotDatasListResult = new List<NewSlotData>(slotDatasListImport);
            Debug.Log("<color=orange>Remote Config sai" + slotDatasListImport[0].typeOfNewBody.ToString() + "</color>");
        }
    }

    public List<NewSlotData> GetSlotDatas(TypeOfNewBody typeOfBody)
    {
        List<NewSlotData> newSlotDatas = new List<NewSlotData>();
        switch (typeOfBody)
        {
            case TypeOfNewBody.Head:
                newSlotDatas = hairSlotsSorted;
                break;
            case TypeOfNewBody.Hair:
                newSlotDatas = eyeSlotsSorted;
                break;
            case TypeOfNewBody.Skin:
                newSlotDatas = dressSlotsSorted;
                break;
            case TypeOfNewBody.Custom:
                newSlotDatas = shoeSlotsSorted;
                break;
            default:
                break;
        }
        return newSlotDatas;
    }
    public NewSlotData GetSlotDataById(int id, TypeOfNewBody typeOfNewBody)
    {
        var slotNewDatas = GetSlotDatas(typeOfNewBody);
        NewSlotData slotNewData = slotNewDatas.Find(x => x.id == id);
        return slotNewData;
    }
}
