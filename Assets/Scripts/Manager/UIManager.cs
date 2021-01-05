using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理UIpanel的展示
/// </summary>
public class UIManager
{
    // 当前场景所有UI面板索引
    public Dictionary<string, GameObject> currentScenePanelDict;
    //其他成员变量
    private GameObject mask;
    private Image maskImage;
    public Transform canvasTransform;
    //场景状态
    public IBaseSceneState currentSceneState;
    public IBaseSceneState lastSceneState;
    // 构造函数
    public UIManager()
    {
        // 创建空的字典
        currentScenePanelDict = new Dictionary<string, GameObject>();
        // 初始化遮罩作为场景切换的动画
        InitMask();
    }
    
    // 初始化遮罩
    public void InitMask()
    {
        // 获取Canvas
        canvasTransform = GameObject.Find("Canvas").transform;
        mask = CreateUIAndSetUIPosition("Img_Mask");
        maskImage = mask.GetComponent<Image>();
    }

    //实例化UI
    public GameObject CreateUIAndSetUIPosition(string uiName)
    {
        // TODO: 实例化UI
        // GameObject itemGo = GameManager.instance.GetGameObjectResource(FactoryType.UIFactory, uiName);
        return null;
    }

    //实例化当前场景所有面板，并存入字典
    public void InitDict()
    {
        // TODO: 实例化所有面板
        
    }

    // 清除所有面板
    public void ClearDict()
    {
        // TODO: 清除所有面板
    }

    public void AddPanelToDict(string panelName)
    {
        // TODO: 清除所有面板 将面板加入字典
    }
}
