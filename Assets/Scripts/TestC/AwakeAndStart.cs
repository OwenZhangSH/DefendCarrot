using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeAndStart : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake");
        gameObject.SetActive(false);
        Invoke("SetToActive", 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");
    }

    void SetToActive()
    {
        gameObject.SetActive(true);
        Invoke("SetToNoActive", 1);
    }
    void SetToNoActive()
    {
        gameObject.SetActive(false);
        Invoke("SetToActive", 1);
    }
}
