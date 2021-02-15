using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public Transform targetTrans; // 目标位置
    public int moveSpeed;
    public int attackValue;
    public int towerID;
    public int towerLevel;

    protected virtual void DestoryBullect()
    {
        targetTrans = null;
        GameController.instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() + "/Bullet/" + towerLevel.ToString(), gameObject);
    }

    protected virtual void Update()
    {
        //游戏结束
        if (GameController.instance.gameOver)
        {
            DestoryBullect();
        }
        //游戏暂停
        if (GameController.instance.isPause)
        {
            return;
        }
        if (targetTrans == null || !targetTrans.gameObject.activeSelf)
        {
            DestoryBullect();
            return;
        }

        //子弹的移动与转向
        if (targetTrans.gameObject.tag == "Item")
        {
            transform.position = Vector3.Lerp(transform.position, targetTrans.position + new Vector3(0, 0, 3),
                1 / Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 3) * Time.deltaTime * moveSpeed * GameController.instance.gameSpeed));
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetTrans.position,
              1 / Vector3.Distance(transform.position, targetTrans.position * Time.deltaTime * moveSpeed * GameController.instance.gameSpeed));
            transform.LookAt(targetTrans.position);
        }


    }
    protected virtual void CreateEffect()
    {
        GameObject effectGo = GameController.instance.GetGameObjectResource("Tower/ID" + towerID.ToString() + "/Effect/" + towerLevel.ToString());
        effectGo.transform.position = transform.position;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetTrans == null) return;
        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            if (collision.gameObject.activeSelf)
            {
                Debug.Log("target: " + targetTrans);
                if (targetTrans.position == null || (collision.tag == "Item" && GameController.instance.targetTrans == null))
                {
                    return;
                }
                if (collision.tag == "Monster" || (collision.tag == "Item" && GameController.instance.targetTrans == collision.transform))
                {
                    collision.SendMessage("TakeDamage", attackValue);
                    CreateEffect();
                    DestoryBullect();
                }
            }
        }
    }
}
