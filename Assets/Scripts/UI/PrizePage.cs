using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizePage : MonoBehaviour
{
    private Image img_Prize;
    private Image img_Instruction;
    private Text tex_PrizeName;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        img_Prize = transform.Find("Img_Prize").GetComponent<Image>();
        img_Instruction = transform.Find("Img_Instruction").GetComponent<Image>();
        tex_PrizeName = transform.Find("Tex_PrizeName").GetComponent<Text>();
    }

    private void OnEnable()
    {
        string prizeName = "";
        // 判断是否会产生
        int randomNum = Random.Range(1, 4);
        if (randomNum >= 4 && GameManager.instance.playerManager.monsterPetDataList.Count < 3)
        {
            int randomEggNum = Random.Range(1, 4);
            while (HasThePet(randomEggNum))
            {
                randomEggNum = Random.Range(1, 4);
            }
            MonsterPetData monsterPetData = new MonsterPetData
            {
                monsterLevel = 1,
                remainCookies = 0,
                remainMilk = 0,
                monsterID = randomEggNum
            };
            GameManager.instance.playerManager.monsterPetDataList.Add(monsterPetData);
            prizeName = "宠物蛋";
        }
        else
        {

            switch (randomNum)
            {
                case 1:
                    prizeName = "牛奶";
                    GameManager.instance.playerManager.milk += 20;
                    break;
                case 2:
                    prizeName = "饼干";
                    GameManager.instance.playerManager.cookies += 20;
                    break;
                case 3:
                    prizeName = "窝";
                    GameManager.instance.playerManager.nest += 1;
                    break;
                default:
                    break;
            }
        }
        tex_PrizeName.text = prizeName;
        img_Instruction.sprite = GameController.instance.GetSprite("MonsterNest/Prize/Instruction" + randomNum.ToString());
        img_Prize.sprite = GameController.instance.GetSprite("MonsterNest/Prize/Prize" + randomNum.ToString());
        animator.Play("Enter");
    }

    private bool HasThePet(int monsterID)
    {
        for (int i = 0; i < GameManager.instance.playerManager.monsterPetDataList.Count; i++)
        {
            if (GameManager.instance.playerManager.monsterPetDataList[i].monsterID == monsterID)
            {
                return true;
            }
        }
        return false;
    }
}
