using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchRescueState : FBState
{
    private NavMeshAgent agent;

    public SearchRescueState(FireBotFSM botController) : base(botController)  
    {
        agent = botController.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        Debug.Log("Entering SEARCH & RESCUE");
        botController.UpdateDisplayWithDelay("SEARCH-AND-RESCUE: Fire-Bot is now searching for Bots.", 0.1f);
    }

    public override void Execute()
    {
        if (agent.destination != botController.botPos)
        {
            agent.ResetPath();
            agent.destination = botController.botPos;
            agent.stoppingDistance = 1.0f;
        }

        if (!agent.pathPending)
        {
            float adjustedDistance = agent.remainingDistance - agent.stoppingDistance;
            if (adjustedDistance <= 0 && agent.velocity.sqrMagnitude < 0.1f)
            {
                Debug.Log("Bot has reached the help target. Transitioning to Evacuation Support State.");
                botController.ChangeState(botController.evacuationSupportState);
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited SEARCH & RESCUE state");
    }
}
