﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour, IBasePanel
{
    protected UIManager uiManager;

    public virtual void EnterPanel()
    {
        
    }

    public virtual void ExitPanel()
    {
        
    }

    public virtual void InitPanel()
    {
        
    }

    public virtual void UpdatePanel()
    {
        
    }

    protected virtual void Awake()
    {
        uiManager = GameManager.instance.uiManager;
    }
}
