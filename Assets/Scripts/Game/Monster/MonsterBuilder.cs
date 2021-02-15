using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuilder : IBuilder<Monster>
{
    public int monsterID;
    private GameObject monsterGO;

    public GameObject GetProduct()
    {
        // 获取GO
        GameObject itemGO = GameController.instance.GetGameObjectResource("MonsterPrefab");
        // 加载数据
        Monster monster = GetProductClass(itemGO);
        SetData(monster);
        SetResource(monster);
        return itemGO;
    }

    public Monster GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Monster>();
    }

    public void SetData(Monster productClassGO)
    {
        productClassGO.monsterID = monsterID;
        productClassGO.HP = monsterID * 100;
        productClassGO.currentHP = monsterID * 100;
        productClassGO.speed = monsterID;
        productClassGO.currentSpeed = monsterID;
        productClassGO.coin = monsterID * 50;
    }

    public void SetResource(Monster productClassGo)
    {
        productClassGo.SetMonsterAnimator();
    }
}
