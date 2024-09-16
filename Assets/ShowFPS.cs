using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ShowFPS : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
 
    public Text fpsText;
    public float deltaTime;
    private void Start()
    {
        if (Application.targetFrameRate < 60)
            Application.targetFrameRate = 60;
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        if (fpsText != null) fpsText.text = Mathf.Ceil(fps).ToString();
    }
}



