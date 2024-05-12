using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EvacuationSupportState : FBState
{
    private NavMeshAgent agent;
    private GameObject assemblyArea;
    private GameObject botRescue;

    public EvacuationSupportState(FireBotFSM botController, GameObject assemblyArea) : base(botController) 
    {
        this.agent = botController.GetComponent<NavMeshAgent>();
        this.assemblyArea = assemblyArea;
    }

    public override void Enter()
    {
        botController.UpdateDisplayWithDelay("EVACUATION-SUPPORT: Fire-Bot is now rescuing the Bot.", 0.1f);

        botRescue = GameObject.FindGameObjectWithTag("Bot");

        agent.destination = assemblyArea.transform.position;
        agent.stoppingDistance = 1.0f;
    }

    public override void Execute()
    {
        NavMeshAgent botRescueAgent = botRescue.GetComponent<NavMeshAgent>();
        botRescueAgent.destination = botController.transform.position;

        float proximity = 2.0f;

        if (Vector3.Distance(agent.transform.position, assemblyArea.transform.position) < proximity &&
            (botRescue == null || Vector3.Distance(botRescue.transform.position, assemblyArea.transform.position) < proximity))
        {
            Debug.Log("Both bots have reached the evacuation safe zone. Transitioning back to Extinguishing State.");
            botController.ChangeState(botController.extinguishingState);
        }

    }

    public override void Exit()
    {
        botController.inAssemblyArea = true;
        Debug.Log("Exited EVACUATION SUPPORT state");
    }
}
