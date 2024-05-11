using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchRescueState : FBState
{
    private NavMeshAgent agent;
    private Bot bot;

    public SearchRescueState(FireBotFSM botController, Bot bot) : base(botController) 
    {
        this.bot = bot;
        agent = botController.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        botController.SetDisplay("SEARCH-AND-RESCUE: Fire-Bot is now searching for Bots.");
        Vector3 botPos = bot.GetCurrentPos();
        agent.destination = botPos;
        agent.stoppingDistance = 1.0f;
    }

    public override void Execute()
    {
        Vector3 currentBotPos = bot.GetCurrentPos();
        agent.destination = currentBotPos;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.01f)
        {
            botController.ChangeState(botController.searchRescueState);
        }
       
    }

    public override void Exit()
    {
        Debug.Log("Exited SEARCH & RESCUE state");
    }
}
