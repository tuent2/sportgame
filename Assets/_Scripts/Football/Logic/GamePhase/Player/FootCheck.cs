using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("1234");
        if (other.CompareTag("ballTag"))
        {
           // Debug.Log("123");
            if (Globals.game_mode == Globals.GAME_MODE.Penalty)
            {
                //Debug.Log("12");

                Football_BallController.instance.BallShotKick();
               
            }
            
        }
    }

}
