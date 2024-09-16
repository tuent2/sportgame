using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelUiController : MonoBehaviour
{
    [SerializeField] Animator Animator;
    [SerializeField] SkinnedMeshRenderer Skin;
    
    [SerializeField] 
    void Start()
    {
        Animator.Play("Idle");
        
    }

    public void ShowGetReadyHome()
    {
        Animator.Play("ReadyHome");
    }

    public void ShowGetReadyAway()
    {
        Animator.Play("ReadyAway");
    }
}
