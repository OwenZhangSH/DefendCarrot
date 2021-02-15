using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillBullet : Bullet
{
    private bool hasTarget;
    private float timeVal;

    private void OnEnable()
    {
        hasTarget = false;
        timeVal = 0;
    }

    private void InitTarget()
    {
        if (targetTrans.gameObject.tag == "Item")
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else
        {
            transform.LookAt(targetTrans.position);
        }
    }

    protected override void Update()
    {
        if (GameController.instance.gameOver || timeVal >= 2.5f)
        {
            DestoryBullect();
        }
        if (GameController.instance.isPause) return;
        if (timeVal < 2.5f)
        {
            timeVal += Time.deltaTime;
        }
        if (hasTarget)
        {
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            if (targetTrans != null && targetTrans.gameObject.activeSelf)
            {
                hasTarget = true;
                InitTarget();
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            if (targetTrans.position == null || (collision.tag == "Item" && GameController.instance.targetTrans == null))
            {
                return;
            }
            if (collision.tag == "Monster" || (collision.tag == "Item" && GameController.instance.targetTrans == collision.transform))
            {
                collision.SendMessage("TakeDamage", attackValue);
                CreateEffect();
            }
        }
    }
}
