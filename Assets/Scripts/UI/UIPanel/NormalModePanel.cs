using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class NormalModePanel : BasePanel
{
    // 顶导
    // 游戏速度
    public bool isNormalSpeed = true;
    public Sprite[] gameSpeedSprites;
    public Image gameSpeedBtn;
    // 游戏暂停
    public bool isPause = false;
    public Sprite[] gamePauseSprites;
    public Image gamePauseBtn;
    public GameObject pauseGO;
    public GameObject playingGO;
    // 菜单
    public GameObject menuPageGO;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
    }

    public override void InitPanel()
    {
        base.InitPanel();
    }
    

    // 点击事件
    // 改变游戏速度
    public void ChangeGameSpeed()
    {
        uiManager.PlayButtonAudioClip();
        isNormalSpeed = !isNormalSpeed;
        if (isNormalSpeed)
        {
            GameController.instance.gameSpeed = 1;
            gameSpeedBtn.sprite = gameSpeedSprites[0];
        } else
        {
            GameController.instance.gameSpeed = 2;
            gameSpeedBtn.sprite = gameSpeedSprites[1];
        }

    }
    // 游戏暂停
    public void PauseGame()
    {
        uiManager.PlayButtonAudioClip();
        isPause = !isPause;
        if (isPause)
        {
            GameController.instance.isPause = true;
            gamePauseBtn.sprite = gamePauseSprites[1];
            pauseGO.SetActive(true);
            playingGO.SetActive(false);
        }
        else
        {
            GameController.instance.isPause = false;
            gamePauseBtn.sprite = gamePauseSprites[0];
            pauseGO.SetActive(false);
            playingGO.SetActive(true);
        }
    }

    // 打开菜单
    public void ShowMenu()
    {
        // 暂停游戏
        uiManager.PlayButtonAudioClip();
        isPause = true;
        GameController.instance.isPause = true;
        gamePauseBtn.sprite = gamePauseSprites[1];
        pauseGO.SetActive(true);
        playingGO.SetActive(false);
        menuPageGO.SetActive(true);
    }
}
