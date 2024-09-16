using UnityEngine;
using Colyseus;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using System.Data;
using NaughtyAttributes;
using System;
using DataChange = Colyseus.Schema.DataChange;
using Colyseus.Schema;
using Newtonsoft.Json.Linq;
using GameDevWare.Serialization;
using System.Threading;

using UnityEditor;
using DG.Tweening;

[Serializable]
class DataSend<T>
{
    public string evt;
    public T users;

    // public T data; 
}
[Serializable]
class playerData
{

    // public string table;
    public string uuid;
    public string name;
    public string level;
    public string exp;
    public string avatar;
    //  public string nationalId;
    // public string isPlaying;
}
//[Serializable]
//class itemPlayerData
//{
//    public string name;
//    public string position;
//    public string rotation;
//    public string scale;
//   // public itemPlayerData;
//}

//[Serializable]
//class itemEnemyData
//{
//    public string name;
//    public Vector3 position;
//    public Vector3 rotation;
//    public Vector3 scale;

//}


public class PlayerDataReceiver : MonoBehaviour
{
    public static PlayerDataReceiver intance;
    private ColyseusClient client;
    private ColyseusRoom<MyRoomState> room;
    playerData type;
    DataSend<playerData> send;
    //private <LobbyState> _lobby;
    private readonly Dictionary<string, playerData> _players = new Dictionary<string, playerData>();
    private readonly ColyseusDecoder Decode = ColyseusDecoder.GetInstance();
    protected ColyseusRoom<IndexedDictionary<string, object>> lobbyRoom;
    //private Room<LobbyState> _lobby;
    //private Room<State> _room;
    //public event Action<string> GamePhaseChanged;
    // public event Action<Dictionary<string, playerData>> playerChanged;
    //DataSend<playerData> data

    public bool isFoundEnemy = false;

    private void Awake()
    {
        intance = this;
    }
    void Start()
    {
        send = new DataSend<playerData>();

        if (LocalStorage.GetUserId() == -1)
        {

            send.evt = "JOIN_LOBBY";
            send.users = new playerData();
            send.users.uuid = null;


        }
        else
        {
            send.evt = "JOIN_LOBBY";
            send.users = new playerData();
            send.users.uuid = LocalStorage.GetUserId().ToString();


        }



        // type.table = "LV1_5";
        // type.uuid = "1";
        //// type.name = "tue";
        // type.exp = "100";
        // type.avatar = "avatar";
        // Debug.Log(type);
        //type.nationalId = "null";
        // type.isPlaying = "false";
        // send = new DataSend<playerData>();


        //Debug.Log(send.data.uuid); // Sẽ in ra "1"
        //Debug.Log(send.data.exp); // Sẽ in ra "100"
        //Debug.Log(send.data.avatar); // Sẽ in ra "avatar"
        //Debug.Log(send.evt); // Sẽ in ra "avatar"

        //  string json = JsonUtility.ToJson(type);
        //  await room.Send( json);


        // wss://hikigame.com/sv-coly
        //client = new ColyseusClient("ws://192.168.1.45:17045");
        //// client = new ColyseusClient("wss://hikigame.com/colyseus");

        //Connect(() =>
        //{
        //    Debug.Log("Success");
        //},
        //() =>
        //{
        //    Debug.Log("Failed");
        //});
    }

    public async void Connect(Action success, Action error)
    {
        ////if (room != null && room.Connect.IsOpen) return;
        ////_client = new Client(endPoint);
        try
        {
            room = await client.JoinOrCreate<MyRoomState>("lobby");
            success?.Invoke();
            RegisterLobbyHandlers();
        }
        catch (Exception)
        {
            error?.Invoke();
        }
    }
    [Button("Get")]
    private void RegisterLobbyHandlers()
    {
        //room.OnMessage<string>("join_lobby", message =>
        //{
        //    Debug.Log(message);
        //});



        room.OnMessage<string>("*", message =>
        {

            handleMessages(message);

            //if (!_players.ContainsKey(player.uuid))
            //    _players.Add(player.uuid, player);
            //type.uuid = player.uuid;





            // RoomsChanged?.Invoke(_players);
        });
        room.OnMessage<string>("user", message =>
        {
            Debug.Log(message);
        });
        room.OnMessage<string>("welcomeMessage", message =>
        {
            Debug.Log(message);
        });

        room.OnMessage<string>("", message =>
        {
            Debug.Log(message);
        });
        room.OnMessage<string>("MESSAGE", message =>
        {
            Debug.Log(message);
        });
    }

    public void handleMessages(string message)
    {

       // string a = ConvertData(message);

        send = JsonUtility.FromJson<DataSend<playerData>>(message);

       // if (send.evt == "JOIN_LOBBY")
       // {
            
       //     isFoundEnemy = false;
       // }
       // else if (send.evt == "CREATE_TABLE")
       // {
            
       //     DOVirtual.DelayedCall(3f, CheckRoom);
       // }
       // else if (send.evt == "READY_TABLE")
       // {
       //     isFoundEnemy = true;
       // }
       // else if (send.evt == "SEND_COLLECTOR_DATA")
       // {
       //     Debug.Log(send.users);
       // }

       // //Debug.Log(send);
       // Debug.Log(send.evt+ ", "+ send.users + ", "+ send.users.uuid + ", "+ send.users.name + ", "+ send.users.level + ", "+ send.users.avatar + ", "+ send.users.avatar+", "+send.users.exp);
        //Debug.Log(send.users);
        //Debug.Log(send.users.uuid);
        //Debug.Log(send.users.name);
        //Debug.Log(send.users.level);
        //Debug.Log(send.users.avatar);
        //Debug.Log(send.users.exp);
    }


    //public void CheckRoom()
    //{
    //    if (isFoundEnemy)
    //    {
    //        Debug.Log("PlayOnline");
    //        Basket_GamePlayController.instance.FindedEnemy();
    //    }
    //    else
    //    {
    //        Debug.Log("PlayOnline");
    //        Basket_GamePlayController.instance.FindedEnemy();
            
    //    }
    //}
    [Button("Send")]
    public async Task SendTestAsync()
    {
        Debug.Log("--------------------");

        string json = JsonUtility.ToJson(send);

        await room.Send(json);
    }

    [Button("Send2")]
    public async Task SendNewUUidAsync()
    {
        Debug.Log("--------------------");

        room = await client.JoinOrCreate<MyRoomState>("basketball", new Dictionary<string, object>
            {
                { "uuid", send.users.uuid },
                { "level", send.users.level }
            });
        room.OnMessage<string>("*", message =>
        {
            handleMessages(message);
        });
    }



    //public object ConvertData(string data)
    //{
    //    string[] newarr = data.Split('&');
    //    List<string> new2Arr = new List<string>();

    //    foreach (string item in newarr)
    //    {
    //        new2Arr.Add(Convert.ToChar(Convert.ToInt32(item) - 12).ToString());
    //    }

    //    new2Arr.Reverse();
    //    return Newtonsoft.Json.JsonConvert.DeserializeObject(string.Join("", new2Arr));
    //}





    public void SendItemData(string name, string position, string rotation, string scale)
    {
        //itemPlayerData item = new itemPlayerData();
        //item.name = name;
        //item.position = position;
        //item.rotation = rotation;
        //item.scale = scale;
        //string json = JsonUtility.ToJson(item);
        //Debug.Log(json);
        //_ = room.Send(json);
    }

    //public string GetEnemyItemData(string jsonString)
    //{
    //    //var jsonData = jsonString;
    //    //// Parse JSON string to itemPlayerData object
    //    //itemEnemyData item = JsonUtility.FromJson<itemEnemyData>(jsonData);

    //    //var data = "enemy data" + "\n" + "itemName: " + item.name + "\n" + "itemPos: " + item.position + "\n" + "ItemRotate: " + item.rotation + "\n" + "itemScale" + item.scale;
    //    //Debug.Log(data);
    //    //return data;
    //}

}


