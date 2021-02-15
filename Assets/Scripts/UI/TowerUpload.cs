using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpload : MonoBehaviour
{
    private int price;
    private Button button;
    private Text text;
    private Image image;
    private Sprite canUpLevelSprite;
    private Sprite cantUpLevelSprite;
    private Sprite reachHighestLevel;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        canUpLevelSprite = GameController.instance.GetSprite("NormalMordel/Game/Tower/Btn_CanUpLevel");
        cantUpLevelSprite = GameController.instance.GetSprite("NormalMordel/Game/Tower/Btn_CantUpLevel");
        reachHighestLevel = GameController.instance.GetSprite("NormalMordel/Game/Tower/Btn_ReachHighestLevel");
        text = transform.Find("Text").GetComponent<Text>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (text == null)
        {
            return;
        }
        UpdateUIView();
    }

    private void UpdateUIView()
    {
        if (GameController.instance.selectGrid.towerProperty.towerLevel >= 3)
        {
            image.sprite = reachHighestLevel;
            button.interactable = false;
            text.enabled = false;
        }
        else
        {
            text.enabled = true;
            price = GameController.instance.selectGrid.towerProperty.upLoadPrice;
            text.text = price.ToString();
            if (GameController.instance.coin >= price)
            {
                image.sprite = canUpLevelSprite;
                button.interactable = true;
            }
            else
            {
                image.sprite = cantUpLevelSprite;
                button.interactable = false;
            }
        }
    }

    public void UploadTower()
    {
        GameController.instance.UploadTower();
    }
}
