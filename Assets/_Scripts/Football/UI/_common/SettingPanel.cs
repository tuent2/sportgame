using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : BaseView
{
    private void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    
}
