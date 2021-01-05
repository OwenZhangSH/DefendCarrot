using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoadSceneState : BaseSceneState
{
    public StartLoadSceneState(UIManager uiManger) : base(uiManger)
    {

    }
    override public void EnterScene()
    {
        uiManager.AddPanelToDict(StringManager.StartLoadPanel);
        base.EnterScene();
    }

    override public void ExitScene()
    {
        base.ExitScene();
        SceneManager.LoadScene(1);
    }
}
