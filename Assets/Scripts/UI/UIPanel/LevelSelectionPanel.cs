using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelSelectionPanel : BasePanel
{
    // 切换展示的GO
    public Transform[] levelContents; // 关卡按钮
    private ScrollViewExtend scrollView; // Scroll View
    private PlayerManager playerManager; // 用户信息

    protected override void Awake()
    {
        base.Awake();
        playerManager = uiManager.playerManager;
        scrollView = transform.Find("Scroll View").GetComponent<ScrollViewExtend>();
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        UpdateLevelInfoUI();
        scrollView.Init();
        gameObject.SetActive(true);
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        gameObject.SetActive(false);
    }

    public override void InitPanel()
    {
        base.InitPanel();
        // 更新关卡信息
        UpdateLevelInfoUI();
    }
    // 更新Level UI
    public void UpdateLevelInfoUI()
    {
        for(int i=0;i< levelContents.Length;i++)
        {
            if(playerManager.unlockedNormalModeLevelList[i])
            {
                levelContents[i].Find("Img_Lock").gameObject.SetActive(false);
                levelContents[i].Find("Img_Page").gameObject.SetActive(true);
                levelContents[i].GetComponent<Button>().interactable = true;
                levelContents[i].Find("Img_Page").Find("Text").GetComponent<Text>().text = 
                    playerManager.unlockedeNormalModeMapNum[i].ToString() + "/5";
            } else
            {
                levelContents[i].Find("Img_Lock").gameObject.SetActive(true);
                levelContents[i].Find("Img_Page").gameObject.SetActive(false);
                levelContents[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    // 顶导操作
    public void ReturnToMainPanel()
    {
        uiManager.ChangeSceneState(new MainSceneState(uiManager));
        ExitPanel();
    }

    public void ToHelpPanel()
    {
        uiManager.PlayButtonAudioClip();
        uiManager.currentScenePanelDict[StringManager.HelpPanel].GetComponent<BasePanel>().EnterPanel();
        ExitPanel();
    }

    // 点击操作
    public void ToSecondLevelPanel(int id)
    {
        uiManager.PlayButtonAudioClip();
        SecendLevelSelectionPanel secendLevelSelectionPanel = uiManager
            .currentScenePanelDict[StringManager.SecendLevelSelectionPanel]
            .GetComponent<SecendLevelSelectionPanel>();
        secendLevelSelectionPanel.currentLevelID = id;
        secendLevelSelectionPanel.EnterPanel();
        ExitPanel();
    }
}
