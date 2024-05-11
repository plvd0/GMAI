using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public float helpInterval = 5f;
    private float timer;
    private SphereCollider helpRadius;

    void Start()
    {
        helpRadius = GetComponent<SphereCollider>();
        helpRadius.enabled = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= helpInterval)
        {
            timer = 0;
            CallForHelp();
        }
    }

    private void CallForHelp()
    {
        helpRadius.enabled = true;
        Debug.Log("Bot is calling for help!");
        Invoke("DisableCallForHelp", 2f);
    }

    void DisableCallForHelp()
    {
        helpRadius.enabled = false;
    }
}
