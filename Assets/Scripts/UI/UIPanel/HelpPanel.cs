﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HelpPanel : BasePanel
{
    // 切换展示的GO
    private GameObject helpPageGo;
    private GameObject monsterPageGo;
    private GameObject towerPageGo;

    // 主页进入动画
    private Tween enterTween;

    protected override void Awake()
    {
        base.Awake();
        // 初始化变量
        helpPageGo = transform.Find("HelpPage").gameObject;
        monsterPageGo = transform.Find("MonsterPage").gameObject;
        towerPageGo = transform.Find("TowerPage").gameObject;
        // 初始化动画
        enterTween = transform.DOLocalMoveX(0, 0.5f).SetAutoKill(false).Pause();
    }

    public override void InitPanel()
    {
        transform.localPosition = new Vector3(1920, 0, 0);
        transform.SetSiblingIndex(5);
    }

    public override void EnterPanel()
    {
        ShowHelpPage();
        MoveToCenter();
    }

    public override void ExitPanel()
    {
        uiManager.PlayButtonAudioClip();
        enterTween.PlayBackwards();
        uiManager.currentScenePanelDict[StringManager.MainPanel].GetComponent<BasePanel>().EnterPanel();
        InitPanel();
    }

    public void MoveToCenter()
    {
        enterTween.PlayForward();
    }

    // 显示页面
    
    public void ShowHelpPage()
    {
        if (!helpPageGo.activeSelf)
        {
            uiManager.PlayButtonAudioClip();
            helpPageGo.SetActive(true);
        }
        monsterPageGo.SetActive(false);
        towerPageGo.SetActive(false);
    }

    public void ShowMonsterPage()
    {
        if (!monsterPageGo.activeSelf)
        {
            uiManager.PlayButtonAudioClip();
            monsterPageGo.SetActive(true);
        }
        helpPageGo.SetActive(false);
        towerPageGo.SetActive(false);
    }

    public void ShowTowerPage()
    {
        if (!towerPageGo.activeSelf)
        {
            uiManager.PlayButtonAudioClip();
            towerPageGo.SetActive(true);
        }
        helpPageGo.SetActive(false);
        monsterPageGo.SetActive(false);
    }
}
