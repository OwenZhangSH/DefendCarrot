using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    public int towerID;
    public int price;
    private Button button;
    private Sprite canClickSprite;
    private Sprite cantClickSprite;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (price == 0)
        {
            return;
        }
        UpdateIcon();
    }

    private void Start()
    {
        canClickSprite = GameController.instance.GetSprite("NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick1");
        cantClickSprite = GameController.instance.GetSprite("NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick0");
        UpdateIcon();
        price = GameController.instance.towerPriceDict[towerID];
    }

    //更新图标
    private void UpdateIcon()
    {
        if (GameController.instance.coin > price)
        {
            image.sprite = canClickSprite;
            button.interactable = true;
        }
        else
        {
            image.sprite = cantClickSprite;
            button.interactable = false;
        }
    }

    public void BuildTower() {
        GameController.instance.BuildTower(towerID);
    }
}
