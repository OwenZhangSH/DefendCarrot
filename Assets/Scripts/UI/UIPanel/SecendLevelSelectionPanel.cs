using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SecendLevelSelectionPanel : BasePanel
{
    public int currentLevelID = 1;
    public int currentMapID = 0;
    // 引用
    public Image bgLeft;
    public Image bgRight;
    public GameObject btnStart;
    public GameObject btnLock;
    public Text waveText;
    public Transform towerTrans;
    private ScrollViewExtend scrollViewExtend;
    public RectTransform contentTrans;
    private PlayerManager playerManager;
    public GridLayoutGroup gridLayoutGroup;

    // 变量
    private string filePath;//图片资源加载的根路径
    private string spritesPath; // 精灵资源路径

    private List<GameObject> levelContentImageGOs; //实例化出来的地图卡片UI
    private List<GameObject> towerContentImageGOs; //实例化出来的建塔列表UI
    
    protected override void Awake()
    {
        base.Awake();
        filePath = StringManager.LevelResourceRoot;
        scrollViewExtend = transform.Find("Scroll View").GetComponent<ScrollViewExtend>();
        scrollViewExtend.PageChange += OnPageChange;
        playerManager = uiManager.playerManager;
        currentLevelID = 1;
        currentMapID = 0;
        levelContentImageGOs = new List<GameObject>();
        towerContentImageGOs = new List<GameObject>();
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        gameObject.SetActive(true);
        ClearMapList();
        InitLevelInfo(currentLevelID);
        scrollViewExtend.Init();
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        gameObject.SetActive(false);
    }

    public override void InitPanel()
    {
        base.InitPanel();
        gameObject.SetActive(false);
    }

    // 初始化当前level
    public void InitLevelInfo(int id)
    {
        // 更新图片路径
        spritesPath = filePath + currentLevelID.ToString() + "/";
        // 更新外部UI元素
        bgLeft.sprite = uiManager.GetSprite(spritesPath + "BG_Left");
        bgRight.sprite = uiManager.GetSprite(spritesPath + "BG_Right");
        // 更新content
        for (int i =0; i < 5;i++)
        {
            // 实例化UI
            levelContentImageGOs.Add(CreateUIAndSetUIPosition("Img_Level", contentTrans));
            levelContentImageGOs[i].GetComponent<Image>().sprite = uiManager.GetSprite(
                spritesPath + "Level_" + (i + 1).ToString());
            levelContentImageGOs[i].transform.Find("Img_Carrot").gameObject.SetActive(false);
            levelContentImageGOs[i].transform.Find("Img_AllClear").gameObject.SetActive(false);
            Map map = playerManager.unlockedNormalModeMapList[(currentLevelID - 1) * 5 + i];
            if (map.isUnlocked)
            {
                // 解锁
                if (map.isAllClear)
                {
                    levelContentImageGOs[i].transform.Find("Img_AllClear").gameObject.SetActive(true);
                }
                if (map.carrotState != 0)
                {
                    Image carrotImage = levelContentImageGOs[i].transform.Find("Img_Carrot").GetComponent<Image>();
                    carrotImage.sprite = uiManager.GetSprite(filePath + "Carrot_" + map.carrotState);
                    carrotImage.gameObject.SetActive(true);
                }
                levelContentImageGOs[i].transform.Find("Img_Lock").gameObject.SetActive(false);
                levelContentImageGOs[i].transform.Find("Img_BG").gameObject.SetActive(false);
            } else
            {
                if (map.isRewardLevel)
                {
                    levelContentImageGOs[i].transform.Find("Img_Lock").gameObject.SetActive(false);
                    levelContentImageGOs[i].transform.Find("Img_BG").gameObject.SetActive(true);
                    Image monsterPetImage = levelContentImageGOs[i].transform.Find("Img_BG")
                        .Find("Img_Monster").GetComponent<Image>();
                    monsterPetImage.sprite = uiManager.GetSprite("MonsterNest/Monster/Baby/" + currentLevelID.ToString());
                    //monsterPetImage.SetNativeSize();
                    //monsterPetImage.transform.localScale = new Vector3(2, 2, 1);
                } else
                {
                    levelContentImageGOs[i].transform.Find("Img_Lock").gameObject.SetActive(true);
                    levelContentImageGOs[i].transform.Find("Img_BG").gameObject.SetActive(false);
                }
            }

        }
        float cellLength = gridLayoutGroup.cellSize.x;
        float spacing = gridLayoutGroup.spacing.x;
        contentTrans.sizeDelta = new Vector2((cellLength + spacing) * 4, contentTrans.sizeDelta.y);
        scrollViewExtend.ChangeTotalNum(5);
        // 更新随map 变化的UI元素
        UpdateMapUI();
    }

    public void UpdateMapUI()
    {
        // 清空tower list
        if (towerContentImageGOs.Count != 0)
        {
            for (int i = 0; i < towerContentImageGOs.Count; i++)
            {
                towerContentImageGOs[i].GetComponent<Image>().sprite = null;
                uiManager.PushGameObjectToFactory(FactoryType.UIFactory, "Img_Tower", towerContentImageGOs[i]);
            }
            towerContentImageGOs.Clear();
        }
        // 关卡信息
        Map map = playerManager.unlockedNormalModeMapList[(currentLevelID - 1) * 5 + currentMapID];
        // 更新锁定
        if (map.isUnlocked)
        {
            btnLock.SetActive(false);
            btnStart.SetActive(true);
        }
        else
        {
            btnLock.SetActive(true);
            btnStart.SetActive(false);
        }
        // 更新波次文本
        waveText.text = map.waveNum.ToString();
        // 更新可建造塔列表
        for (int i = 0; i < map.towerIDListLength; i++)
        {
            towerContentImageGOs.Add(CreateUIAndSetUIPosition("Img_Tower", towerTrans));
            towerContentImageGOs[i].GetComponent<Image>().sprite = uiManager
                .GetSprite(filePath + "Tower" + "/Tower_" + map.towerIDList[i].ToString());
        }
    }

    // 清空MapList
    private void ClearMapList()
    {
        if (levelContentImageGOs.Count > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                uiManager.PushGameObjectToFactory(FactoryType.UIFactory, "Img_Level", levelContentImageGOs[i]);
            }
            levelContentImageGOs.Clear();
        }
    }

    public GameObject CreateUIAndSetUIPosition(string name, Transform parentTrans)
    {
        GameObject itemGo = uiManager.GetGameObjectResource(FactoryType.UIFactory, name);
        itemGo.transform.SetParent(parentTrans);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        return itemGo;
    }

    // 顶导操作
    public void ReturnToLevelPanel()
    {
        uiManager.PlayButtonAudioClip();
        uiManager.currentScenePanelDict[StringManager.LevelSelectionPanel].GetComponent<BasePanel>().EnterPanel();
        ExitPanel();
    }

    public void ToHelpPanel()
    {
        uiManager.PlayButtonAudioClip();
        uiManager.currentScenePanelDict[StringManager.HelpPanel].GetComponent<BasePanel>().EnterPanel();
        ExitPanel();
    }

    // 进入游戏
    public void ToGamePanel()
    {
        // TODO: 进入游戏
    }

    // 当移动发生后的触发函数
    // TODO: 完成
    public void OnPageChange(int pageNum)
    {
        currentMapID = pageNum;
        UpdateMapUI();
    }
}
