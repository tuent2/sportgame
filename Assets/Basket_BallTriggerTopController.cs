using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Basket_BallTriggerTopController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ballTag"))
        {
            Basket_GamePlayController.instance.isTouchTopGoal= true;
        }
    }
}
