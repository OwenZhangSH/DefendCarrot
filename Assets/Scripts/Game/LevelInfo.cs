using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo 
{
    public int levelID;
    public int mapID;

    public List<GridPoint.GridState> gridPoints;
    public List<GridPoint.GridIndex> monsterPath;
    public List<Wave.WaveInfo> waveInfo;
}
