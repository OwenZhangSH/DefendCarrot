using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 工厂类接口
/// </summary>
/// <typeparam name="T">需要实例化的产品类型</typeparam>
public interface IBaseResourceFactory<T>
{
    T GetSingleResources(string resourcePath);
}
