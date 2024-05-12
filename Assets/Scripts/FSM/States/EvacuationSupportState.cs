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
        this.agent = botController.GetComponent<NavMeshAgent>(); // Assigns the bot's
        this.assemblyArea = assemblyArea; // Takes in assemblyArea GameObject as a parameter to pass into this state
    }

    public override void Enter()
    {
        botController.UpdateDisplayWithDelay("EVACUATION-SUPPORT: Fire-Bot is now rescuing the Bot.", 0.1f);

        botRescue = GameObject.FindGameObjectWithTag("Bot"); // Sets any GameObject tagged as 'Bot' that needs to be rescued

        agent.destination = assemblyArea.transform.position; // Sets destination Fire-Bot needs to navigate to 
        agent.stoppingDistance = 1.0f;
    }

    public override void Execute()
    {
        NavMeshAgent botRescueAgent = botRescue.GetComponent<NavMeshAgent>();
        botRescueAgent.destination = botController.transform.position; // Sets the destination that the Bot being rescued to be the Fire-Bot's current position

        float proximity = 2.0f; // Sets proximity to determine when both Bot & Fire-Bot is close to Assembly area

        if (Vector3.Distance(agent.transform.position, assemblyArea.transform.position) < proximity && 
            (botRescue == null || Vector3.Distance(botRescue.transform.position, assemblyArea.transform.position) < proximity)) // Checks if both Bot & Fire-Bot is close enough to Assembly area, determined by proximity value
        {
            botController.ChangeState(botController.extinguishingState); // Transitions back to Extinguishing state
        }

    }

    public override void Exit()
    {
        botController.inAssemblyArea = true; // Marks that the Bot is inside the Assembly area, ignoring CheckForHelp() method in Extinguishing state
    }
}