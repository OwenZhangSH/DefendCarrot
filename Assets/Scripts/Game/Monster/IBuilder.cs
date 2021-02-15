using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder<T>
{
    //获取到游戏物体身上的脚本对象，从而去赋值
    T GetProductClass(GameObject gameObject);
    //bulid具体的游戏对象
    GameObject GetProduct();
    //设置具体的数据
    void SetData(T productClassGO);
    //设置特有的资源
    void SetResource(T productClassGO);
}
