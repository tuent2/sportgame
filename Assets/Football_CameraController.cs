using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Football_CameraController : MonoBehaviour
{
    public static Football_CameraController intance;

    public Camera _shootCamera;
    public Camera _recordCamera;
    public Camera _goalKeeperCamera;
    private void Awake()
    {
        intance = this;
    }


}
