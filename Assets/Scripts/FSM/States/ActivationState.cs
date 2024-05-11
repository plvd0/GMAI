using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActivationState : FBState
{
    public ActivationState(FireBotFSM botController) : base(botController) {}

    public override void Enter()
    {
        botController.SetDisplay("ACTIVATION: Fire-Bot has detected fire, enroute to scene.");
        NavMeshAgent agent = botController.fireBot.GetComponent<NavMeshAgent>();
        MoveToScene(botController.fireBot, botController.sceneDestination.transform.position);
    }

    public override void Execute()
    {
        Vector3 fireBotTargetPos = botController.sceneDestination.transform.position;

        if (!AtScene(botController.fireBot, fireBotTargetPos))
        {
            MoveToScene(botController.fireBot, fireBotTargetPos);
        }
        else
        {
            botController.ChangeState(botController.assessingState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited ACTIVATION state");
    }

    private void MoveToScene(GameObject fireObj, Vector3 targetPos)
    {
        NavMeshAgent agent = fireObj.GetComponent<NavMeshAgent>();
        if (agent != null) 
        {
            agent.destination = targetPos;
        }
    }

    private bool AtScene(GameObject fireObj, Vector3 targetPos)
    {
        NavMeshAgent agent = fireObj.GetComponent<NavMeshAgent>();
        if (agent == null) return false;

        float distanceToTarget = Vector3.Distance(agent.transform.position, targetPos);

        if (!agent.pathPending && agent.remainingDistance <= 0.1f && agent.velocity.sqrMagnitude < 0.01f)
        {
            return true;
        }
        return false;
    }
}
