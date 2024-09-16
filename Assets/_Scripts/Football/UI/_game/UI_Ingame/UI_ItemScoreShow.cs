using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;
public class UI_ItemScoreShow : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] bool isHomeScore;
    [SerializeField] Image line;
    [SerializeField] Image p1;

    private void Start()
    {

        if (isHomeScore && index== 0)
        {
            SetIsSelected(true);
        } 
        else
        {
            SetIsSelected(false);
        }
    }
    
    public void SetIsSelected(bool isSelected)
    {
        if (!isSelected)
        {
            line.gameObject.SetActive(false);
        }
        else
        {
            line.gameObject.SetActive(true);
        }
    }
    
    [Button("Test")]
    public void SetIsGoal()
    {
        gameObject.transform.DOScale(2f, 0.3f).OnComplete(() => {
            if (GamePhaseController.instance.isGoal)
            {
                p1.color = Color.green;
            }
            else
            {
                p1.color = Color.red;
            }
            line.gameObject.SetActive(false);
            gameObject.transform.DOScale(1f, 0.3f);
        });
       
    }

    
}
