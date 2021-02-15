using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    [System.Serializable]
    public struct WaveInfo
    {
        public int[] monsterIDList;
    }

    public WaveInfo waveInfo;
    protected Wave nextWave;
    protected int waveID;

    public Wave(int[] monsterIDList, int waveID)
    {
        waveInfo.monsterIDList = monsterIDList;
        this.waveID = waveID;
    }

    public void SetNextWave(Wave wave)
    {
        nextWave = wave;
    }

    public void Handle(int waveID)
    {
        if (this.waveID < waveID)
        {
            nextWave.Handle(waveID);
        }
        else
        {
            //产生怪物
            GameController.instance.monsterIDList = waveInfo.monsterIDList;
            GameController.instance.isCreatingMonster = false;
        }
    }
}
