﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoinMove : MonoBehaviour
{
    private Text coinText;
    private Image coinImage;
    public Sprite[] coinSprites;
    [HideInInspector]
    public int coin;

    private void Awake()
    {
        coinText = transform.Find("Text").GetComponent<Text>();
        coinImage = transform.Find("Image").GetComponent<Image>();
        coinSprites = new Sprite[2];
        coinSprites[0] = GameController.instance.GetSprite("NormalMordel/Game/Coin");
        coinSprites[1] = GameController.instance.GetSprite("NormalMordel/Game/ManyCoin");
        ShowCoin();
    }

    private void OnEnable()
    {
        ShowCoin();
    }

    private void ShowCoin()
    {
        //数额
        coinText.text = coin.ToString();
        //图片显示
        if (coin >= 500)
        {
            coinImage.sprite = coinSprites[1];
        }
        else
        {
            coinImage.sprite = coinSprites[0];
        }
        transform.DOLocalMoveY(60, 0.5f);
        DOTween.To(() => coinImage.color, toColor => coinImage.color = toColor, new Color(1, 1, 1, 0), 0.5f);
        Tween tween = DOTween.To(() => coinText.color, toColor => coinText.color = toColor, new Color(1, 1, 1, 0), 0.5f);
        tween.OnComplete(DestoryCoin);
    }

    //销毁金币UI
    private void DestoryCoin()
    {
        transform.localPosition = Vector3.zero;
        coinImage.color = coinText.color = new Color(1, 1, 1, 1);
        GameController.instance.PushGameObjectToFactory("CoinCanvas", transform.parent.gameObject);
    }
}
