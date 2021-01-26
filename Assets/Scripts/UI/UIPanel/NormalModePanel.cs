using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    // GameOver 
    public GameObject gameOverPageGO;
    // GameWin
    public GameObject gameWinPageGO;
    // PrizePage
    public GameObject prizePageGO;
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
        GameController.instance.ChangeGameSpeed();
    }
    
    public void UpdateGameSpeedUI()
    {
        isNormalSpeed = GameController.instance.gameSpeed == 1? true:false;
        if (isNormalSpeed)
        {
            gameSpeedBtn.sprite = gameSpeedSprites[0];
        }
        else
        {
            gameSpeedBtn.sprite = gameSpeedSprites[1];
        }
    }
    // 游戏暂停
    public void PauseGame()
    {
        uiManager.PlayButtonAudioClip();
        isPause = GameController.instance.isPause;
        if (!isPause)
            GameController.instance.PauseGame();
        else
            GameController.instance.ContinueGame();
    }

    public void UpdatePauseGameUI()
    {
        isPause = GameController.instance.isPause;
        if (isPause)
        {
            gamePauseBtn.sprite = gamePauseSprites[1];
            pauseGO.SetActive(true);
            playingGO.SetActive(false);
        }
        else
        {
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
        GameController.instance.PauseGame();
        menuPageGO.SetActive(true);
    }
    
    // 继续游戏
    public void Continue()
    {
        uiManager.PlayButtonAudioClip();
        GameController.instance.ContinueGame();
        menuPageGO.SetActive(false);
    }

    // 重新开始
    public void Restart()
    {
        uiManager.PlayButtonAudioClip();
        GameController.instance.RestartGame();
    }
    
    // 初始化UI
    public void InitUI()
    {
        gameOverPageGO.SetActive(false);
        gameWinPageGO.SetActive(false);
        menuPageGO.SetActive(false);
        gameObject.SetActive(false);
        UpdatePauseGameUI();
        UpdateGameSpeedUI();
    }

    // 选择关卡
    public void ChooseOtherLevel()
    {
        uiManager.PlayButtonAudioClip();
        GameController.instance.ToOtherLevel();
        uiManager.ChangeSceneState(new NormalModeOptionSceneState(uiManager));
    }
    
    // 关闭奖励页面
    public void ClosePrizePage()
    {
        uiManager.PlayButtonAudioClip();
        GameController.instance.ContinueGame();
        prizePageGO.SetActive(false);
    }
}
