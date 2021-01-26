using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 控制游戏逻辑
/// </summary>
public class GameController : MonoBehaviour
{
    // 单例
    private static GameController _instance;
    public static GameController instance
    {
        get
        {
            return _instance;
        }
    }
    // 游戏属性
    public int gameSpeed;
    public bool isPause;

    //游戏UI的面板
    public NormalModePanel normalModepanel;

    // 数据
    public int coin;
    public int killedMonsterNum;
    public int clearItemNum;
    private void Awake()
    {
        _instance = this;
        normalModepanel = GameManager.instance.uiManager.currentScenePanelDict[StringManager.NormalModePanel]
            .GetComponent<NormalModePanel>();
        normalModepanel.InitUI();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // UI相关事件
    // 改变游戏速度
    public void ChangeGameSpeed()
    {
        gameSpeed = gameSpeed == 1 ? 2 : 1;
        normalModepanel.UpdateGameSpeedUI();
    }

    public void PauseGame()
    {
        isPause = true;
        normalModepanel.UpdatePauseGameUI();
    }

    public void ContinueGame()
    {
        isPause = true;
        normalModepanel.UpdatePauseGameUI();
    }

    public void RestartGame()
    {
        UpdatePlayerManagerData();
        SceneManager.LoadScene(4);
    }

    public void ToOtherLevel()
    {
        UpdatePlayerManagerData();
    }

    // playerManager 数据更新
    //更新基础数据
    private void UpdatePlayerManagerData()
    {
        GameManager.instance.playerManager.coin += coin;
        GameManager.instance.playerManager.killedMonsterNum += killedMonsterNum;
        GameManager.instance.playerManager.clearItemNum += clearItemNum;
    }

    
}
