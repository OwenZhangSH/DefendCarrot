using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int totalWaveNum;
    public Wave[] waveList;
    public int currentWave;

    public Level(int waveNum, List<Wave.WaveInfo> waveInfoList)
    {
        totalWaveNum = waveNum;
        waveList = new Wave[waveNum];
        for (int i = 0; i < waveNum; i++)
        {
            waveList[i] = new Wave(waveInfoList[i].monsterIDList, i);
        }

        for (int i = 0; i < waveNum - 1; i++)
        {
            waveList[i].SetNextWave(waveList[i+1]);
        }
    }

    public void HandleWave()
    {
        if (currentWave >= totalWaveNum)
        {
            //胜利
            currentWave--;
            GameController.instance.GameWin();
        }
        else if (currentWave == totalWaveNum - 1)
        {
            //最后一波怪的UI显示音乐播放
            GameController.instance.StartFinalWave();
        }
        else
        {
            waveList[currentWave].Handle(currentWave);
        }
    }

    public void AddWaveNum()
    {
        currentWave++;
    }

    //调用最后一回合的Handle方法
    public void HandleLastWave()
    {
        waveList[currentWave].Handle(currentWave);
    }
}
