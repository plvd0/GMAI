using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExtinguishingState : FBState
{
    private FireManager fireManager;

    private float extinguishingTime = 10.0f;
    private float timer;
    private Vector3 firePosition;

    public ExtinguishingState(FireBotFSM botController, FireManager fireManager) : base(botController)
    {
        this.firePosition = fireManager.firePosition;
    }

    public override void Enter()
    {
        botController.SetDisplay("EXTINGUISHING: Fire-Bot is now fighting the fire.");
        timer = extinguishingTime;

        MoveToFire();
    }

    public override void Execute()
    {
        if (AtFirePos())
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
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
            agent.destination = firePosition;
        }
    }

    private bool AtFirePos()
    {
        NavMeshAgent agent = botController.GetComponent<NavMeshAgent>();
        if (agent != null && !agent.pathPending)
        {
            if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.1f))
            {
                return true;
            }
        }
        return false;
    }
}
