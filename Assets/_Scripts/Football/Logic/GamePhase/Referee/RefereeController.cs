using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefereeController : MonoBehaviour
{
    public static RefereeController instance;

    [SerializeField] Animator RefereeAni;

    private void Awake()
    {
        instance = this;
    }
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
