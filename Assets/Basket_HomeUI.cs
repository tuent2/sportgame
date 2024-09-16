using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
public class Basket_HomeUI : MonoBehaviour
{
    //public void ClickStartPlay
    [Button("TestAni")]
    public void TestAniCamera()
    {
        Basket_UIController.instance.homeCamera.gameObject.SetActive(false);
        Basket_UIController.instance.gameCamera.gameObject.SetActive(true);
    }
    
    public void ClickBackToLobby()
    {
        SceneManager.LoadScene(Data.scence_lobby);
    }
   
}
