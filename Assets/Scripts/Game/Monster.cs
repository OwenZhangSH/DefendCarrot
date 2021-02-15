using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    // 属性
    public int monsterID;
    public int HP; // 总血量
    public int currentHP; // 当前血量
    public float speed; // 默认速度
    public float currentSpeed; // 当前速度
    public int coin; // 奖励金钱
    // 引用
    private Animator animator;
    private Slider slider;
    private List<Vector3> monsterPathPoint;

    // 状态
    private int pathPointIndex = 1;
    private bool reachCarrot;//到达终点
    private bool isDecreasSpeed;//是否减速

    private float decreaseSpeedTimeVal;//减速计时器
    private float decreaseTime;//减速持续的具体时间
    //资源
    public AudioClip dieAudioClip;
    public RuntimeAnimatorController runtimeAnimatorController;
    private GameObject shitGO;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        slider = transform.Find("MonsterCanvas").Find("HPSlider").GetComponent<Slider>();
        slider.gameObject.SetActive(false);
        monsterPathPoint = GameController.instance.mapMaker.monsterPathPos;
        shitGO = transform.Find("Shit").gameObject;
    }

    private void OnEnable()
    {
        monsterPathPoint = GameController.instance.mapMaker.monsterPathPos;
        //怪物的转向
        if (pathPointIndex + 1 < monsterPathPoint.Count)
        {
            float xOffset = monsterPathPoint[0].x - monsterPathPoint[1].x;
            if (xOffset < 0)//右走
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (xOffset > 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        slider.gameObject.transform.eulerAngles = Vector3.zero;
    }

    private void Update()
    {
        if (GameController.instance.isPause) return;
        if (!reachCarrot)
        {
            // 移动
            transform.position = Vector3.Lerp(transform.position, monsterPathPoint[pathPointIndex],
                1 / Vector3.Distance(transform.position, monsterPathPoint[pathPointIndex])
                * Time.deltaTime * currentSpeed * GameController.instance.gameSpeed);
            // 更新目标
            if (Vector3.Distance(transform.position, monsterPathPoint[pathPointIndex]) <= 0.01f)
            {
                // 怪物的转向
                if (pathPointIndex + 1 < monsterPathPoint.Count)
                {
                    float xOffset = monsterPathPoint[pathPointIndex].x - monsterPathPoint[pathPointIndex + 1].x;
                    if (xOffset < 0)//右走
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (xOffset > 0)
                    {
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                }
                slider.gameObject.transform.eulerAngles = Vector3.zero;
                // 更新目标点
                pathPointIndex++;
                if (pathPointIndex >= monsterPathPoint.Count)
                {
                    reachCarrot = true;
                }
            }
        } else
        {
            DestoryMonster();
            GameController.instance.DecreaseHP();
        }
        // 判断是否被减速
        if (isDecreasSpeed)
        {
            decreaseSpeedTimeVal += Time.deltaTime;
        }
        // 判断是否减速到时间
        if (decreaseSpeedTimeVal >= decreaseTime)
        {
            CancelDecreaseDebuff();
            decreaseSpeedTimeVal = 0;
        }
    }

    private void DestoryMonster()
    {
        if (GameController.instance.targetTrans == transform)
        {
            GameController.instance.HideSignal();
        }
        if (!reachCarrot)//被玩家杀死的
        {
            //生成金币以及数目
            GameObject coinGo = GameController.instance.GetGameObjectResource("CoinCanvas");
            coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().coin = coin;
            coinGo.transform.SetParent(GameController.instance.transform);
            coinGo.transform.position = transform.position;
            //增加玩家的金币数量
            GameController.instance.ChangeCoin(coin);
            // 奖品掉落
            int randomNum = UnityEngine.Random.Range(0, 100);
            if (randomNum < 10)
            {
                GameObject prizeGo = GameController.instance.GetGameObjectResource("Prize");
                prizeGo.transform.position = transform.position - new Vector3(0, 0, 6);
                GameController.instance.PlayEffectMusic("NormalMordel/GiftCreate");
            }
        }
        // 播放特效
        GameObject effectGo = GameController.instance.GetGameObjectResource("DestoryEffect");
        effectGo.transform.SetParent(GameController.instance.transform);
        effectGo.transform.position = transform.position;
        // 更新数据
        GameController.instance.killedMonsterNum++;
        GameController.instance.killedMonsterTotalNum++;
        // GO放回资源池
        InitMonsterGo();
        GameController.instance.PushGameObjectToFactory("MonsterPrefab", gameObject);
    }

    // 初始化数据
    private void InitMonsterGo()
    {
        monsterID = 0;
        HP = 0;
        currentHP = 0;
        currentSpeed = 0;
        speed = 0;
        coin = 0;
        pathPointIndex = 1;
        dieAudioClip = null;
        reachCarrot = false;
        slider.value = 1;
        slider.gameObject.SetActive(false);

        transform.eulerAngles = new Vector3(0, 0, 0);
        decreaseSpeedTimeVal = 0;
        decreaseTime = 0;
        CancelDecreaseDebuff();
    }

    private void MonsterDie()
    {
        //播放死亡音效
        GameController.instance.PlayEffectMusic("NormalMordel/Monster/" + 
            GameController.instance.currentMap.levelID.ToString() + "/" + monsterID.ToString());
        DestoryMonster();
    }

    private void TakeDamage(int attackValue)
    {
        slider.gameObject.SetActive(true);
        currentHP -= attackValue;
        if (currentHP <= 0)
        {
            MonsterDie();
            return;
        }
        slider.value = (float)currentHP / HP;
    }

    /// <summary>
    /// 减速相关
    /// </summary>
    /// <param name="bullectProperty"></param>
    //减速buff的方法
    private void DecreaseSpeed(BullectProperty bullectProperty)
    {
        if (!isDecreasSpeed)
        {
            currentSpeed = currentSpeed - bullectProperty.debuffValue;
            shitGO.SetActive(true);
        }
        decreaseSpeedTimeVal = 0;
        isDecreasSpeed = true;
        decreaseTime = bullectProperty.debuffTime;
    }

    //用来取消减速buff的方法
    private void CancelDecreaseDebuff()
    {
        isDecreasSpeed = false;
        currentSpeed = speed;
        shitGO.SetActive(false);
    }

    //获取特异性属性的方法
    public void SetMonsterAnimator()
    {
        runtimeAnimatorController = GameController.instance.controllers[monsterID - 1];
        animator.runtimeAnimatorController = runtimeAnimatorController;
    }

    /// <summary>
    /// 点击集火
    /// </summary>
    private void OnMouseDown()
    {
        if (GameController.instance.targetTrans == null)
        {
            GameController.instance.ChangeTarget(transform);
        }
        //转换新目标
        else if (GameController.instance.targetTrans != transform)
        {
            GameController.instance.ChangeTarget(transform);
        }
        //两次点击的是同一个目标
        else if (GameController.instance.targetTrans == transform)
        {
            GameController.instance.CancelTarget();
        }
    }
}
