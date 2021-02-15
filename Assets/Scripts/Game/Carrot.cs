using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carrot : MonoBehaviour
{
    // 萝卜状态图片
    private Sprite[] carrotSprites;
    private Animator animator;
    private SpriteRenderer sr;
    private Text text;
    // Start is called before the first frame update
    void Awake()
    {
        carrotSprites = new Sprite[7];
        for (int i = 0; i < carrotSprites.Length; i++)
        {
            carrotSprites[i] = GameController.instance.GetSprite("NormalMordel/Game/Carrot/" + i.ToString());
        }
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        text = transform.Find("HpCanvas").Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.carrotHp < 10)
        {
            animator.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (GameController.instance.carrotHp >= 10)
        {
            animator.SetTrigger("Touch");
            int randomNum = Random.Range(1, 4);
            GameController.instance.PlayEffectMusic("NormalMordel/Carrot/" + randomNum.ToString());
        }
    }

    public void UpdateCarrotUI()
    {
        int hp = GameController.instance.carrotHp;
        text.text = hp.ToString();
        if (hp >= 7 && hp < 10)
        {
            sr.sprite = carrotSprites[6];
        }
        else if (hp < 7 && hp > 0)
        {
            sr.sprite = carrotSprites[hp - 1];
        }
        else
        {
            //游戏结束
            GameController.instance.GameLose();
        }
    }
}
