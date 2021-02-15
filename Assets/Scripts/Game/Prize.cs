using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameController.instance.PlayEffectMusic("NormalMordel/GiftGot");
        GameController.instance.isPause = true;
        GameController.instance.normalModepanel.ShowPrizePage();
        GameController.instance.PushGameObjectToFactory("Prize", gameObject);
    }
}
