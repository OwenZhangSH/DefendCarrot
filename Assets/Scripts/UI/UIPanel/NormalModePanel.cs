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
    // StartUI
    public GameObject startGameUI;
    // Final Wave UI
    public GameObject finalWaveUI;
    // 金币
    public Text coinText;
    public Text currenWaveText;
    public Text totalWaveText;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        InitUI();
        gameObject.SetActive(true);
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
    }

    public void UpdateUI()
    {
        UpdatePauseGameUI();
        UpdateGameSpeedUI();
        UpdateCoinUI();
        updateTotalWaveUI();
        UpdateWaveUI();
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

    // 更新金币UI
    public void UpdateCoinUI()
    {
        coinText.text = GameController.instance.coin.ToString();
    }
    // 更新回合UI
    public void UpdateWaveUI()
    {
        int first = 0;
        int second = 0;
        int currentWave = GameController.instance.level.currentWave + 1;
        if (currentWave >= 10)
        {
            second = currentWave % 10;
            first = currentWave / 10;
        } else
        {
            second = currentWave;
        }
        currenWaveText.text = first.ToString() + " " + second.ToString();
    }

    // 更新总回合UI
    public void updateTotalWaveUI()
    {
        totalWaveText.text = GameController.instance.level.totalWaveNum.ToString();
    }

    // 开始加载游戏
    public void ShowStartAnim()
    {
        startGameUI.SetActive(true);
        InvokeRepeating("PlayAudio", 0.5f, 1);
    }

    private void PlayAudio()
    {
        GameController.instance.PlayEffectMusic("NormalMordel/CountDown");
    }

    public void CloseStartUI()
    {
        startGameUI.SetActive(false);
        CancelInvoke();
    }

    public void ShowFinalWaveUI()
    {
        finalWaveUI.SetActive(true);
        Invoke("CloseFinalWaveUI", 0.508f);
    }

    public void CloseFinalWaveUI()
    {
        finalWaveUI.SetActive(false);
        GameController.instance.level.HandleLastWave();
    }
    // 胜利页面
    public void ShowWinPage()
    {
        gameWinPageGO.SetActive(true);
    }
    // 失败页面
    public void ShowLosePage()
    {
        gameOverPageGO.SetActive(true);
    }

    // prize页面
    public void ShowPrizePage()
    {
        prizePageGO.SetActive(true);
    }
}
