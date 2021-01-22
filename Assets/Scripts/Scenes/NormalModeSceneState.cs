using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalModeSceneState : BaseSceneState
{

    public NormalModeSceneState(UIManager uiManger) : base(uiManger)
    {
        
    }

    public override void EnterScene()
    {
        uiManager.AddPanelToDict(StringManager.GameLoadPanel);
        uiManager.AddPanelToDict(StringManager.NormalModePanel);
        base.EnterScene();
        GameManager.instance.audioManager.CloseBGMusic();
    }

    public override void ExitScene()
    {
        base.ExitScene();
        GameManager.instance.audioManager.OpenBGMusic();
    }
}
