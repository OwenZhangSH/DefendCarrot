using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEffect : MonoBehaviour
{
    public int attackValue;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            collision.SendMessage("TakeDamage", attackValue);
        }
    }
}
