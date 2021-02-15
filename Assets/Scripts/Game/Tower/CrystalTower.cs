using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalTower : TowerProperty
{
    private float distance;
    private float bulletWidth;
    private float bulletLength;
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        bulletGO = GameController.instance.GetGameObjectResource(
            "Tower/ID" + tower.towerID.ToString() + "/bullet/" + towerLevel.ToString());
        bulletGO.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GameController.instance.GetAudioClip("NormalMordel/Tower/Attack/" + tower.towerID.ToString());
    }

    private void OnEnable()
    {
        if (animator == null) return;
        bulletGO = GameController.instance.GetGameObjectResource(
            "Tower/ID" + tower.towerID.ToString() + "/bullet/" + towerLevel.ToString());
        bulletGO.SetActive(false);
    }

    protected override void Update()
    {
        if (GameController.instance.isPause || targetTrans == null || GameController.instance.gameOver)
        {
            if (targetTrans == null)
            {
                bulletGO.SetActive(false);
            }
            return;
        }
        Attack();
    }

    protected override void Attack()
    {
        if (targetTrans == null)
        {
            return;
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        animator.Play("Attack");
        if (targetTrans.gameObject.tag == "Item")
        {
            distance = Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 3));
        }
        else
        {
            distance = Vector3.Distance(transform.position, targetTrans.position);
        }
        bulletWidth = 3 / distance;
        bulletLength = distance / 2;
        if (bulletWidth <= 0.5f)
        {
            bulletWidth = 0.5f;
        }
        else if (bulletWidth >= 1)
        {
            bulletWidth = 1;
        }
        bulletGO.transform.position = new Vector3((targetTrans.position.x + transform.position.x) / 2, (targetTrans.position.y + transform.position.y) / 2, 0);
        bulletGO.transform.localScale = new Vector3(1, bulletWidth, bulletLength);
        bulletGO.SetActive(true);
        bulletGO.GetComponent<Bullet>().targetTrans = targetTrans;
    }

    protected override void DestoryTower()
    {
        bulletGO.SetActive(false);
        GameController.instance.PushGameObjectToFactory(
            "Tower/ID" + tower.towerID.ToString() + "/bullet/" + towerLevel.ToString(), bulletGO);
        bulletGO = null;
        base.DestoryTower();
    }
}
