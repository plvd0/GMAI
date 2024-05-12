using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExtinguishingState : FBState
{
    private FireManager fireManager;

    private float extinguishingTime = 10.0f; // Time that Fire-Bot will take extingushing the fire - 10s
    private float timer; // Timer to count down the extinguishing time

    private float helpInterval = 5.0f; // Intervals to check for help while extinguishing from Bots 
    private float helpTimer; // Timer to count down the help interval

    public ExtinguishingState(FireBotFSM botController, FireManager fireManager) : base(botController)
    {
        this.fireManager = fireManager; // Takes in FireManager as a parameter to pass variables into this state
    }

    public override void Enter()
    {
        Debug.Log("ExtinguishingState initialized with fire position: " + fireManager.firePosition);

        botController.UpdateDisplayWithDelay("EXTINGUISHING: Fire-Bot is now fighting the fire.", 0.1f); // Displays the message with a delay of 0.1s using the FireBotFSM's method

        timer = extinguishingTime; // Sets the timer to 10s
        helpTimer = helpInterval; // Sets the timer to 5s
    }

    public override void Execute()
    {
        if (botController.currentState != this) return; // Prevents execution if the current state isn't Extinguishing

        helpTimer -= Time.deltaTime; // Decreases the help timer by the time during runtime
        if (helpTimer <= 0) // If help timer reaches 0s
        {
            CheckForHelp(); // Runs this method that checks
            helpTimer = helpInterval; // Resets the timer back to 5s to run again
        }

        if (AtFirePos()) // Checks if Fire-Bot is at the position of the fire
        {
            if (timer > 0) // If timer is above 0, it isn't extinguished, run code below
            { 
                timer -= Time.deltaTime; // Decreases the timer by the time during runtime
                botController.SetDisplay($"EXTINGUISHING: {Mathf.Floor(timer)} seconds before Fire-Bot extinguishes the fire."); // Updates the display, rounds down the number to whole values
            }
            else
            {
                GameObject.Destroy(fireManager.fire); // Once the timer reaches 0, it destroys the Fire object
                botController.ChangeState(botController.postFireState); // Transitions to Post-Fire state
            }
        }
        else
        {
            MoveToFire(); // If Fire-Bot is not at the position of the fire, move it towards it
        }

    }

    public override void Exit() {}

    private void MoveToFire()
    {
        NavMeshAgent agent = botController.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            Vector3 targetPos = fireManager.firePosition;
            agent.destination = targetPos; // Sets the destination to Fire
            agent.stoppingDistance = 5.0f; // Sets stopping distance from the Fire
        }
    }

    private bool AtFirePos()
    {
        NavMeshAgent agent = botController.GetComponent<NavMeshAgent>();
        if (agent != null && !agent.pathPending)
        {
            Vector3 targetPos = fireManager.firePosition; // Sets the position to the Fire
            float distance = Vector3.Distance(agent.transform.position, targetPos); // Calculates distance between Fire-Bot & the Fire

            if (distance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.1f) // Checks if distance is less or equal to stopping distance & has practically stopped
            {
                return true; // Fire-Bot has reached the fire's position
            }
        }
        return false; // Fire-Bot has not reached the fire's position
    }

    private void CheckForHelp()
    {
        if (botController.inAssemblyArea) // Checks if the value is true/not
        {
            return; // Exits if the Bot rescued is already inside Assembly
        }

        float detection = 25.0f; // Detection radius for other Bots
        Collider[] collider = Physics.OverlapSphere(botController.transform.position, detection); // Detects all colliders within the Fire-Bot's position using a sphere of radius 25.0f

        foreach (var col in collider) // Loops through each collider detected within radius
        {
            if (col.CompareTag("Bot")) // Checks if collider belongs to 'Bot'
            {
                Debug.Log("HELP NEEDED");
                botController.botPos = col.transform.position; // Updates botPos to value that the Bot is at
                botController.ChangeState(botController.searchRescueState); // Transitions to Search & Rescue state instead
            }
        }
    }
}