using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipsFactory : IBaseResourceFactory<AudioClip>
{
    protected Dictionary<string, AudioClip> factoryDict = new Dictionary<string, AudioClip>();
    protected string loadPath;

    public AudioClipsFactory()
    {
        loadPath = "AudioClips/";
    }

    public AudioClip GetSingleResources(string resourcePath)
    {
        AudioClip item = null;
        string itemLoadPath = loadPath + resourcePath;
        if (factoryDict.ContainsKey(resourcePath)) item = factoryDict[resourcePath];
        else
        {
            item = Resources.Load<AudioClip>(itemLoadPath);
            factoryDict[resourcePath] = item;
        }
        if (item == null)
        {
            Debug.Log(resourcePath + "的资源获取失败，失败路径为:" + itemLoadPath);
        }
        return item;
    }
}
