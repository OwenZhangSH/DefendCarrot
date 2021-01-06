using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasePanel
{
    /*
     * 进入panel前调用函数
     */
    void EnterPanel();

    /*
     * 离开panel前调用函数
     */
    void ExitPanel();

    /*
     * 初始化panel函数
     */
    void InitPanel();

    /*
     * 更新panel函数
     */
    void UpdatePanel();
}
