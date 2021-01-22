using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalModeOptionSceneState : BaseSceneState
{

    public NormalModeOptionSceneState(UIManager uiManger) : base(uiManger)
    {
        
    }

    public override void EnterScene()
    {
        uiManager.AddPanelToDict(StringManager.GameLoadPanel);
        uiManager.AddPanelToDict(StringManager.HelpPanel);
        uiManager.AddPanelToDict(StringManager.LevelSelectionPanel);
        uiManager.AddPanelToDict(StringManager.SecendLevelSelectionPanel);
        base.EnterScene();
    }

    public override void ExitScene()
    {
        if (uiManager.currentSceneState.GetType() == typeof(NormalModeSceneState))
            SceneManager.LoadScene(4);
        else
            SceneManager.LoadScene(1);
        base.ExitScene();
    }
}
