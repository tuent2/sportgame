using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class Home_TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Home_TabGroup tabGroup;
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;
    public Image Bg;
    public int id;
    [HideInInspector]
    public Image background;

    void Start()
    {
        background = GetComponent<Image>();
        if (tabGroup != null)
            tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        if (onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (onTabDeselected != null)
        {
            onTabSelected.Invoke();
        }

    }

    public void IsSelected(bool isSelected)
    {

        //isSelected == true ? Bg.color = Color.green : Bg.color = Color.white;
        if (isSelected) Bg.color = Color.green;
        else Bg.color = Color.white;
        //Bg.color = Color.green;
        //lineSelected.gameObject.SetActive(isSelected);
    }


}
