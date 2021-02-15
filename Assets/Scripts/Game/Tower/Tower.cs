using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // 属性
    public int towerID;

    // 引用
    private CircleCollider2D circleCollider2D;
    private SpriteRenderer attackRangeSR;
    // 相关属性
    private TowerProperty towerProperty;//塔的功能脚本

    public bool isPointFire;
    public bool hasTarget;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        towerProperty = GetComponent<TowerProperty>();
        towerProperty.tower = this;
        attackRangeSR = transform.Find("attackRange").GetComponent<SpriteRenderer>();
        attackRangeSR.gameObject.SetActive(false);
        attackRangeSR.transform.localScale = new Vector3(towerProperty.towerLevel, towerProperty.towerLevel, 1);
        circleCollider2D.radius = 1.1f * towerProperty.towerLevel;
        isPointFire = false;
        hasTarget = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.isPause)
        {
            return;
        }
        // 如果当前目标消失，重置目标
        if (hasTarget)
        {
            if (!towerProperty.targetTrans.gameObject.activeSelf)
            {
                towerProperty.targetTrans = null;
                hasTarget = false;
                isPointFire = false;
            }
        }
        // 如果集火目标转换，重置目标
        if (isPointFire)
        {
            if (towerProperty.targetTrans != GameController.instance.targetTrans)
            {
                Debug.Log(123);
                towerProperty.targetTrans = null;
                hasTarget = false;
                isPointFire = false;
            }
        }
    }

    public void SetTowerProperty()
    {

    }

    public void DestoryTower()
    {
        // 属性重置
        towerProperty.Init();
        // 塔重置
        Init();
        // 放回资源池
        GameController.instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() + "/TowerSet/" + towerProperty.towerLevel.ToString(), gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Monster" && collision.tag != "Item" && isPointFire)
        {
            return;
        }
        //集火目标
        Transform pointFireTrans = GameController.instance.targetTrans;
        // 如果有集火目标
        if (pointFireTrans != null)
        {
            // 当前目标是否是集火目标,如果是不改变目标
            if (!isPointFire)
            {
                // 检测物体是否是集火目标
                if(collision.transform == pointFireTrans)
                {
                    towerProperty.targetTrans = collision.transform;
                    isPointFire = true;
                    hasTarget = true;
                } else
                {
                    if (collision.tag == "Monster" && !hasTarget)
                    {
                        towerProperty.targetTrans = collision.transform;
                        hasTarget = true;
                    }
                }
            }
        } else
        {
            // 如果当前没有目标，设置目标，如果有目标保持不变
            if (!hasTarget && collision.tag == "Monster")
            {
                towerProperty.targetTrans = collision.transform;
                hasTarget = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (towerProperty.targetTrans == collision.transform)
        {
            towerProperty.targetTrans = null;
            hasTarget = false;
            isPointFire = false;
        }
    }
}
