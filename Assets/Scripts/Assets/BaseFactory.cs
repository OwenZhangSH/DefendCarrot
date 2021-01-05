using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFactory : IBaseFacotry
{
    // 资源池 对应prefab原有资源
    protected Dictionary<string, GameObject> facotryDict = new Dictionary<string, GameObject>();
    // 对象池 对应游戏对象
    protected Dictionary<string, Stack<GameObject>> objectPoolDict = new Dictionary<string, Stack<GameObject>>();

    // 加载路径
    protected string loadPath;

    public BaseFactory()
    {
        loadPath = "Prefabs/";
    }

    public GameObject GetItem(string itemName)
    {
        GameObject itemGo = null;
        if (objectPoolDict.ContainsKey(itemName))//包含此对象池
        {
            if (objectPoolDict[itemName].Count == 0)
            {
                GameObject go = GetResource(itemName);
                itemGo = GameManager.instance.CreateItem(go);
            }
            else
            {
                itemGo = objectPoolDict[itemName].Pop();
                itemGo.SetActive(true);
            }
        }
        else//不包含此对象池
        {
            objectPoolDict.Add(itemName, new Stack<GameObject>());
            GameObject go = GetResource(itemName);
            itemGo = GameManager.instance.CreateItem(go);
        }

        if (itemGo == null)
        {
            Debug.Log(itemName + "的实例获取失败");
        }

        return itemGo;
    }

    public void PushItem(string itemName, GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(GameManager.instance.transform);
        if (objectPoolDict.ContainsKey(itemName))
        {
            objectPoolDict[itemName].Push(item);
        }
        else
        {
            Debug.Log("当前字典没有" + itemName + "的栈");
        }
    }

    private GameObject GetResource(string itemName)
    {
        GameObject itemGo = null;
        string itemLoadPath = loadPath + itemName;
        if (facotryDict.ContainsKey(itemName))
        {
            itemGo = facotryDict[itemName];
        }
        else
        {
            itemGo = Resources.Load<GameObject>(itemLoadPath);
            facotryDict.Add(itemName, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log(itemName + "的资源获取失败");
            Debug.Log("失败路径：" + itemLoadPath);
        }
        return itemGo;
    }
}
