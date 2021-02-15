using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GridPoint : MonoBehaviour
{
    // 属性
    private SpriteRenderer spriteRenderer;
    public GridState gridState;
    public GridIndex gridIndex;
    // 引用
    private Sprite gridSprite; // 格子图片
    private Sprite startSprite;
    private Sprite cantBuildSprite;
#if Tool
    private Sprite monsterPathSprite;//怪物路点图片资源
    public GameObject[] itemPrefabs;//道具数组
    public GameObject currentItem;//当前格子持有道具
#endif
    // 格子状态
    [System.Serializable]
    public struct GridState
    {
        public bool canBuild;
        public bool isMonsterPoint;
        public bool hasItem;
        public int itemID;
    }
    // 格子坐标
    [System.Serializable]
    public struct GridIndex
    {
        public int xIndex;
        public int yIndex;
    }

    private SpriteRenderer bgSR;
    private SpriteRenderer roadSR;


    // 塔相关
    [HideInInspector]
    public bool hasTower;
    public GameObject towerGO;
    public Tower tower;
    public int towerLevel;
    public TowerProperty towerProperty;

    private GameObject towerListGO;//当前关卡建塔列表
    private GameObject towerCanvasGO;
    private Transform uploadButtonTrans;//两个按钮的trans引用
    private Transform sellTowerButtonTrans;
    private Vector3 uploadButtonInitPos;//两个按钮的初始位置
    private Vector3 sellTowerButtonInitPos;

    // 升级提示
    private GameObject uploadSignalGO;//是否可升级信号

    private void Awake()
    {
#if Tool
        gridSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/Grid");
        monsterPathSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/1/Monster/6-1");
        itemPrefabs = new GameObject[10];
        string prefabsPath = "Prefabs/Game/" + MapMaker.Instance.levelID.ToString() + "/Item/";
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            itemPrefabs[i] = Resources.Load<GameObject>(prefabsPath + i);
            if (!itemPrefabs[i])
            {
                Debug.Log("加载失败，失败路径：" + prefabsPath + i);
            }
        }
#endif
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitGrid();
#if Game
        gridSprite = GameController.instance.GetSprite("NormalMordel/Game/Grid");
        startSprite = GameController.instance.GetSprite("NormalMordel/Game/StartSprite");
        cantBuildSprite = GameController.instance.GetSprite("NormalMordel/Game/cantBuild");
        spriteRenderer.sprite = startSprite;
        Tween t = DOTween.To(
            () => spriteRenderer.color,
            toColor => spriteRenderer.color = toColor,
            new Color(1, 1, 1, 0.2f),
            3
        ).OnComplete(ChangeSpriteToGrid);
        // 塔相关
        towerListGO = GameController.instance.towerListGO;
        towerCanvasGO = GameController.instance.towerCanvasGO;
        uploadButtonTrans = towerCanvasGO.transform.Find("Btn_UpLevel");
        sellTowerButtonTrans = towerCanvasGO.transform.Find("Btn_SellTower");
        uploadButtonInitPos = uploadButtonTrans.localPosition;
        sellTowerButtonInitPos = sellTowerButtonTrans.localPosition;
        uploadSignalGO = transform.Find("LevelUpSignal").gameObject;
        uploadSignalGO.SetActive(false);
#endif
    }

    private void Update()
    {
        if (uploadSignalGO != null)
        {
            if (hasTower)
            {
                if (towerProperty.upLoadPrice <= GameController.instance.coin && towerLevel < 3)
                {
                    uploadSignalGO.SetActive(true);
                }
                else
                {
                    uploadSignalGO.SetActive(false);
                }
            }
            else
            {
                if (uploadSignalGO.activeSelf)
                {
                    uploadSignalGO.SetActive(false);
                }
            }
        }
    }

    private void ChangeSpriteToGrid()
    {
        spriteRenderer.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);

        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
        }
        else
        {
            spriteRenderer.sprite = cantBuildSprite;
        }
    }

#if Game
    private void OnMouseDown()
    {
        //选择的是UI则不发生交互
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (gridState.canBuild)
        {
            GridPoint selectGrid = GameController.instance.selectGrid;
            if (selectGrid == null)
            {
                GameController.instance.selectGrid = this;
                ShowGrid();
                GameController.instance.PlayEffectMusic("NormalMordel/Grid/GridSelect");
            }
            else if (selectGrid == this)
            {
                HideGrid();
                GameController.instance.selectGrid = null;
                GameController.instance.PlayEffectMusic("NormalMordel/Grid/GridSelect");
            }
            else if (selectGrid != this)
            {
                selectGrid.HideGrid();
                GameController.instance.selectGrid = this;
                ShowGrid();
                GameController.instance.PlayEffectMusic("NormalMordel/Grid/GridSelect");
            }
        } else
        {
            HideGrid();
            ShowCantBuild();
            GameController.instance.PlayEffectMusic("NormalMordel/Grid/SelectFault");
            if (GameController.instance.selectGrid != null)
            {
                GameController.instance.selectGrid.HideGrid();
            }
        }
    }
#endif
    /// <summary>
    /// 格子处理
    /// </summary>
    /// 
    public void ShowGrid()
    {
        if (!hasTower)
        {
            spriteRenderer.enabled = true;
            //显示建塔列表
            towerListGO.transform.position = CorrectTowerListGoPosition();
            towerListGO.SetActive(true);
        }
        else
        {
            towerCanvasGO.transform.position = transform.position;
            CorrectHandleTowerCanvasGoPosition();
            towerCanvasGO.SetActive(true);
            //显示塔的攻击范围
            towerGO.transform.Find("attackRange").gameObject.SetActive(true);
        }
    }
    public void HideGrid()
    {
        if (!hasTower)
        {
            //隐藏建塔列表
            towerListGO.SetActive(false);
        }
        else
        {
            towerCanvasGO.SetActive(false);
            //隐藏塔的范围
            towerGO.transform.Find("attackRange").gameObject.SetActive(false);
        }
        spriteRenderer.enabled = false;
    }

    public void CloseSR()
    {
        spriteRenderer.enabled = false;
    }

    public void ShowCantBuild()
    {
        spriteRenderer.enabled = true;
        Tween t = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color = toColor, new Color(1, 1, 1, 0), 2f);
        t.OnComplete(() =>
        {
            spriteRenderer.enabled = false;
            spriteRenderer.color = new Color(1, 1, 1, 1);
        });
    }

    private Vector3 CorrectTowerListGoPosition()
    {
        Vector3 correctPosition = Vector3.zero;
        if (gridIndex.xIndex <= 3 && gridIndex.xIndex >= 0)
        {
            correctPosition += new Vector3(GameController.instance.mapMaker.gridWidth, 0, 0);
        }
        else if (gridIndex.xIndex <= 11 && gridIndex.xIndex >= 8)
        {
            correctPosition -= new Vector3(GameController.instance.mapMaker.gridWidth, 0, 0);
        }
        if (gridIndex.yIndex <= 3 && gridIndex.yIndex >= 0)
        {
            correctPosition += new Vector3(0, GameController.instance.mapMaker.gridHeight, 0);
        }
        else if (gridIndex.yIndex <= 7 && gridIndex.yIndex >= 4)
        {
            correctPosition -= new Vector3(0, GameController.instance.mapMaker.gridHeight, 0);
        }
        correctPosition += transform.position;
        return correctPosition;
    }

    private void CorrectHandleTowerCanvasGoPosition()
    {
        uploadButtonTrans.localPosition = Vector3.zero;
        sellTowerButtonTrans.localPosition = Vector3.zero;
        if (gridIndex.yIndex <= 0)
        {
            if (gridIndex.xIndex == 0)
            {
                sellTowerButtonTrans.position += new Vector3(GameController.instance.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            else
            {
                sellTowerButtonTrans.position -= new Vector3(GameController.instance.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            uploadButtonTrans.localPosition = uploadButtonInitPos;
        }
        else if (gridIndex.yIndex >= 6)
        {
            if (gridIndex.xIndex == 0)
            {
                uploadButtonTrans.position += new Vector3(GameController.instance.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            else
            {
                uploadButtonTrans.position -= new Vector3(GameController.instance.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
        }
        else
        {
            uploadButtonTrans.localPosition = uploadButtonInitPos;
            sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
        }
    }

    public void UpdateTowerData()
    {
        spriteRenderer.enabled = false;
        towerGO = transform.GetChild(1).gameObject;
        tower = towerGO.GetComponent<Tower>();
        towerProperty = towerGO.GetComponent<TowerProperty>();
        towerLevel = towerProperty.towerLevel;
        hasTower = true;
    }

#if Tool
    private void OnMouseDown()
    {
        // 路径
        if (Input.GetKey(KeyCode.P))
        {
            gridState.canBuild = false;
            spriteRenderer.enabled = true;
            gridState.isMonsterPoint = !gridState.isMonsterPoint;
            if (gridState.isMonsterPoint)
            {
                MapMaker.Instance.monsterPath.Add(gridIndex);
                spriteRenderer.sprite = monsterPathSprite;
            }else
            {
                MapMaker.Instance.monsterPath.Remove(gridIndex);
                spriteRenderer.sprite = gridSprite;
                gridState.canBuild = true;
            } 
        } else if(Input.GetKey(KeyCode.I))
        {
            gridState.itemID++;
            if (gridState.itemID == itemPrefabs.Length)
            {
                gridState.itemID = -1;
                Destroy(currentItem);
                currentItem = null;
                gridState.hasItem = false;
                return;
            }
            if (currentItem == null)
            {
                //产生道具
                CreateItem();
            }
            else//本身就有道具
            {
                Destroy(currentItem);
                //产生道具
                CreateItem();
            }
            gridState.hasItem = true;
        } else if (!gridState.isMonsterPoint)
        {
            gridState.isMonsterPoint = false;
            gridState.canBuild = !gridState.canBuild;
            if (gridState.canBuild)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }

#endif
    public void InitGrid()
    {
        gridState.canBuild = true;
        gridState.isMonsterPoint = false;
        gridState.hasItem = false;
        spriteRenderer.enabled = true;
        gridState.itemID = -1;
#if Tool
        spriteRenderer.sprite = gridSprite;
        Destroy(currentItem); 
#endif
        towerGO = null;
        towerProperty = null;
        hasTower = false;
    }

    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
#if Tool
            if (gridState.isMonsterPoint)
            {
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
#endif
#if Game
            spriteRenderer.enabled = false;
#endif
        }
    }

    // 生成道具
    private void CreateItem()
    {
        GameObject itemGO = GameController.instance.GetGameObjectResource(
            GameController.instance.mapMaker.levelID.ToString() + "/Item/" + gridState.itemID);
        // item 参数处理
        itemGO.GetComponent<Item>().itemID = gridState.itemID;
        itemGO.GetComponent<Item>().gridPoint = this;
        itemGO.transform.SetParent(GameController.instance.transform);
        Vector3 createPos = transform.position - new Vector3(0, 0, 3);
        if (gridState.itemID <= 2)
        {
            createPos += new Vector3(GameController.instance.mapMaker.gridWidth,
                -GameController.instance.mapMaker.gridHeight) / 2;
        }
        else if (gridState.itemID <= 4)
        {
            createPos += new Vector3(GameController.instance.mapMaker.gridWidth, 0) / 2;
        }
        itemGO.transform.position = createPos;
    }
}
