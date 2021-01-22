using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
