using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTower : TowerProperty
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
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
    }
}
