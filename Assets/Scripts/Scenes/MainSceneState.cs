using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneState : BaseSceneState
{

    Dictionary<System.Type, int> sceneMap = new Dictionary<System.Type, int>();
    public MainSceneState(UIManager uiManger) : base(uiManger)
    {
        sceneMap.Add(typeof(NormalModeOptionSceneState), 2);
        sceneMap.Add(typeof(BossModeOptionSceneState), 3);
        sceneMap.Add(typeof(MonsterNestSceneState), 6);
    }
    override public void EnterScene()
    {
        // 加载panel
        uiManager.AddPanelToDict(StringManager.MainPanel);
        uiManager.AddPanelToDict(StringManager.SettingPanel);
        //uiManager.AddPanelToDict(StringManager.HelpPanel);
        //uiManager.AddPanelToDict(StringManager.GameLoadPanel);
        // 加载音乐
        GameManager.instance.audioManager.PlayBGMusic(
            GameManager.instance.assetManager.audioClipsFactory.GetSingleResources("Main/BGMusic"));
        base.EnterScene();
    }

    override public void ExitScene()
    {
        base.ExitScene();
        SceneManager.LoadScene(sceneMap[uiManager.currentSceneState.GetType()]);
    }
}
