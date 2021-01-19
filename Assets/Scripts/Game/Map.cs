using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡类
/// </summary>
public class Map
{
    public int[] towerIDList; //本关卡可以建的塔种类
    public int towerIDListLength;//建塔数组长度
    public bool isAllClear;//是否清空此关卡道具
    public int carrotState;//萝卜状态
    public int mapID;//小关卡ID
    public int levelID;//大关卡ID
    public bool isUnlocked;//此关卡是否解锁
    public bool isRewardLevel;//是否为奖励关卡
    public int waveNum;//一共几波怪

    //public Map(int waveNum, int towerIDListLength, int[] towerIDList,
    //    bool allClear, int carrotState, int mapID, int levelID, bool locked, bool isRewardLevel)
    //{
    //    this.waveNum = waveNum;
    //    this.towerIDListLength = towerIDListLength;
    //    this.towerIDList = towerIDList;
    //    isAllClear = allClear;
    //    this.carrotState = carrotState;
    //    this.mapID = mapID;
    //    this.levelID = levelID;
    //    isUnlocked = locked;
    //    this.isRewardLevel = isRewardLevel;
    //}
}
