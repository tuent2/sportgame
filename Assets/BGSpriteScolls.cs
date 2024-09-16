using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BGSpriteScolls : MonoBehaviour
{
    [SerializeField] Vector2 scollerMoveSpeed;
    Vector2 offset;
    Material material;
    void Awake()
    {
        material = GetComponent<Image>().material;
    }


    void Update()
    {
       
            offset = scollerMoveSpeed * Time.deltaTime;

            material.mainTextureOffset += offset;
        

    }
}
