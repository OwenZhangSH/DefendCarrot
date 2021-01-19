using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainPanel : BasePanel
{
    // 萝卜动画
    private Animator carrotAnim;
    private Transform monsterTransform;
    private Transform cloudTransform;

    // 主页离开动画
    private Tween exitTween;
    private Tween[] mainPanelTweens;

    protected override void Awake()
    {
        base.Awake();
        transform.SetSiblingIndex(8);
        // 初始化变量
        monsterTransform = transform.Find("Img_Monster").transform;
        cloudTransform = transform.Find("Img_Cloud").transform;

        // 初始化移动动画
        mainPanelTweens = new Tween[2];
        mainPanelTweens[0] = transform.DOLocalMoveX(1920, 0.5f)
            .SetAutoKill(false).Pause();
        mainPanelTweens[1] = transform.DOLocalMoveX(-1920, 0.5f)
            .SetAutoKill(false).Pause();
        // 初始化Animator
        carrotAnim = transform.Find("Carrot").GetComponent<Animator>();
        carrotAnim.Play("CarrotGrow");
        // 播放动画
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        monsterTransform.DOLocalMoveY(600, 1.5f).SetLoops(-1, LoopType.Yoyo);
        cloudTransform.DOLocalMoveX(1300, 8f).SetLoops(-1, LoopType.Restart);
    }

    public override void EnterPanel()
    {
        transform.SetSiblingIndex(8);
        carrotAnim.Play("CarrotGrow");
        if (exitTween != null) exitTween.PlayBackwards();
        cloudTransform.gameObject.SetActive(true);
    }

    public override void ExitPanel()
    {
        exitTween.PlayForward();
        cloudTransform.gameObject.SetActive(false);
    }

    // 进入setting panel
    public void EnterSettingPanel()
    {
        //播放点击音效
        uiManager.PlayButtonAudioClip();
        exitTween = mainPanelTweens[0];
        uiManager.currentScenePanelDict[StringManager.SettingPanel].GetComponent<IBasePanel>().EnterPanel();
    }
    // 进入 help panel
    public void EnterHelpPanel()
    {
        //播放点击音效
        uiManager.PlayButtonAudioClip();
        exitTween = mainPanelTweens[1];
        uiManager.currentScenePanelDict[StringManager.HelpPanel].GetComponent<IBasePanel>().EnterPanel();
    }
    // 退出游戏回调函数
    public void ExitGame()
    {
        uiManager.PlayButtonAudioClip();
        GameManager.instance.SaveData();
        Application.Quit();
    }

    public void ToNormalModeScene()
    {
        uiManager.PlayButtonAudioClip();
        uiManager.currentScenePanelDict[StringManager.GameLoadPanel].GetComponent<IBasePanel>().EnterPanel();
        uiManager.ChangeSceneState(new NormalModeOptionSceneState(uiManager));
    }

    public void ToBossModeScene()
    {
        uiManager.PlayButtonAudioClip();
        uiManager.currentScenePanelDict[StringManager.GameLoadPanel].GetComponent<IBasePanel>().EnterPanel();
        uiManager.ChangeSceneState(new BossModeOptionSceneState(uiManager));
    }

    public void ToMonsterNestScene()
    {
        uiManager.PlayButtonAudioClip();
        uiManager.currentScenePanelDict[StringManager.GameLoadPanel].GetComponent<IBasePanel>().EnterPanel();
        uiManager.ChangeSceneState(new MonsterNestSceneState(uiManager));
    }
}
