using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    public GridPoint gridPoint;
    public int itemID;
    private int coins;//销毁金币数额
    private int HP;
    private int currentHP;
    private Slider slider;

    private float timeVal;//显示或隐藏血条的计时器
    private bool showHp;

    private void OnEnable()
    {
        if(itemID != 0)
        {
            InitItem();
        }
    }

    private void Start()
    {
        slider = transform.Find("ItemCanvas").Find("HpSlider").GetComponent<Slider>();
        InitItem();
        slider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (showHp)
        {
            if (timeVal <= 0)
            {
                slider.gameObject.SetActive(false);
                showHp = false;
                timeVal = 3;
            }
            else
            {
                timeVal -= Time.deltaTime;
            }
        }
    }

    private void InitItem()
    {
        coins = 1000 - 100 * itemID;
        HP = 1500 - 100 * itemID;
        currentHP = HP;
        slider.value = (float)currentHP / HP;
        slider.gameObject.SetActive(false);
        timeVal = 3;
    }

    private void TakeDamage(int attackValue)
    {
        slider.gameObject.SetActive(true);
        currentHP -= attackValue;
        if (currentHP <= 0)
        {
            DestoryItem();
            return;
        }
        slider.value = (float)currentHP / HP;
        showHp = true;
        timeVal = 3;
    }

    private void DestoryItem()
    {
        if (GameController.instance.targetTrans == transform)
        {
            GameController.instance.HideSignal();
        }

        //金币奖励
        GameObject coinGO = GameController.instance.GetGameObjectResource("CoinCanvas");
        coinGO.transform.Find("Emp_Coin").GetComponent<CoinMove>().coin = coins;
        coinGO.transform.SetParent(GameController.instance.transform);
        coinGO.transform.position = transform.position;
        GameController.instance.ChangeCoin(coins);

        //销毁特效
        GameObject effectGo = GameController.instance.GetGameObjectResource("DestoryEffect");
        effectGo.transform.SetParent(GameController.instance.transform);
        effectGo.transform.position = transform.position;

        GameController.instance.PushGameObjectToFactory(
            GameController.instance.mapMaker.levelID.ToString() + "/Item/" + itemID, gameObject);

        gridPoint.gridState.hasItem = false;
        InitItem();

        GameController.instance.PlayEffectMusic("NormalMordel/Item");
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (GameController.instance.targetTrans == null)
        {
            GameController.instance.ChangeTarget(transform);
        }
        else if (GameController.instance.targetTrans != transform)
        {
            GameController.instance.ChangeTarget(transform);
        }
        else if (GameController.instance.targetTrans == transform)
        {
            GameController.instance.CancelTarget();
        }
    }
}
