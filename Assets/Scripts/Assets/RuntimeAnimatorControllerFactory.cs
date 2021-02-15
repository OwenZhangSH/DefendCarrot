using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeAnimatorControllerFactory : IBaseResourceFactory<RuntimeAnimatorController>
{
    protected Dictionary<string, RuntimeAnimatorController> factoryDict = new Dictionary<string, RuntimeAnimatorController>();
    protected string loadPath;

    public RuntimeAnimatorControllerFactory()
    {
        loadPath = "Animator/AnimatorController/";
    }

    public RuntimeAnimatorController GetSingleResources(string resourcePath)
    {
        RuntimeAnimatorController item = null;
        string itemLoadPath = loadPath + resourcePath;
        if (factoryDict.ContainsKey(resourcePath)) item = factoryDict[resourcePath];
        else
        {
            item = Resources.Load<RuntimeAnimatorController>(itemLoadPath);
            factoryDict[resourcePath] = item;
        }
        if (item == null)
        {
            Debug.Log(resourcePath + "的资源获取失败，失败路径为:" + itemLoadPath);
        }
        return item;
    }
}
