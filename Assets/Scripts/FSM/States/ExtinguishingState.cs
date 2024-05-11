using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExtinguishingState : FBState
{
    private FireManager fireManager;

    private float extinguishingTime = 10.0f;
    private float timer;

    public ExtinguishingState(FireBotFSM botController, FireManager fireManager) : base(botController)
    {
        this.fireManager = fireManager;
    }

    public override void Enter()
    {
        Debug.Log("ExtinguishingState initialized with fire position: " + fireManager.firePosition);
        botController.SetDisplay("EXTINGUISHING: Fire-Bot is now fighting the fire.");
        timer = extinguishingTime;

        MoveToFire();
    }

    public override void Execute()
    {
        if (AtFirePos())
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                botController.SetDisplay($"EXTINGUISHING: {timer} before Fire-Bot extinguishes it.");
            }
            else
            {
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
}