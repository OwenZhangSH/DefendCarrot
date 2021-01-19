using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesFactory : IBaseResourceFactory<Sprite>
{
    protected Dictionary<string, Sprite> factoryDict = new Dictionary<string, Sprite>();
    protected string loadPath;

    public SpritesFactory()
    {
        loadPath = "Pictures/";
    }

    public Sprite GetSingleResources(string resourcePath)
    {
        Sprite item = null;
        string itemLoadPath = loadPath + resourcePath;
        if (factoryDict.ContainsKey(resourcePath)) item = factoryDict[resourcePath];
        else
        {
            item = Resources.Load<Sprite>(itemLoadPath);
            factoryDict[resourcePath] = item;
        }
        if (item == null)
        {
            Debug.Log(resourcePath + "的资源获取失败，失败路径为:" + itemLoadPath);
        }
        return item;
    }
}
