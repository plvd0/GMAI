using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActivationState : FBState
{
    private NavMeshAgent agent; // References the NavMeshAgent component inside Fire-Bot

    public ActivationState(FireBotFSM botController) : base(botController) {}

    public override void Enter()
    {
        botController.UpdateDisplayWithDelay("ACTIVATION: Fire-Bot is now enroute to the scene.", 0.1f); // Displays the message with a delay of 0.1s using the FireBotFSM's method
        this.agent = botController.GetComponent<NavMeshAgent>(); // Retrieving the NavMeshAgent component & initializes it
        MoveToScene(botController.fireBot, botController.sceneDestination.transform.position); // Calls method to move the Fire-Bot to the scene
    }

    public override void Execute()
    {
        Vector3 fireBotTargetPos = botController.sceneDestination.transform.position; // Stores the target position that the Fire-Bot should travel to

        if (!AtScene(botController.fireBot, fireBotTargetPos)) // Checks if the Fire-Bot has reached the scene
        {
            MoveToScene(botController.fireBot, fireBotTargetPos); // If it hasn't reached, it calls method to move toward the destination
        }
        else
        {
            botController.ChangeState(botController.assessingState); // If it has reached, it will transition to Assessing state
        }
    }

    public override void Exit() {}

    private void MoveToScene(GameObject fireObj, Vector3 targetPos)
    {
        NavMeshAgent agent = fireObj.GetComponent<NavMeshAgent>(); // Retrieves the NavMeshAgent component from the parameter passed to it (Fire-Bot)
        if (agent != null) 
        {
            agent.destination = targetPos; // Sets the destination to the destination position, passed from FireBotFSM
        }
    }

    private bool AtScene(GameObject fireObj, Vector3 targetPos) // Checks whether Fire-Bot has reached the scene
    {
        NavMeshAgent agent = fireObj.GetComponent<NavMeshAgent>(); // Retrieves the NavMeshAgent component from the parameter passed to it (Fire-Bot)
        if (agent == null) return false;

        float distanceToTarget = Vector3.Distance(agent.transform.position, targetPos); // Calculates the distance between current position of Fire-Bot & the destination position

        if (!agent.pathPending && agent.remainingDistance <= 0.1f && agent.velocity.sqrMagnitude < 0.01f) // Checks if Fire-Bot is not pending a path & close enough to the destination position
        {
            return true; // Marks true, indicating Fire-Bot has reached the scene
        }
        return false; // Marks false, indicating Fire-Bot hasn't reached the scene
    }
}