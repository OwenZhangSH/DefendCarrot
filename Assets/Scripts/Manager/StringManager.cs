using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 字符串管理，以防止写错
/// </summary>
public class StringManager
{
    // 存档
    public static string playerManagerInitDataFilePath = Application.streamingAssetsPath + "/Json" 
        + "/playerManagerInitData.json";
    public static string playerManagerDataFilePath = Application.streamingAssetsPath + "/Json"
        + "/playerManager.json";
    public const string StartLoadPanel = "StartLoadPanel";
    public const string MainPanel = "MainPanel";
    public const string SettingPanel = "SettingPanel";
    public const string GameLoadPanel = "GameLoadPanel";
    public const string HelpPanel = "HelpPanel";
    public const string GameNormalOptionPanel = "GameNormalOptionPanel";
    public const string GameNormalBigLevelPanel = "GameNormalBigLevelPanel";
    public const string GameNormalLevelPanel = "GameNormalLevelPanel";
    public const string GameBossOptionPanel = "GameBossOptionPanel";
    public const string NormalModelPanel = "NormalModelPanel";
    public const string BossModelPanel = "BossModelPanel";
    public const string MonsterNestPanel = "MonsterNestPanel";

}
