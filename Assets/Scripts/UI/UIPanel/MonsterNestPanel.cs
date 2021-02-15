using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MonsterNestPanel : BasePanel
{
    // 引用
    private GameObject shopPageGO;
    private Text tex_Cookie;
    private Text tex_Milk;
    private Text tex_Nest;
    private Text tex_Diamand;
    private List<GameObject> monsterPetGOList;
    private Transform Emp_MonsterGroupTrans;

    protected override void Awake()
    {
        base.Awake();
        shopPageGO = transform.Find("ShopPage").gameObject;
        tex_Cookie = transform.Find("Img_TopPage").Find("Tex_Cookies").GetComponent<Text>();
        tex_Milk = transform.Find("Img_TopPage").Find("Tex_Milk").GetComponent<Text>();
        tex_Nest = transform.Find("Img_TopPage").Find("Tex_Nest").GetComponent<Text>();
        tex_Diamand = transform.Find("ShopPage").Find("Img_Diamands").Find("Tex_Diamands").GetComponent<Text>();
        Emp_MonsterGroupTrans = transform.Find("Emp_MonsterGroup");
        //for (int i = 1; i < 4; i++)
        //{
        //    mUIFacade.GetSprite("MonsterNest/Monster/Egg/" + i.ToString());
        //    mUIFacade.GetSprite("MonsterNest/Monster/Baby/" + i.ToString());
        //    mUIFacade.GetSprite("MonsterNest/Monster/Normal/" + i.ToString());
        //}
        monsterPetGOList = new List<GameObject>();
    }

    public override void InitPanel()
    {
        base.InitPanel();
        for (int i = 0; i < monsterPetGOList.Count; i++)
        {
            uiManager.PushGameObjectToFactory(FactoryType.UIFactory, "Emp_Monsters", monsterPetGOList[i]);
        }
        monsterPetGOList.Clear();
        // 生成新的Monster Pet
        for (int i = 0; i < uiManager.playerManager.monsterPetDataList.Count; i++)
        {
            if (uiManager.playerManager.monsterPetDataList[i].monsterID != 0)
            {
                GameObject monsterPetGo = uiManager.GetGameObjectResource(FactoryType.UIFactory, "Emp_Monsters");
                monsterPetGo.GetComponent<MonsterPet>().monsterPetData = uiManager.playerManager.monsterPetDataList[i];
                monsterPetGo.GetComponent<MonsterPet>().monsterNestPanel = this;
                monsterPetGo.GetComponent<MonsterPet>().InitMonsterPet();
                monsterPetGo.transform.SetParent(Emp_MonsterGroupTrans);
                monsterPetGo.transform.localScale = Vector3.one;
                monsterPetGOList.Add(monsterPetGo);
            }
        }
        UpdateText();
    }
    // 打开商店页面
    public void ShowShopPage()
    {
        shopPageGO.SetActive(true);
    }

    // 关闭商店页面
    public void CloseShopPage()
    {
        shopPageGO.SetActive(false);
    }
    // 回到主页面
    public void ReturnToMain()
    {
        uiManager.ChangeSceneState(new MainSceneState(uiManager));
    }

    // Shop
    public void BuyNest()
    {
        if (GameManager.instance.playerManager.diamands >= 60)
        {
            GameManager.instance.playerManager.diamands -= 60;
        }
        GameManager.instance.playerManager.nest++;
        UpdateText();
    }

    public void BuyMilk()
    {
        if (GameManager.instance.playerManager.diamands >= 1)
        {
            GameManager.instance.playerManager.diamands -= 1;
        }
        GameManager.instance.playerManager.milk += 10;
        UpdateText();
    }

    public void BuyCookie()
    {
        if (GameManager.instance.playerManager.diamands >= 10)
        {
            GameManager.instance.playerManager.diamands -= 10;
        }
        GameManager.instance.playerManager.cookies += 15;
        UpdateText();
    }

    //更新文本
    public void UpdateText()
    {
        tex_Cookie.text = GameManager.instance.playerManager.cookies.ToString();
        tex_Milk.text = GameManager.instance.playerManager.milk.ToString();
        tex_Nest.text = GameManager.instance.playerManager.nest.ToString();
        tex_Diamand.text = GameManager.instance.playerManager.diamands.ToString();
    }

    public void SetCanvasTrans(Transform uiTrans)
    {
        uiTrans.SetParent(uiManager.canvasTransform);
    }
}
