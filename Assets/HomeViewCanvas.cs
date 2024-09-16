using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HomeViewCanvas : MonoBehaviour
{
    [SerializeField] Button StarPlayButton;

    public void ClickStarPlayButton()
    {
        SceneManager.LoadScene("_loby");
    }

    
}
