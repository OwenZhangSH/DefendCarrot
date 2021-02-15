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
    public Level level;
    public int gameSpeed;
    public bool isPause;
    public bool isCreatingMonster;
    public int[] monsterIDList;//当前波次的产怪列表
    public bool isStart;
    public bool gameOver;
    // 组件
    // 游戏UI的面板
    public NormalModePanel normalModepanel;
    public MapMaker mapMaker;
    public GridPoint selectGrid;//上一个选择的格子

    // 数据
    public int coin;
    public int killedMonsterNum;
    public int clearItemNum;
    public int monsterIDIndex;//用于统计当前怪物列表产生怪物的索引
    // 游戏玩家数据
    public int coinTotalNum;
    public int killedMonsterTotalNum;
    public int clearItemTotalNum;
    // 地图信息
    [HideInInspector]
    public Map currentMap;

    // 萝卜状态
    public int carrotHp;

    // 集火目标
    public Transform targetTrans;
    public GameObject targetSignal;

    // Builder
    public MonsterBuilder monsterBuilder;
    public TowerBuilder towerBuilder;
    // 建塔
    //建塔有关的成员变量
    public Dictionary<int, int> towerPriceDict;//建塔价格表  
    public GameObject towerListGO;//建塔按钮列表
    public GameObject towerCanvasGO;//处理塔升级与买卖的画布
    //游戏资源
    public RuntimeAnimatorController[] controllers;//怪物的动画播放控制器

    // 调试使用
    public bool gameWin;
    public bool isFirstWin;
    public bool gameLose;
    public bool isFirstLose;
    private void Awake()
    {
        _instance = this;
        isStart = false;
        normalModepanel = GameManager.instance.uiManager.currentScenePanelDict[StringManager.NormalModePanel]
            .GetComponent<NormalModePanel>();
        normalModepanel.EnterPanel();
        mapMaker = GetComponent<MapMaker>();
        currentMap = GameManager.instance.currentMap;
        //currentMap = GameManager.instance.playerManager.unlockedNormalModeMapList[(currentLevelID - 1) * 5 + currentMapID];
        InitGame();
        LoadingGame();
        isPause = true;
        // builder
        monsterBuilder = new MonsterBuilder();
        // Animator
        controllers = new RuntimeAnimatorController[12];
        for(int i = 0; i < controllers.Length; i++)
        {
            controllers[i] = GetRuntimeAnimatorController("Monster/" + mapMaker.levelID.ToString() + "/" + (i + 1).ToString());
        }
        // 建塔相关
        towerBuilder = new TowerBuilder();
        // 建塔需要的按键
        for (int i = 0; i < currentMap.towerIDList.Length; i++)
        {
            GameObject itemGo = GameManager.instance.GetGameObjectResource(FactoryType.UIFactory, "Btn_TowerBuild");
            itemGo.transform.GetComponent<TowerButton>().towerID = currentMap.towerIDList[i];
            itemGo.transform.SetParent(towerListGO.transform);
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.transform.localScale = Vector3.one;
        }
        //建塔价格表
        towerPriceDict = new Dictionary<int, int>
        {
            { 1,100},
            { 2,120},
            { 3,140},
            { 4,160},
            { 5,160}
        };
    }

    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart) return;
        if (!isPause)
        {
            if (killedMonsterNum >= monsterIDList.Length)
            {
                if (level.currentWave == level.totalWaveNum)
                {
                    return;
                }
                AddRoundNum();
            }
            else
            {
                if (!isCreatingMonster)
                {
                    CreateMonster();
                }
            }
        }
        else
        {
            //暂停
            StopCreateMonster();
            isCreatingMonster = false;
        }

        if (!isFirstWin)
        {
            if (gameWin)
            {
                isFirstWin = true;
                GameWin();
            }
        }
        if (!isFirstLose)
        {
            if (gameLose)
            {
                isFirstLose = true;
                GameLose();
            }
        }
    }

    // 下一回合
    public void AddRoundNum()
    {
        monsterIDIndex = 0;
        killedMonsterNum = 0;
        level.AddWaveNum();
        level.HandleWave();
        //更新有关回合显示的UI
        normalModepanel.UpdateUI();
    }

    // 初始化游戏
    public void InitGame()
    {
        // 初始化Game数据
        InitGameData();
        // 初始化地图
        mapMaker.InitMapMaker();
        mapMaker.LoadMap(currentMap.levelID, currentMap.mapID);
        // 初始化关卡信息
        level = new Level(mapMaker.waveInfoList.Count, mapMaker.waveInfoList);
        // 初始化UI
        normalModepanel.UpdateUI();
    }

    // 加载游戏
    public void LoadingGame()
    {
        // 播放加载动画
        normalModepanel.ShowStartAnim();
        Invoke("GameStart", 3.5f);
    }

    // 更新游戏数据
    public void InitGameData()
    {
        gameSpeed = 1;
        isPause = true;
        coin = 1000;
        killedMonsterNum = 0;
        clearItemNum = 0;
        carrotHp = 10;
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
        isPause = false;
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

    public void ChangeCoin(int coinNum)
    {
        coin += coinNum;
        if (coinNum >=0)
        {
            coinTotalNum += coinNum;
        }
        normalModepanel.UpdateCoinUI();
    }

    // playerManager 数据更新
    //更新基础数据
    private void UpdatePlayerManagerData()
    {
        GameManager.instance.playerManager.coin += coinTotalNum;
        GameManager.instance.playerManager.killedMonsterNum += killedMonsterTotalNum;
        GameManager.instance.playerManager.clearItemNum += clearItemTotalNum;
    }

    /// <summary>
    /// 游戏逻辑
    /// </summary>
    
    // 游戏胜利
    public void GameWin()
    {
        // 游戏暂停
        PauseGame();
        gameOver = true;
        // 更新游戏情况
        Map map = GameManager.instance.playerManager.unlockedNormalModeMapList[currentMap.mapID - 1 + (currentMap.levelID - 1) * 5];
        if (IsAllClear())
        {
            map.isAllClear = true;
        }
        //萝卜徽章更新
        int carrotState = GetCarrotState();
        if (carrotState != 0 && map.carrotState != 0)
        {
            if (map.carrotState > carrotState)
            {
                map.carrotState = carrotState;
            }
        }
        else if (map.carrotState == 0)
        {
            map.carrotState = carrotState;
        }
        //解锁下一个关卡
        //不是最后一关且不是隐藏关卡才能解锁下一关
        if (currentMap.mapID % 5 != 0)
        {
            // 解锁下一关卡
            GameManager.instance.playerManager.unlockedNormalModeMapList[currentMap.mapID + (currentMap.levelID - 1) * 5].isUnlocked = true;
            // 当前大关解锁数量++
            GameManager.instance.playerManager.unlockedeNormalModeMapNum[currentMap.levelID - 1]++;
        }
        // 更新玩家数据
        UpdatePlayerManagerData();
        GameManager.instance.playerManager.normalModeLevelNum++;
        // 打开游戏胜利页面
        normalModepanel.ShowWinPage();
        // 播放音效
        PlayEffectMusic("NormalMordel/Perfect");

    }

    public bool IsAllClear()
    {
        for (int x = 0; x < MapMaker.xColumn; x++)
        {
            for (int y = 0; y < MapMaker.yRow; y++)
            {
                if (mapMaker.gridPoints[x, y].gridState.hasItem)
                {
                    return false;
                }
            }
        }
        return true;
    }

    //获取萝卜状态
    public int GetCarrotState()
    {
        int carrotState = 0;
        if (carrotHp >= 4)
        {
            carrotState = 1;
        }
        else if (carrotHp >= 2)
        {
            carrotState = 2;
        }
        else
        {
            carrotState = 3;
        }
        return carrotState;
    }

    // 最后一波
    public void StartFinalWave()
    {
        // 播放最后
        PlayEffectMusic("NormalMordel/Finalwave");
        // 播放动画
        normalModepanel.ShowFinalWaveUI();
    }

    // 游戏失败
    public void GameLose()
    {
        // 暂停游戏
        gameOver = true;
        PauseGame();
        // 更新玩家数据
        UpdatePlayerManagerData();
        // 打开失败页面
        normalModepanel.ShowLosePage();
        // 播放音效
        PlayEffectMusic("NormalMordel/Lose");
    }

    // 游戏开始
    public void GameStart()
    {
        isStart = true;
        // 关闭加载UI
        normalModepanel.CloseStartUI();
        // 取消暂停
        ContinueGame();
        // track 先set true 确保只有handle时会将isCreatingMonster设置为false;
        isCreatingMonster = true;
        level.HandleWave();
    }

    /// <summary>
    /// 怪物相关
    /// </summary>
    public void CreateMonster()
    {
        isCreatingMonster = true;
        InvokeRepeating("InstantiateMonster", (float)1 / gameSpeed, (float)1 / gameSpeed);
    }

    public void StopCreateMonster()
    {
        CancelInvoke();
    }

    private void InstantiateMonster()
    {
        PlayEffectMusic("NormalMordel/Monster/Create");
        if (monsterIDIndex >= monsterIDList.Length)
        {
            StopCreateMonster();
            return;
        }
        // 特效
        GameObject effectGo = GetGameObjectResource("CreateEffect");
        effectGo.transform.SetParent(transform);
        effectGo.transform.position = mapMaker.monsterPathPos[0];
        // 生成怪物
        if (monsterIDIndex < monsterIDList.Length)
        {
            monsterBuilder.monsterID = level.waveList[level.currentWave].waveInfo.monsterIDList[monsterIDIndex];
        }
        GameObject monsterGO = monsterBuilder.GetProduct();
        monsterGO.transform.SetParent(transform);
        monsterGO.transform.position = mapMaker.monsterPathPos[0];
        monsterIDIndex++;
    }

    public void DecreaseHP()
    {
        PlayEffectMusic("NormalMordel/Carrot/Crash");
        carrotHp--;
        mapMaker.carrot.UpdateCarrotUI();
    }

    /// <summary>
    /// 建塔相关
    /// </summary>
    public void BuildTower(int towerID)
    {
        // 播放音效`
        PlayEffectMusic("NormalMordel/Tower/TowerBulid");
        towerBuilder.towerID = towerID;
        towerBuilder.towerLevel = 1;
        GameObject towerGO = towerBuilder.GetProduct();
        towerGO.transform.SetParent(selectGrid.transform);
        towerGO.transform.position = selectGrid.transform.position;
        // 特效
        GameObject effectGO = GetGameObjectResource("BuildEffect");
        effectGO.transform.SetParent(transform);
        effectGO.transform.position = selectGrid.transform.position;
        // 更新格子的状态
        selectGrid.HideGrid();
        selectGrid.UpdateTowerData();
        // 更新游戏数据
        ChangeCoin(-towerPriceDict[towerID]);
        selectGrid = null;
        // 先隐藏一次更新数值
        towerCanvasGO.SetActive(false);
    }

    public void SellTower()
    {
        // 播放音效
        PlayEffectMusic("NormalMordel/Tower/TowerSell");
        coin += selectGrid.towerProperty.sellPrice;
        normalModepanel.UpdateCoinUI();
        // 特效
        GameObject itemGO = GetGameObjectResource("BuildEffect");
        itemGO.transform.position = selectGrid.transform.position;
        // 销毁掉之前的塔
        selectGrid.tower.DestoryTower();
        selectGrid.HideGrid();
        // 更新数据
        selectGrid.InitGrid();
        selectGrid.CloseSR();
        selectGrid = null;
    }

    public void UploadTower()
    {
        if (selectGrid.towerProperty.towerLevel >= 3) return;
        int towerID = selectGrid.tower.towerID;
        // 播放音效
        PlayEffectMusic("NormalMordel/Tower/TowerUpdata");
        // 扣除金钱
        ChangeCoin(-selectGrid.towerProperty.upLoadPrice);
        // 升级特效
        GameObject itemGO = GetGameObjectResource("UpLevelEffect");
        itemGO.transform.position = selectGrid.transform.position;
        // 销毁掉之前的塔
        selectGrid.tower.DestoryTower();
        // 创建新的塔
        towerBuilder.towerID = towerID;
        towerBuilder.towerLevel = selectGrid.towerProperty.towerLevel + 1;
        GameObject towerGO = towerBuilder.GetProduct();
        towerGO.transform.SetParent(selectGrid.transform);
        towerGO.transform.position = selectGrid.transform.position;
        // 更新格子的状态
        selectGrid.HideGrid();
        selectGrid.UpdateTowerData();
        selectGrid = null;
    }

    /// <summary>
    /// 集火UI
    /// </summary>
    /// 

    public void ShowSignal()
    {
        PlayEffectMusic("NormalMordel/Tower/ShootSelect");
        targetSignal.transform.position = targetTrans.position + new Vector3(0, mapMaker.gridHeight / 2, 0);
        targetSignal.transform.SetParent(targetTrans);
        targetSignal.SetActive(true);
    }

    public void HideSignal()
    {
        targetSignal.gameObject.SetActive(false);
        targetTrans = null;
    }

    public void ChangeTarget(Transform trans)
    {
        HideSignal();
        targetTrans = trans;
        ShowSignal();
    }

    public void CancelTarget()
    {
        HideSignal();
    }

    /// <summary>
    /// 资源加载
    /// </summary>
    public GameObject GetGameObjectResource(string resourcePath)
    {
        return GameManager.instance.GetGameObjectResource(FactoryType.GameFactory, resourcePath);
    }
    public Sprite GetSprite(string resourcePath)
    {
        return GameManager.instance.GetSprite(resourcePath);
    }
    public AudioClip GetAudioClip(string resourcePath)
    {
        return GameManager.instance.GetAudioClip(resourcePath);
    }
    public void PushGameObjectToFactory(string name, GameObject go)
    {
        GameManager.instance.PushGameObjectToFactory(FactoryType.GameFactory, name, go);
    }
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return GameManager.instance.GetRunTimeAnimatorController(resourcePath);
    }

    //播放特效音
    public void PlayEffectMusic(string audioClipPath)
    {
        GameManager.instance.audioManager.PlayEffectMusic(GetAudioClip(audioClipPath));
    }
}
