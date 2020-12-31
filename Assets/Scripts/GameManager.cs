using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    // UI Manager 管理UI元素
    // Game Controller 控制游戏逻辑
}
