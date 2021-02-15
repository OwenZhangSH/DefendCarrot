using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class MapMaker : MonoBehaviour
{
#if Tool
    public bool drawLine;
    public GameObject gridGO;
    private static MapMaker _instance;
    public static MapMaker Instance { get => _instance; }
#endif
    // 游戏属性
    // 地图
    private float mapWidth;
    private float mapHeight;

    // 格子
    [HideInInspector]
    public float gridWidth;
    [HideInInspector]
    public float gridHeight;

    //行列
    public const int yRow = 8;
    public const int xColumn = 12;

    // 关卡索引
    public int levelID;
    public int mapID;

    // 路径
    [HideInInspector]
    public List<GridPoint.GridIndex> monsterPath;
    [HideInInspector]
    public List<Vector3> monsterPathPos;
    // 格子对象
    public GridPoint[,] gridPoints;

    // 怪物波数
    public List<Wave.WaveInfo> waveInfoList;
    // 背景
    private SpriteRenderer bgSR;
    private SpriteRenderer roadSR;

    // 萝卜
    [HideInInspector]
    public Carrot carrot;

    private void Awake()
    {
#if Tool
        _instance = this;
        InitMapMaker();
#endif
    }

    // 初始化地图
    public void InitMapMaker()
    {
        CalculateSize();
        gridPoints = new GridPoint[xColumn, yRow];
        monsterPath = new List<GridPoint.GridIndex>();
        monsterPathPos = new List<Vector3>();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
#if Tool
                GameObject itemGO = Instantiate(gridGO, transform.position, transform.rotation);
#endif
#if Game
                GameObject itemGO = GameController.instance.GetGameObjectResource("Grid");
#endif
                itemGO.transform.SetParent(transform);
                itemGO.transform.position = 
                    new Vector3(
                        x * gridWidth - mapWidth / 2 + gridWidth /2,
                        y* gridHeight - mapHeight/2 + gridHeight/2);
                gridPoints[x, y] = itemGO.GetComponent<GridPoint>();
                gridPoints[x, y].gridIndex.xIndex = x;
                gridPoints[x, y].gridIndex.yIndex = y;
            }
        }
        bgSR = transform.Find("BG").GetComponent<SpriteRenderer>();
        roadSR = transform.Find("Road").GetComponent<SpriteRenderer>();
    }

    public void CalculateSize()
    {
        Vector3 leftDown = new Vector3(0, 0);
        Vector3 rightUp = new Vector3(1, 1);

        Vector3 posLeft = Camera.main.ViewportToWorldPoint(leftDown);
        Vector3 posRight = Camera.main.ViewportToWorldPoint(rightUp);

        mapWidth = posRight.x - posLeft.x;
        mapHeight = posRight.y - posLeft.y;

        gridWidth = mapWidth / xColumn;
        gridHeight = mapHeight / yRow;
    }

    // 加载地图
    public void LoadMap(int level, int map)
    {
        levelID = level;
        mapID = map;
        UpdateMapFromLevelInfo(LoadLevelInfo(levelID.ToString() + "_" + mapID.ToString() + ".json"));
        monsterPathPos = new List<Vector3>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            monsterPathPos.Add(gridPoints[monsterPath[i].xIndex, monsterPath[i].yIndex].transform.position);
        }

        //起始点与终止点
        GameObject startPointGo = GameController.instance.GetGameObjectResource("startPoint");
        startPointGo.transform.position = monsterPathPos[0];
        startPointGo.transform.SetParent(transform);

        GameObject endPointGo = GameController.instance.GetGameObjectResource("Carrot");
        endPointGo.transform.position = monsterPathPos[monsterPathPos.Count - 1] - new Vector3(0, 0, 1);
        endPointGo.transform.SetParent(transform);
        carrot = endPointGo.GetComponent<Carrot>();
    }

#if Tool
    private void OnDrawGizmos()
    {
        if (!drawLine) return;
        CalculateSize();
        Gizmos.color = Color.green;
        // 画行
        for (int y = 0; y <= yRow; y++)
        {
            Vector3 startPos = new Vector3(-mapWidth / 2, -mapHeight / 2 + y * gridHeight);
            Vector3 endPos = new Vector3(mapWidth / 2, -mapHeight / 2 + y * gridHeight);
            Gizmos.DrawLine(startPos, endPos);
        }
        // 画列
        for (int x = 0; x <= xColumn; x++)
        {
            Vector3 startPos = new Vector3(-mapWidth / 2 + x * gridWidth, -mapHeight / 2);
            Vector3 endPos = new Vector3(-mapWidth / 2 + x * gridWidth, mapHeight / 2);
            Gizmos.DrawLine(startPos, endPos);
        }
    }
#endif
    /// <summary>
    /// 地图编辑相关
    /// </summary>
    /// 
    // 生成LevelInfo
    private LevelInfo CreateLevelInfo()
    {
        LevelInfo levelInfo = new LevelInfo
        {
            levelID = this.levelID,
            mapID = this.mapID
        };
        // set 格子状态
        levelInfo.gridPoints = new List<GridPoint.GridState>();
        for (int x=0; x< xColumn;x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                levelInfo.gridPoints.Add(gridPoints[x, y].gridState);
            }
        }
        // set 路径
        levelInfo.monsterPath = new List<GridPoint.GridIndex>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            levelInfo.monsterPath.Add(monsterPath[i]);
        }
        // set Wave Info
        levelInfo.waveInfo = new List<Wave.WaveInfo>();
        for (int i = 0; i < waveInfoList.Count; i++)
        {
            levelInfo.waveInfo.Add(waveInfoList[i]);
        }

        return levelInfo;
    }
    // 保存
    public void SaveLevel()
    {
        LevelInfo levelInfo = CreateLevelInfo();
        string filePath = Application.streamingAssetsPath + "/Json/Level/" + levelID.ToString() + "_" +
            mapID.ToString() + ".json";
        string saveStr = JsonMapper.ToJson(levelInfo);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveStr);
        sw.Close();
    }
    
    // 读取Level Info
    public LevelInfo LoadLevelInfo(string fileName)
    {
        LevelInfo levelInfo = new LevelInfo();
        string filePath = Application.streamingAssetsPath + "/Json/Level/" + fileName;
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string levelInfoStr = sr.ReadToEnd();
            sr.Close();
            levelInfo = JsonMapper.ToObject<LevelInfo>(levelInfoStr);
        }

        return levelInfo;
    }

    // 根据LevelInfo 更新地图
    public void UpdateMapFromLevelInfo(LevelInfo levelInfo)
    {
        levelID = levelInfo.levelID;
        mapID = levelInfo.mapID;
        // 更新格子状态
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].gridState = levelInfo.gridPoints[y + x * yRow];
                // 格子更新
                gridPoints[x, y].UpdateGrid();
            }
        }
        // 更新路径点
        monsterPath.Clear();
        for (int i = 0; i < levelInfo.monsterPath.Count; i++)
        {
            monsterPath.Add(levelInfo.monsterPath[i]);
        }
        // 更新波次信息
        waveInfoList.Clear();
        for (int i = 0; i < levelInfo.waveInfo.Count; i++)
        {

            waveInfoList.Add(levelInfo.waveInfo[i]);
        }
        bgSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + levelID.ToString() + "/" + "BG" + 
            (mapID / 3).ToString());
        roadSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + levelID.ToString() + "/" + 
            "Road" + mapID);
    }

    // 清除路径点
    public void ClearMonsterPath()
    {
        for (int i = 0; i < monsterPath.Count; i++)
        {
            gridPoints[monsterPath[i].xIndex, monsterPath[i].yIndex].InitGrid();
        }
        monsterPath.Clear();
    }

    // 重置地图
    public void RecoverTowerPoint()
    {
        monsterPath.Clear();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].InitGrid();
            }
        }
    }

    // 初始化地图
    public void InitMap()
    {
        levelID = 0;
        mapID = 0;
        RecoverTowerPoint();
        waveInfoList.Clear();
        bgSR.sprite = null;
        roadSR.sprite = null;
    }
}
