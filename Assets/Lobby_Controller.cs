using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Controller : MonoBehaviour
{
    public static Lobby_Controller instance;

    public LobbyPlayerController lobbyPlayer;
    public GameObject footballboth;
    public GameObject basketballboth;
    private void Awake()
    {
        instance = this;
    }

    
}
