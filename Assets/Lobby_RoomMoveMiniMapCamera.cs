using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
public class Lobby_RoomMoveMiniMapCamera : MonoBehaviour
{
     Camera cam;

    private bool moveAllowed;
    private Vector3 touchPos;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.touchCount >0)
        {
            if (Input.touchCount == 2)
            {

            }
            else
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase) {
                    case TouchPhase.Began:
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            moveAllowed = false;
                        }
                        else
                        {
                            moveAllowed = true;
                        }
                        touchPos = cam.ScreenToViewportPoint(touch.position);
                        break;
                    case TouchPhase.Moved:
                        if (moveAllowed)
                        {
                            Vector3 direction = touchPos - cam.ScreenToWorldPoint(touch.position);
                            cam.transform.localPosition += direction;
                        }
                        break;
                }
            }
            
        }
    }
}
