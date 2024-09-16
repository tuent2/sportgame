using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket_BallTriggerBottomController : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ballTag"))
        {
            
            if (Basket_GamePlayController.instance.isTouchTopGoal == true)
            {
                
               if (other.gameObject.layer == LayerMask.NameToLayer("ballEnemy"))
                {
                    if (Basket_UIController.instance.ingameScore.isFinalTime)
                    {
                        Basket_UIController.instance.ingameScore.UpdateScore(20, false);
                    }
                    else
                    {
                        Basket_UIController.instance.ingameScore.UpdateScore(10, false);
                    }
                }
               else
                {
                    if (Basket_UIController.instance.ingameScore.isFinalTime)
                    {
                        Basket_UIController.instance.ingameScore.UpdateScore(20,true);
                    }
                    else
                    {
                        Basket_UIController.instance.ingameScore.UpdateScore(10, true);
                    }
                }
               
                //CanvasScoreController.instance.UpdateScore(10);
                hitEffect.Play();
            }
            Basket_GamePlayController.instance.isTouchTopGoal = false;
        }
    }
}
