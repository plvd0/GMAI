using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DepartureState : FBState
{
    private NavMeshAgent agent;
    private GameObject fireStation;
    private FireManager fireManager;

    public DepartureState(FireBotFSM botController, GameObject fireStation, FireManager fireManager) : base(botController) 
    {
        this.agent = botController.GetComponent<NavMeshAgent>();
        this.fireStation = fireStation;
        this.fireManager = fireManager;
    }

    public override void Enter()
    {
        botController.UpdateDisplayWithDelay("DEPARTURE: Fire-Bot has finished their duties, enroute to fire station now.", 0.1f);
        agent.destination = fireStation.transform.position; // Sets the destination of Fire-Bot to the Fire Station position
        agent.stoppingDistance = 1.0f; // Sets stopping distance to 1.0f
    }

    public override void Execute()
    {
        float proximity = 1.5f; // Sets proximity to determine when Fire-Bot is close to Fire Station
        float distance = Vector3.Distance(agent.transform.position, fireStation.transform.position); // Calculates distance between Fire-Bot & Fire Station

        if (!agent.pathPending && distance <= proximity) // Checks if Fire-Bot isn't currently navigating a path & within proximity distance
        {
            if (agent.velocity.sqrMagnitude < 0.1f) // Checks if Fire-Bot isn't practically moving
            {
                Arrival(); // Runs this method if so
            }
        }
    }

    public override void Exit() {}

    private void Arrival()
    {
        if (botController.currentEquipment != null)
        {
            GameObject.Destroy(botController.currentEquipment); // Destroys the current equipment Fire-Bot is holding - Fire Hose/Extinguisher
            botController.currentEquipment = null; // Resets the value to be null
        }

        GameObject.Destroy(GameObject.FindGameObjectWithTag("Bot")); // Destroys any GameObject in the scene with 'Bot' tag
        botController.inAssemblyArea = false; // Resets the value back to False, indicating no Bots are at Assembly area
        fireManager.fireActivated = false; // Resets the value back to False

        botController.ChangeState(botController.idleState); // Transitions back to Idle state
    }
}