using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProperty : MonoBehaviour
{
    // 属性值
    protected float timeVal;//攻击计时器
    public float attackCD;//攻击CD
    public int price;//当前塔的价格
    [HideInInspector]
    public int sellPrice;
    public int towerLevel;
    [HideInInspector]
    public int upLoadPrice;

    // 引用
    public Tower tower;
    public Animator animator;
    public Transform targetTrans;

    // 资源
    protected GameObject bulletGO;//空资源，为了使用其成员变量与方法

    protected virtual void Start()
    {
        upLoadPrice = (int)(price * 1.5f);
        sellPrice = price / 2;
        animator = transform.Find("tower").GetComponent<Animator>();
        timeVal = attackCD;
    }

    protected virtual void Update()
    {
        if (GameController.instance.isPause || targetTrans == null) return;
        if (!targetTrans.gameObject.activeSelf)
        {
            targetTrans = null;
            return;
        }
        if (timeVal >= attackCD / GameController.instance.gameSpeed)
        {
            timeVal = 0;
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
        if (targetTrans.gameObject.tag == "Item")
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        } else
        {
            transform.LookAt(targetTrans.position);
        }

    }

    public void Init()
    {
        tower = null;
    }

    protected virtual void DestoryTower()
    {
        tower.DestoryTower();
    }
    protected virtual void Attack()
    {
        if (targetTrans == null)
        {
            return;
        }
        animator.Play("Attack");
        GameController.instance.PlayEffectMusic("NormalMordel/Tower/Attack/" + tower.towerID.ToString());
        bulletGO = GameController.instance.GetGameObjectResource(
            "Tower/ID" + tower.towerID.ToString() + "/Bullet/" + towerLevel.ToString());
        bulletGO.transform.position = transform.position;
        bulletGO.GetComponent<Bullet>().targetTrans = targetTrans;
    }
}
