using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EducateState : FBState
{
    private NavMeshAgent agent;
    private GameObject assemblyArea;
    private FireManager fireManager;

    public EducateState(FireBotFSM botController, GameObject assemblyArea, FireManager fireManager) : base(botController) 
    {
        this.agent = botController.GetComponent<NavMeshAgent>();
        this.assemblyArea = assemblyArea;
        this.fireManager = fireManager;
    }

    public override void Enter()
    {
        botController.SetDisplay("EDUCATE: Fire-Bot educates the Bot on fire safety.");

        agent.destination = assemblyArea.transform.position; // Sets the destination to Assembly area
        agent.stoppingDistance = 2.5f; // Sets stopping distance from Assembly area
    }

    public override void Execute()
    {
        if (!agent.pathPending && Vector3.Distance(agent.transform.position, assemblyArea.transform.position) <= agent.stoppingDistance) // Checks if Fire-Bot is close enough to Assembly area
        {
            if (agent.velocity.sqrMagnitude < 0.1f) // Checks if Fire-Bot has practically stopped moving
            {
                string fireType = fireManager.fireType; // Retrieves fire type from FireManager - Electrical/Combustible
                string fireSource = fireManager.fireSource; // Retrieves fire location from FireManager - Kitchen/Living Room
                string preventionTip = GenerateTip(fireType, fireSource); // Generates safety tip based on fire type & source

                botController.SetDisplay($"EDUCATE: Safety tip based on: {fireSource} \n {preventionTip}"); // Updates display text with the tip based on location & type
                botController.StartCoroutine(ExitAfterDelay(3.0f)); // Starts coroutine to exit state after 3s
            }
        }
    }

    public override void Exit() {}

    private string GenerateTip(string type, string location)
    {
        string tips = "Keep your house clean of clutter to reduce fire risks."; // Default tip if no condition met

        switch (type) // Swithc statement for the different types of fire
        {
            case "Electrical": // Specific tips based on location for Electrical
                if (location == "Kitchen") 
                {
                    tips = "Check & maintain kitchen appliances for frayed cords or damaged parts.";
                }
                else if (location == "Living Room")
                {
                    tips = "Don't overload electrical outlets with too many devices.";
                }
                break;

            case "Combustible": // Specific tips based on location for Combustible
                if (location == "Kitchen")
                {
                    tips = "Never leave cooking food unattended.";
                }
                else if (location == "Living Room")
                {
                    tips = "Keep candles on a stable surface & away from flammable items.";
                }
                break;

            default:
                break;
        }

        return tips;
    }

    private IEnumerator ExitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        botController.ChangeState(botController.departureState); // Transitions to Departure state after 3s
    }
}