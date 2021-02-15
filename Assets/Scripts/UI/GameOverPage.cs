using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPage : MonoBehaviour
{
    private Text tex_RoundCount;
    private Text tex_TotalCount;
    private Text tex_CurrentLevel;

    private void Awake()
    {
        tex_RoundCount = transform.Find("Tex_RoundCount").GetComponent<Text>();
        tex_TotalCount = transform.Find("Tex_TotalCount").GetComponent<Text>();
        tex_CurrentLevel = transform.Find("Tex_CurrentLevel").GetComponent<Text>();
    }

    private void OnEnable()
    {
        int totalNum = GameController.instance.level.totalWaveNum;
        tex_TotalCount.text = totalNum.ToString();
        tex_CurrentLevel.text = (GameController.instance.currentMap.mapID + (GameController.instance.currentMap.levelID - 1) * 5).ToString();
        int waveNum = GameController.instance.level.currentWave + 1;
        string waveStr = "";
        if (waveNum < 10)
        {
            waveStr = "0  " + waveNum.ToString();
        }
        else
        {
            waveStr = (waveNum / 10).ToString() + "  " + (waveNum % 10).ToString();
        }
        tex_RoundCount.text = waveStr;
    }
}
