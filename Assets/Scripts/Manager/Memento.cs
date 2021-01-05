using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memento
{
    private PlayerManager playerManager;
    
    // 构造函数
    public Memento(PlayerManager pm)
    {
        playerManager = pm;
    }

    public PlayerManager GetPlayerManager()
    {
        return playerManager;
    }
}
