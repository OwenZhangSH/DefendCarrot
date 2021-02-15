using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 游戏管理，负责管理整个游戏
/// </summary>
public class GameManager : MonoBehaviour
{
    // 单例模式，可以全局直接访问
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            return _instance;
        }
    }
    // Player Manager 管理用户信息
    public PlayerManager playerManager;
    // Asset Manager 管理资源
    public AssetManager assetManager;
    // Audio Manager 管理音乐
    public AudioManager audioManager;
    // UI Manager 管理UI元素
    public UIManager uiManager;
    // Game Controller 控制游戏逻辑
    public GameController gameController;

    private CareTaker ct;
    public bool initPlayerManager;//是否重置游戏

    public Map currentMap;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        // 管理器初始化
        // 从存档中提取memento
        ct = new CareTaker();
        if (initPlayerManager)
            ct.SetMementoFromFile(StringManager.playerManagerInitDataFilePath);
        else
            ct.SetMementoFromFile(StringManager.playerManagerDataFilePath);
        playerManager = new PlayerManager();
        //playerManager.InitPlayerManager();
        //ct.SaveMementoToFile(playerManager.createMemento());
        playerManager.SetMemento(ct.GetMemento());
        assetManager = new AssetManager();
        audioManager = new AudioManager();
        uiManager = new UIManager();
        uiManager.currentSceneState.EnterScene();
    }

    public GameObject CreateItem(GameObject go)
    {
        GameObject itemGo = Instantiate(go);
        return itemGo;
    }

    // AssetManager 透传方法
    public GameObject GetGameObjectResource(FactoryType type, string name)
    {
        return assetManager.factoryDict[type].GetItem(name);
    }

    public void PushGameObjectToFactory(FactoryType type, string name, GameObject go)
    {
        assetManager.factoryDict[type].PushItem(name, go);
    }

    public Sprite GetSprite(string path)
    {
        return assetManager.spritesFactory.GetSingleResources(path);
    }
    public RuntimeAnimatorController GetRunTimeAnimatorController(string resourcePath)
    {
        return assetManager.runtimeAnimatorControllerFactory.GetSingleResources(resourcePath);
    }
    //获取audioClip资源
    public AudioClip GetAudioClip(string resourcePath)
    {
        return assetManager.audioClipsFactory.GetSingleResources(resourcePath);
    }


    // playerManager 相关方法
    public void SaveData()
    {
        ct.SaveMementoToFile();
    }

    public void ResetGame()
    {
        ct.SetMementoFromFile(StringManager.playerManagerInitDataFilePath);
        playerManager.SetMemento(ct.GetMemento());
    }
}
