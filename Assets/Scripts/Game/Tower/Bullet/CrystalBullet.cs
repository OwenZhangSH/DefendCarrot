using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBullet : Bullet
{
    private float attackTimeVal;
    private bool canTakeDamage;

    protected override void Update()
    {
        if (GameController.instance.gameOver) DestoryBullect();
        if (GameController.instance.isPause) return;
        if (targetTrans == null || !targetTrans.gameObject.activeSelf)
        {
            DestoryBullect();
            return;
        }
        if (targetTrans.gameObject.tag == "Item")
        {

            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else
        {

            transform.LookAt(targetTrans.position);
        }
        if (!canTakeDamage)
        {
            attackTimeVal += Time.deltaTime;
            if (attackTimeVal >= 0.5f - towerLevel * 0.15f)
            {
                canTakeDamage = true;
                attackTimeVal = 0;
                DecreaseHP();
            }
        }
    }

    private void DecreaseHP()
    {
        if (!canTakeDamage || targetTrans == null)
        {
            return;
        }
        if (targetTrans.gameObject.activeSelf)
        {
            if (targetTrans.position == null || (targetTrans.tag == "Item" && GameController.instance.targetTrans == null))
            {
                return;
            }
            if (targetTrans.tag == "Monster" || (targetTrans.tag == "Item" && GameController.instance.targetTrans.gameObject.tag == "Item"))
            {
                targetTrans.SendMessage("TakeDamage", attackValue);
                CreateEffect();
                canTakeDamage = false;
            }
        }
    }
    protected override void CreateEffect()
    {
        if (targetTrans.position == null)
        {
            return;
        }
        GameObject effectGO = GameController.instance.GetGameObjectResource(
            "Tower/ID" + towerID.ToString() + "/Effect/" + towerLevel.ToString());
        effectGO.transform.position = targetTrans.position;

    }
}
