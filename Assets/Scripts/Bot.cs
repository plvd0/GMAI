using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public float helpRange = 25f;
    public float callInterval = 3f;

    void Start()
    {
        StartCallForHelp();
    }

    public void StartCallForHelp()
    {
        InvokeRepeating("CallForHelp", 0, callInterval);
    }

    public void StopCallForHelp()
    {
        CancelInvoke("CallForHelp");
    }

    public void CallForHelp()
    {
        Debug.Log("CALLING FOR HELP");
        foreach(Collider collider in Physics.OverlapSphere(transform.position, helpRange))
        {
            if (collider.CompareTag("FireBot"))
            {
                FireBotFSM fireBot = collider.GetComponent<FireBotFSM>();
                if (fireBot != null)
                {
                    fireBot.HearHelp(transform.position);
                }
            }
        }
    }

    public Vector3 GetCurrentPos()
    {
        Debug.Log("Bot position: " + transform.position);
        return transform.position;
    }
}
