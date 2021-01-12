using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    // 上层透传参数
    public PlayerManager playerManager;
    public UIManager()
    {
        // 初始化变量
        playerManager = GameManager.instance.playerManager;
        // 创建空的字典
        currentScenePanelDict = new Dictionary<string, GameObject>();
        // 初始化遮罩作为场景切换的动画
        InitMask();
        currentSceneState = new StartLoadSceneState(this);
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
        GameObject itemGo = GameManager.instance.GetGameObjectResource(FactoryType.UIFactory, uiName);
        itemGo.transform.SetParent(canvasTransform);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        return itemGo;
    }

    //实例化当前场景所有面板，并存入字典
    public void InitDict()
    {
        foreach (var item in currentScenePanelDict)
        {
            item.Value.transform.SetParent(canvasTransform);
            item.Value.transform.localPosition = Vector3.zero;
            item.Value.transform.localScale = Vector3.one;
            IBasePanel basePanel = item.Value.GetComponent<IBasePanel>();
            if (basePanel == null)
            {
                Debug.Log("获取面板上IBasePanel脚本失败");
            }
            basePanel.InitPanel();
        }
    }

    // 清除所有面板
    public void ClearDict()
    {
        foreach (var item in currentScenePanelDict)
        {
            GameManager.instance.PushGameObjectToFactory(FactoryType.UIPanelFactory, item.Key, item.Value);
        }
        currentScenePanelDict.Clear();
    }

    public void AddPanelToDict(string panelName)
    {
        currentScenePanelDict.Add(panelName, GameManager.instance.GetGameObjectResource(
            FactoryType.UIPanelFactory, panelName));
    }

    /*
     * 切换scene状态
     */
    public void ChangeSceneState(IBaseSceneState sceneState)
    {
        lastSceneState = currentSceneState;
        currentSceneState = sceneState;
        ShowMask();
    }

    //显示遮罩
    public void ShowMask()
    {
        mask.transform.SetSiblingIndex(10);
        Tween t = DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 1), 2f);
        t.OnComplete(ExitSceneComplete);
    }

    //离开当前场景
    private void ExitSceneComplete()
    {
        lastSceneState.ExitScene();
        currentSceneState.EnterScene();
        HideMask();
    }

    //隐藏遮罩
    public void HideMask()
    {
        mask.transform.SetSiblingIndex(10);
        Tween t = DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 0), 2f);
    }

    /// <summary>
    /// 音乐控制
    /// </summary>
    //开关音乐
    public void CloseOrOpenBGMusic()
    {
        GameManager.instance.audioManager.CloseOrOpenBGMusic();
    }

    public void CloseOrOpenEffectMusic()
    {
        GameManager.instance.audioManager.CloseOrOpenEffectMusic();
    }

    //播放按钮音效
    public void PlayButtonAudioClip()
    {
        GameManager.instance.audioManager.PlayButtonAudioClip();
    }

    //播放翻书音效
    public void PlayPagingAudioClip()
    {
        GameManager.instance.audioManager.PlayPagingAudioClip();
    }

}
