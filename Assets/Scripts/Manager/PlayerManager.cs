using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制玩家信息相关
/// </summary>
public class PlayerManager
{
    public int normalModeLevelNum; //冒险模式解锁的地图个数
    public int burriedLevelNum; //隐藏关卡解锁的地图个数
    public int bossModeNum;//boss模式KO的BOSS
    public int coin;//获得金币的总数
    public int killedMonsterNum;//杀怪总数
    public int killedBossNum;//杀掉BOSS的总数
    public int clearItemNum;//清理道具的总数
    public List<bool> unlockedNormalModeLevelList;//大关卡
    public List<Map> unlockedNormalModeMapList;//所有的小关卡
    public List<int> unlockedeNormalModeMapNum;//解锁小关卡数量

    //怪物窝
    public int cookies;
    public int milk;
    public int nest;
    public int diamands;
    public List<MonsterPetData> monsterPetDataList;//宠物喂养信息


    // 创建Memento
    public Memento createMemento()
    {
        return new Memento(this);
    }

    // 读取Memento中的数据
    public void SetMemento(Memento memento)
    {
        PlayerManager playerManager = memento.GetPlayerManager();
        //数据信息
        normalModeLevelNum = playerManager.normalModeLevelNum;
        burriedLevelNum = playerManager.burriedLevelNum;
        bossModeNum = playerManager.bossModeNum;
        coin = playerManager.coin;
        killedMonsterNum = playerManager.killedMonsterNum;
        killedBossNum = playerManager.killedBossNum;
        clearItemNum = playerManager.clearItemNum;
        cookies = playerManager.cookies;
        milk = playerManager.milk;
        nest = playerManager.nest;
        diamands = playerManager.diamands;
        //列表
        unlockedNormalModeLevelList = playerManager.unlockedNormalModeLevelList;
        unlockedNormalModeMapList = playerManager.unlockedNormalModeMapList;
        unlockedeNormalModeMapNum = playerManager.unlockedeNormalModeMapNum;
        monsterPetDataList = playerManager.monsterPetDataList;
    }

    //用于玩家初始Json文件的制作
    public void InitPlayerManager()
    {
        normalModeLevelNum = 0;
        burriedLevelNum = 0;
        bossModeNum = 0;
        coin = 0;
        killedMonsterNum = 0;
        killedBossNum = 0;
        clearItemNum = 0;
        cookies = 100;
        milk = 100;
        nest = 1;
        diamands = 10;
        unlockedeNormalModeMapNum = new List<int>()
        {
            1,0,0
        };
        unlockedNormalModeLevelList = new List<bool>()
        {
            true,false,false
        };
        unlockedNormalModeMapList = new List<Map>();
        monsterPetDataList = new List<MonsterPetData>()
        {
            new MonsterPetData()
            {
                monsterID=1,
                monsterLevel=1,
                remainCookies=0,
                remainMilk=0
            },

        };
    }
}
