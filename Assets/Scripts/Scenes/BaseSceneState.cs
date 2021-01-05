using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneState : IBaseSceneState
{
    protected UIManager uiManager;
    public BaseSceneState(UIManager uiManager)
    {
        this.uiManager = uiManager;
    }
    public virtual void EnterScene()
    {
        GameManager.instance.uiManager.InitDict();
    }

    public virtual void ExitScene()
    {
        GameManager.instance.uiManager.ClearDict();
    }
}
