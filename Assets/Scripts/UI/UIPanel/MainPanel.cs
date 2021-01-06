using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
        // 初始化变量
    }

    public override void EnterPanel()
    {
        
    }

    public override void ExitPanel()
    {
        
    }

    // 退出游戏回调函数
    public void ExitGame()
    {
        //uiManager.PlayButtonAudioClip();
        GameManager.instance.SaveData();
        Application.Quit();
    }

    public void ToNormalModeScene()
    {
        //uiManager.PlayButtonAudioClip();
        uiManager.currentScenePanelDict[StringManager.GameLoadPanel].GetComponent<IBasePanel>().EnterPanel();
        uiManager.ChangeSceneState(new NormalModeOptionSceneState(uiManager));
    }

    public void ToBossModeScene()
    {
        //uiManager.PlayButtonAudioClip();
        uiManager.currentScenePanelDict[StringManager.GameLoadPanel].GetComponent<IBasePanel>().EnterPanel();
        uiManager.ChangeSceneState(new BossModeOptionSceneState(uiManager));
    }

    public void ToMonsterNestScene()
    {
        //uiManager.PlayButtonAudioClip();
        uiManager.currentScenePanelDict[StringManager.GameLoadPanel].GetComponent<IBasePanel>().EnterPanel();
        uiManager.ChangeSceneState(new MonsterNestSceneState(uiManager));
    }
}
