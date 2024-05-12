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
        botController.UpdateDisplayWithDelay("SEARCH-AND-RESCUE: Fire-Bot is now searching for Bot calling for help.", 0.1f);
    }

    public override void Execute()
    {
        if (agent.destination != botController.botPos) // Checks if destination isn't already set to the Bot
        {
            agent.ResetPath(); // Resets current path to avoid existing navigation
            agent.destination = botController.botPos; // Sets new destination to the Bot
            agent.stoppingDistance = 1.0f; // Sets stopping distance
        }

        if (!agent.pathPending) // Checks if Fire-Bot has no pending navigation
        {
            float adjustedDistance = agent.remainingDistance - agent.stoppingDistance; // Calculates adjusted distance to target, account for stopping distance
            if (adjustedDistance <= 0 && agent.velocity.sqrMagnitude < 0.1f) // If adjusted distance is less than or equal to 0 & Fire-Bot has practically stopped moving
            {
                Debug.Log("Bot reached, evacuation state");
                botController.ChangeState(botController.evacuationSupportState); // Transitions to Evacuation Support state
            }
        }
    }

    public override void Exit() {}
}
