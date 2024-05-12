using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExtinguishingState : FBState
{
    private FireManager fireManager;

    private float extinguishingTime = 10.0f;
    private float timer;

    private float helpInterval = 5.0f;
    private float helpTimer;

    public ExtinguishingState(FireBotFSM botController, FireManager fireManager) : base(botController)
    {
        this.fireManager = fireManager;
    }

    public override void Enter()
    {
        Debug.Log("ExtinguishingState initialized with fire position: " + fireManager.firePosition);
        botController.UpdateDisplayWithDelay("EXTINGUISHING: Fire-Bot is now fighting the fire.", 0.1f);
        timer = extinguishingTime;
        helpTimer = helpInterval;
    }

    public override void Execute()
    {
        if (botController.currentState != this) return;

        helpTimer -= Time.deltaTime;
        if (helpTimer <= 0)
        {
            CheckForHelp();
            helpTimer = helpInterval;
        }

        if (AtFirePos())
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                botController.SetDisplay($"EXTINGUISHING: {Mathf.Floor(timer)} seconds before Fire-Bot extinguishes the fire.");
            }
            else
            {
                GameObject.Destroy(fireManager.fire);
                botController.ChangeState(botController.postFireState);
            }
        }
        else
        {
            MoveToFire();
        }

    }

    public override void Exit()
    {
        Debug.Log("Exited EXTINGUISHING state");
    }

    private void MoveToFire()
    {
        NavMeshAgent agent = botController.GetComponent<NavMeshAgent>();
        if(agent != null)
        {
            Vector3 targetPos = fireManager.firePosition;
            agent.destination = targetPos;
            agent.stoppingDistance = 5.0f;
        }
    }

    private bool AtFirePos()
    {
        NavMeshAgent agent = botController.GetComponent<NavMeshAgent>();
        if (agent != null && !agent.pathPending)
        {
            Vector3 targetPos = fireManager.firePosition;
            float distance = Vector3.Distance(agent.transform.position, targetPos);

            if (distance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    private void CheckForHelp()
    {
        if (botController.inAssemblyArea)
        {
            return;
        }

        float detection = 25.0f;
        Collider[] collider = Physics.OverlapSphere(botController.transform.position, detection);

        foreach (var col in collider)
        {
            if (col.CompareTag("Bot"))
            {
                Debug.Log("HELP NEEDED");
                botController.botPos = col.transform.position;
                botController.ChangeState(botController.searchRescueState);
            }
        }
    }
}