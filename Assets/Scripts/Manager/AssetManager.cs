﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理资源生成，获取
/// </summary>
public class AssetManager
{
    public Dictionary<FactoryType, IBaseFacotry> factoryDict = new Dictionary<FactoryType, IBaseFacotry>();
    public AudioClipsFactory audioClipsFactory;
    public SpritesFactory spritesFactory;
    public RuntimeAnimatorControllerFactory runtimeAnimatorControllerFactory;

    public AssetManager()
    {
        factoryDict.Add(FactoryType.UIPanelFactory, new UIPanelFactory());
        factoryDict.Add(FactoryType.UIFactory, new UIFactory());
        factoryDict.Add(FactoryType.GameFactory, new GameFactory());
        audioClipsFactory = new AudioClipsFactory();
        spritesFactory = new SpritesFactory();
        runtimeAnimatorControllerFactory = new RuntimeAnimatorControllerFactory();
    }
}
