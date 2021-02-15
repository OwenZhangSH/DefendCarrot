using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MonsterPetData
{
    public int monsterLevel;
    public int remainCookies;
    public int remainMilk;
    public int monsterID;
}
/// <summary>
/// 管理怪物宠物
/// </summary>
public class MonsterPet : MonoBehaviour
{
    // 宠物数据
    public MonsterPetData monsterPetData;

    // 引用
    private GameObject[] monsterLevelGO;//宠物对应的三个等级的游戏物体
    public Sprite[] btnSprites;//0.可用milk 1.不可用milk 2 3
    // egg
    private GameObject img_InstructionGO;
    //Baby
    private GameObject emp_FeedGO;
    private Text tex_milk;
    private Text tex_cookie;
    private Button btn_Milk;
    private Button btn_Cookie;
    private Image img_btn_Milk;
    private Image img_btn_Cookie;
    //Normal
    private GameObject img_TalkRightGO;
    private GameObject img_TalkLeftGO;

    public MonsterNestPanel monsterNestPanel;

    private void Awake()
    {
        // 游戏物体
        monsterLevelGO = new GameObject[3];
        monsterLevelGO[0] = transform.Find("Emp_Egg").gameObject;
        monsterLevelGO[1] = transform.Find("Emp_Baby").gameObject;
        monsterLevelGO[2] = transform.Find("Emp_Normal").gameObject;
        //Egg
        img_InstructionGO = monsterLevelGO[0].transform.Find("Img_Instruction").gameObject;
        img_InstructionGO.SetActive(false);
        // Baby
        emp_FeedGO = monsterLevelGO[1].transform.Find("Emp_Feed").gameObject;
        emp_FeedGO.SetActive(false);
        btn_Milk = monsterLevelGO[1].transform.Find("Emp_Feed").Find("Btn_Milk").GetComponent<Button>();
        img_btn_Milk = monsterLevelGO[1].transform.Find("Emp_Feed").Find("Btn_Milk").GetComponent<Image>();
        btn_Cookie = monsterLevelGO[1].transform.Find("Emp_Feed").Find("Btn_Cookie").GetComponent<Button>();
        img_btn_Cookie = monsterLevelGO[1].transform.Find("Emp_Feed").Find("Btn_Cookie").GetComponent<Image>();
        tex_milk = monsterLevelGO[1].transform.Find("Emp_Feed").Find("Btn_Milk").Find("Text").GetComponent<Text>();
        tex_cookie = monsterLevelGO[1].transform.Find("Emp_Feed").Find("Btn_Cookie").Find("Text").GetComponent<Text>();
        //Normal
        img_TalkLeftGO = transform.Find("Img_TalkLeft").gameObject;
        img_TalkRightGO = transform.Find("Img_TalkRight").gameObject;
    }

    private void OnEnable()
    {
        InitMonsterPet();
    }

    //初始化宠物
    public void InitMonsterPet()
    {
        if (monsterPetData.remainMilk == 0)
        {
            monsterPetData.remainMilk = monsterPetData.monsterID * 60;
        }
        if (monsterPetData.remainCookies == 0)
        {
            monsterPetData.remainCookies = monsterPetData.monsterID * 30;
        }
        ShowMonster();
    }

    //显示宠物
    private void ShowMonster()
    {
        for (int i = 0; i < monsterLevelGO.Length; i++)
        {
            monsterLevelGO[i].SetActive(false);
            if ((i + 1) == monsterPetData.monsterLevel)
            {
                monsterLevelGO[i].SetActive(true);
                Sprite petSprite = null;
                switch (monsterPetData.monsterLevel)
                {
                    case 1:
                        petSprite = GameManager.instance.GetSprite("MonsterNest/Monster/Egg/" + monsterPetData.monsterID.ToString());
                        break;
                    case 2:
                        petSprite = GameManager.instance.GetSprite("MonsterNest/Monster/Baby/" + monsterPetData.monsterID.ToString());
                        break;
                    case 3:
                        petSprite = GameManager.instance.GetSprite("MonsterNest/Monster/Normal/" + monsterPetData.monsterID.ToString());
                        break;
                    default:
                        break;
                }
                Image monsterImage = monsterLevelGO[i].transform.Find("Img_Pet").GetComponent<Image>();
                monsterImage.sprite = petSprite;
                monsterImage.SetNativeSize();
                float imageScale = 0;
                if (monsterPetData.monsterLevel == 1)
                {
                    imageScale = 2;
                }
                else
                {
                    imageScale = 1 + (monsterPetData.monsterLevel - 1) * 0.5f;
                }

                monsterImage.transform.localScale = new Vector3(imageScale, imageScale, 1);
            }
        }
    }

    /// <summary>
    /// 相关事件
    /// </summary>
    public void ClickPet()
    {
        GameManager.instance.audioManager.PlayEffectMusic(
            GameManager.instance.GetAudioClip("MonsterNest/PetSound" + monsterPetData.monsterLevel.ToString()));
        switch (monsterPetData.monsterLevel)
        {
            case 1:
                if (GameManager.instance.playerManager.nest >= 1)
                {
                    GameManager.instance.playerManager.nest--;
                    //升级 更新显示
                    Upload();
                    monsterPetData.monsterLevel++;
                    ShowMonster();
                    monsterNestPanel.UpdateText();
                }
                else
                {
                    img_InstructionGO.SetActive(true);
                    Invoke("CloseTalkUI", 2);
                }

                break;
            case 2:
                if (emp_FeedGO.activeSelf)
                {
                    emp_FeedGO.SetActive(false);
                }
                else
                {
                    emp_FeedGO.SetActive(true);
                    if (GameManager.instance.playerManager.milk == 0)
                    {
                        img_btn_Milk.sprite = btnSprites[1];
                        btn_Milk.interactable = false;
                    }
                    else
                    {
                        img_btn_Milk.sprite = btnSprites[0];
                        btn_Milk.interactable = true;
                    }
                    if (GameManager.instance.playerManager.cookies == 0)
                    {
                        img_btn_Cookie.sprite = btnSprites[3];
                        btn_Cookie.interactable = false;
                    }
                    else
                    {
                        img_btn_Cookie.sprite = btnSprites[2];
                        btn_Cookie.interactable = true;
                    }
                    if (monsterPetData.remainMilk == 0)
                    {
                        btn_Milk.gameObject.SetActive(false);
                    }
                    else
                    {
                        tex_milk.text = monsterPetData.remainMilk.ToString();
                        btn_Milk.gameObject.SetActive(true);
                    }
                    if (monsterPetData.remainCookies == 0)
                    {
                        btn_Cookie.gameObject.SetActive(false);
                    }
                    else
                    {
                        tex_cookie.text = monsterPetData.remainCookies.ToString();
                        btn_Cookie.gameObject.SetActive(true);
                    }
                }
                break;
            case 3:
                int randomNum = Random.Range(0, 2);
                if (randomNum == 1)
                {
                    img_TalkRightGO.SetActive(true);
                    Invoke("CloseTalkUI", 2);
                }
                else
                {
                    img_TalkLeftGO.SetActive(true);
                    Invoke("CloseTalkUI", 2);
                }
                break;
            default:
                break;
        }
    }



    //关闭对话框
    private void CloseTalkUI()
    {
        img_InstructionGO.SetActive(false);
        img_TalkRightGO.SetActive(false);
        img_TalkLeftGO.SetActive(false);
    }

    //升级
    private void Upload()
    {

        if (monsterPetData.remainMilk == 0 && monsterPetData.remainCookies == 0)
        {
            GameManager.instance.audioManager.PlayEffectMusic(GameManager.instance.GetAudioClip("MonsterNest/PetChange"));
            monsterPetData.monsterLevel++;
            if (monsterPetData.monsterLevel >= 3)
            {
                GameManager.instance.playerManager.unlockedNormalModeMapList[monsterPetData.monsterID * 5 - 1].isUnlocked = true;
                GameManager.instance.playerManager.burriedLevelNum++;
                ShowMonster();
            }
            else
            {
                ShowMonster();
            }
        }
        SaveMonsterData();
    }

    private void SaveMonsterData()
    {
        for (int i = 0; i < GameManager.instance.playerManager.monsterPetDataList.Count; i++)
        {
            if (GameManager.instance.playerManager.monsterPetDataList[i].monsterID == monsterPetData.monsterID)
            {
                GameManager.instance.playerManager.monsterPetDataList[i] = monsterPetData;
            }
        }
    }

    //喂牛奶
    public void FeedMilk()
    {
        //播放喂养动画与音效
        GameManager.instance.audioManager.PlayEffectMusic(GameManager.instance.GetAudioClip("MonsterNest/Feed01"));
        GameObject heartGO = GameManager.instance.GetGameObjectResource(FactoryType.UIFactory ,"Img_Heart");
        heartGO.transform.position = transform.position;
        monsterNestPanel.SetCanvasTrans(heartGO.transform);
        if (GameManager.instance.playerManager.milk >= monsterPetData.remainMilk)
        {
            GameManager.instance.playerManager.milk -= monsterPetData.remainMilk;
            monsterPetData.remainMilk = 0;
            //更新文本
            monsterNestPanel.UpdateText();
        }
        else
        {
            monsterPetData.remainMilk -= GameManager.instance.playerManager.milk;
            GameManager.instance.playerManager.milk = 0;
            btn_Milk.gameObject.SetActive(false);
        }
        emp_FeedGO.SetActive(false);
        Invoke("Upload", 0.433f);
    }

    //喂饼干
    public void FeedCookie()
    {
        //播放喂养动画与音效
        GameManager.instance.audioManager.PlayEffectMusic(GameManager.instance.GetAudioClip("MonsterNest/Feed02"));
        GameObject heartGO = GameManager.instance.GetGameObjectResource(FactoryType.UIFactory, "Img_Heart");
        heartGO.transform.position = transform.position;
        monsterNestPanel.SetCanvasTrans(heartGO.transform);
        if (GameManager.instance.playerManager.cookies >= monsterPetData.remainCookies)
        {

            GameManager.instance.playerManager.cookies -= monsterPetData.remainCookies;
            monsterPetData.remainCookies = 0;
            //更新文本
            monsterNestPanel.UpdateText();

        }
        else
        {
            monsterPetData.remainCookies -= GameManager.instance.playerManager.cookies;
            GameManager.instance.playerManager.cookies = 0;
            btn_Cookie.gameObject.SetActive(false);
        }
        emp_FeedGO.SetActive(false);
        Invoke("Upload", 0.433f);
    }
}
