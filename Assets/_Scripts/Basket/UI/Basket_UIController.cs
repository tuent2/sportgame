using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket_UIController : MonoBehaviour
{
    public static Basket_UIController instance;
    [Header("Camera")]
    public GameObject homeCamera;
    public GameObject gameCamera;
    [Header("Canvas")]
    public Basket_HomeUI home;
    public Basket_FindMatchCanvas findMatchCanvas;
    public Basket_Ingame ingame;
    public Basket_IngameScore ingameScore;
    public Basket_EndGame endgame;
    //public Ba
    private void Awake()
    {
        instance = this;
    }
    

    public void StartFindEnemy()
    {
        findMatchCanvas.gameObject.SetActive(true);
        findMatchCanvas.StartFindingEnemy();
    }
    
}
