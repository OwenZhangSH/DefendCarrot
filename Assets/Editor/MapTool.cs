using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

[CustomEditor(typeof(MapMaker))]
public class MapTool : Editor
{
    private MapMaker mapMaker;
    // 文件信息
    private List<FileInfo> fileList = new List<FileInfo>();
    private string[] fileNameList;

    // 当前编辑的关卡索引
    public int selectIndex = -1;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            mapMaker = MapMaker.Instance;

            EditorGUILayout.BeginHorizontal();
            //获取操作的文件名
            fileNameList = GetNames(fileList);
            int currentIndex = EditorGUILayout.Popup(selectIndex, fileNameList);
            if (currentIndex != selectIndex)//当前选择对象是否改变
            {
                selectIndex = currentIndex;

                // 初始化地图
                mapMaker.InitMapMaker();
                //加载当前选择的level文件
                mapMaker.UpdateMapFromLevelInfo(mapMaker.LoadLevelInfo(fileNameList[selectIndex]));
            }

            if (GUILayout.Button("读取关卡列表"))
            {
                LoadLevelFiles();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("回复地图编辑器默认状态"))
            {
                mapMaker.RecoverTowerPoint();
            }

            if (GUILayout.Button("清除怪物路点"))
            {
                mapMaker.ClearMonsterPath();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("保存当前关卡数据文件"))
            {
                mapMaker.SaveLevel();
            }
        }
    }

    /// <summary>
    /// 相关方法
    /// </summary>
    public string[] GetNames(List<FileInfo> files)
    {
        List<string> names = new List<string>();
        foreach (FileInfo file in files)
        {
            names.Add(file.Name);
        }
        return names.ToArray();
    }

    private void LoadLevelFiles()
    {
        ClearList();
        fileList = GetLevelFiles();
    }

    //清除文件列表
    private void ClearList()
    {
        fileList.Clear();
        selectIndex = -1;
    }

    //读取关卡信息列表
    private List<FileInfo> GetLevelFiles()
    {
        string[] files = Directory.GetFiles(Application.streamingAssetsPath + "/Json/Level/", "*.json");

        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            list.Add(file);
        }
        return list;
    }
}
