using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    // 切换展示的GO
    private GameObject optionPageGO;
    private GameObject statisticsPageGO;
    private GameObject producerPageGO;
    private GameObject resetPanelGO;

    // 主页进入动画
    private Tween enterTween;

    // 音效图片
    private bool playBGMusic = true;
    private bool playEffectMusic = true;
    private Image effectAudioImage;
    private Image bgAudioImage;
    public Sprite[] btnSprites;//0.音效开 1.音效关 2.背景音乐开 3.背景音乐关

    // 统计文本
    public Text[] statisticesTexts;

    protected override void Awake()
    {
        base.Awake();
        // 初始化变量
        optionPageGO = transform.Find("OptionPage").gameObject;
        statisticsPageGO = transform.Find("StatisticsPage").gameObject;
        producerPageGO = transform.Find("ProducerPage").gameObject;
        resetPanelGO = transform.Find("ResetPanel").gameObject;
        // 初始化动画
        enterTween = transform.DOLocalMoveX(0, 0.5f).SetAutoKill(false).Pause();
        // 初始化开关图片
        effectAudioImage = optionPageGO.transform.Find("Btn_EffectAudio").GetComponent<Image>();
        bgAudioImage = optionPageGO.transform.Find("Btn_BGAudio").GetComponent<Image>();
    }

    public override void InitPanel()
    {
        transform.localPosition = new Vector3(-1920, 0, 0);
        transform.SetSiblingIndex(2);
    }

    public override void EnterPanel()
    {
        ShowOptionPage();
        MoveToCenter();
    }

    public override void ExitPanel()
    {
        uiManager.PlayButtonAudioClip();
        enterTween.PlayBackwards();
        uiManager.currentScenePanelDict[StringManager.MainPanel].GetComponent<BasePanel>().EnterPanel();
        InitPanel();
    }

    /// <summary>
    /// 显示页面
    /// </summary>
    public void ShowOptionPage()
    {
        if (!optionPageGO.activeSelf)
        {
            uiManager.PlayButtonAudioClip();
            optionPageGO.SetActive(true);
        }
        statisticsPageGO.SetActive(false);
        producerPageGO.SetActive(false);
    }

    public void ShowStatisticsPage()
    {
        if (!statisticsPageGO.activeSelf)
        {
            uiManager.PlayButtonAudioClip();
            statisticsPageGO.SetActive(true);
            ShowStatistics();
        }
        optionPageGO.SetActive(false);
        producerPageGO.SetActive(false);
    }

    public void ShowProducerPage()
    {
        if (!producerPageGO.activeSelf)
        {
            uiManager.PlayButtonAudioClip();
            producerPageGO.SetActive(true);
        }
        optionPageGO.SetActive(false);
        statisticsPageGO.SetActive(false);
    }

    // 进入动画播放
    public void MoveToCenter()
    {
        enterTween.PlayForward();
    }

    // 显示数据
    public void ShowStatistics()
    {
        PlayerManager playerManager = uiManager.playerManager;
        statisticesTexts[0].text = playerManager.normalModeLevelNum.ToString();
        statisticesTexts[1].text = playerManager.burriedLevelNum.ToString();
        statisticesTexts[2].text = playerManager.bossModeNum.ToString();
        statisticesTexts[3].text = playerManager.coin.ToString();
        statisticesTexts[4].text = playerManager.killedMonsterNum.ToString();
        statisticesTexts[5].text = playerManager.killedBossNum.ToString();
        statisticesTexts[6].text = playerManager.clearItemNum.ToString();
    }

    /// <summary>
    /// 音乐控制
    /// </summary>

    public void CloseOrOpenBGMusic()
    {
        uiManager.PlayButtonAudioClip();
        playBGMusic = !playBGMusic;
        uiManager.CloseOrOpenBGMusic();
        if (playBGMusic)
        {
            bgAudioImage.sprite = btnSprites[2];
        }
        else
        {
            bgAudioImage.sprite = btnSprites[3];
        }
    }

    public void CloseOrOpenEffectMusic()
    {
        uiManager.PlayButtonAudioClip();
        playEffectMusic = !playEffectMusic;
        uiManager.CloseOrOpenEffectMusic();
        if (playEffectMusic)
        {
            effectAudioImage.sprite = btnSprites[0];
        }
        else
        {
            effectAudioImage.sprite = btnSprites[1];
        }
    }
    /// <summary>
    /// 重置游戏
    /// </summary>

    public void OpenResetPanel()
    {
        resetPanelGO.SetActive(true);
    }

    public void CloseResetPanel()
    {
        resetPanelGO.SetActive(false);
    }

    public void ResetGame()
    {
        uiManager.PlayButtonAudioClip();
        GameManager.instance.initPlayerManager = true;
        GameManager.instance.ResetGame();
        CloseResetPanel();
    }
}
