using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushToPool : MonoBehaviour
{
    public string resourcePath;

    public void PushGameObjectToPool()
    {
        GameManager.instance.PushGameObjectToFactory(FactoryType.GameFactory, resourcePath, gameObject);
    }
}
