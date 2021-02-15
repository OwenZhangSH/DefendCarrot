using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShitBullet : Bullet
{
    private BullectProperty bullectProperty;

    void Start()
    {
        bullectProperty = new BullectProperty
        {
            debuffTime = towerLevel * 0.4f,
            debuffValue = towerLevel * 0.3f
        };
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag == "Monster")
        {
            collision.SendMessage("DecreaseSpeed", bullectProperty);
        }
        base.OnTriggerEnter2D(collision);
    }
}
