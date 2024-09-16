using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_PlayerCameraFollow : MonoBehaviour
{
    public Transform player; // Transform của người chơi
    private Vector3 offset;  // Khoảng cách giữa camera và người chơi

    void Start()
    {
        offset = transform.localPosition - player.localPosition;
    }

    void LateUpdate()
    {
        // Cập nhật vị trí của camera theo người chơi
        if (player != null)
        {
            transform.localPosition = player.localPosition + offset;
        }
    }
}
