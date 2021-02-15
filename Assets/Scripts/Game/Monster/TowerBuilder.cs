using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : IBuilder<Tower>
{
    public int towerID;
    public int towerLevel;
    private GameObject towerGO;

    public GameObject GetProduct()
    {
        // 获取GO
        GameObject itemGO = GameController.instance.GetGameObjectResource(
            "Tower/ID" + towerID.ToString() + "/TowerSet/" + towerLevel.ToString());
        // 加载数据
        Tower tower = GetProductClass(itemGO);
        SetData(tower);
        SetResource(tower);
        return itemGO;
    }

    public Tower GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Tower>();
    }

    public void SetData(Tower productClassGO)
    {
        productClassGO.towerID = towerID;
    }

    public void SetResource(Tower productClassGo)
    {
        productClassGo.SetTowerProperty();
    }
}
