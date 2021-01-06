using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour, IBasePanel
{
    protected UIManager uiManager;

    public virtual void EnterPanel()
    {
        throw new System.NotImplementedException();
    }

    public virtual void ExitPanel()
    {
        throw new System.NotImplementedException();
    }

    public virtual void InitPanel()
    {
        throw new System.NotImplementedException();
    }

    public virtual void UpdatePanel()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void Awake()
    {
        uiManager = GameManager.instance.uiManager;
    }
}
