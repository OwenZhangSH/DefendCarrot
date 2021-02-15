using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellTowerButton : MonoBehaviour
{
    private int price;
    private Button button;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        text = transform.Find("Text").GetComponent<Text>();
    }

    private void OnEnable()
    {
        if (text == null)
        {
            return;
        }
        price = GameController.instance.selectGrid.towerProperty.sellPrice;
        text.text = price.ToString();
    }

    // Update is called once per frame
    public void SellTower()
    {
        GameController.instance.SellTower();
    }
}
