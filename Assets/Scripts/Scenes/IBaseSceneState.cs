using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseSceneState
{
    /// <summary>
    /// 进入scene的调用函数
    /// </summary>
    void EnterScene();

    /// <summary>
    /// 退出scene的调用函数
    /// </summary>
    void ExitScene();
}
