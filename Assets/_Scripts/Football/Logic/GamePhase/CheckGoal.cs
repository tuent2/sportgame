using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ballTag"))
        {
            Time.timeScale = 1f;
            SoundController.instance?.PlayGoalClip();
            GamePhaseController.instance.isGoal = true;
           
            GamePhaseController.instance.isRecording = !GamePhaseController.instance.isRecording;
            
        }
    }
}
