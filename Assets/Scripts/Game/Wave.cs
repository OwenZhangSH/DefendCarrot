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
}
