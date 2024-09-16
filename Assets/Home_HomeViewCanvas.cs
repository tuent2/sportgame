using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Home_HomeViewCanvas : MonoBehaviour
{
    [SerializeField] Button StarPlayButton;

    public void ClickStarPlayButton()
    {
        SceneManager.LoadScene("_loby");
    }

    public void ClickPlayerUI()
    {
        Debug.Log("Click PlayerUI");
        Home_UIController.instance.home_playerUi.gameObject.SetActive(true);
    }

}
