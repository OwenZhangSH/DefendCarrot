using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinPage : MonoBehaviour
{
    private Text tex_RoundCount;
    private Text tex_TotalCount;
    private Text tex_CurrentLevel;
    private Image img_Carrot;
    public Sprite[] carrotSprites;//0.金 1.银 2.铜
    // Start is called before the first frame update
    private void Awake()
    {
        tex_RoundCount = transform.Find("Tex_RoundCount").GetComponent<Text>();
        tex_TotalCount = transform.Find("Tex_TotalCount").GetComponent<Text>();
        tex_CurrentLevel = transform.Find("Tex_CurrentLevel").GetComponent<Text>();
        img_Carrot = transform.Find("Img_Carrot").GetComponent<Image>();
        carrotSprites = new Sprite[3];
        for (int i = 0; i < 3; i++)
        {
            carrotSprites[i] = GameController.instance.GetSprite("GameOption/Normal/Level/Carrot_" + (i + 1).ToString());
        }
    }

    private void OnEnable()
    {
        int totalNum = GameController.instance.level.totalWaveNum;
        tex_TotalCount.text = totalNum.ToString();
        tex_CurrentLevel.text = (GameController.instance.currentMap.mapID + (GameController.instance.currentMap.levelID - 1) * 5).ToString();
        int waveNum = totalNum;
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
        if (GameController.instance.carrotHp >= 4)
        {
            img_Carrot.sprite = carrotSprites[0];
        }
        else if (GameController.instance.carrotHp >= 2)
        {
            img_Carrot.sprite = carrotSprites[1];
        }
        else
        {
            img_Carrot.sprite = carrotSprites[2];
        }
    }
}
