using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterNestSceneState : BaseSceneState
{
    public MonsterNestSceneState(UIManager uiManger) : base(uiManger)
    {
        
    }

    public override void EnterScene()
    {
        uiManager.AddPanelToDict(StringManager.GameLoadPanel);
        uiManager.AddPanelToDict(StringManager.MonsterNestPanel);
        base.EnterScene();
        GameManager.instance.audioManager.
            PlayBGMusic(GameManager.instance.assetManager.audioClipsFactory.GetSingleResources("MonsterNest/BGMusic"));
    }

    public override void ExitScene()
    {
        SceneManager.LoadScene(1);
        base.ExitScene();
    }
}
