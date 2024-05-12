using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssessingState : FBState
{
    private FireManager fireManager;

    private float timer = 8.0f; // Stores a countdown timer for 8s
    public string agentType; // Stores the type of agent Fire-Bot will use - Water/CO2 based on fire type
    public string location; // Stores the location of fire

    private bool initialDisplayComplete = false; // Tracks whether the initial display shown is completed, set to False at start

    public AssessingState(FireBotFSM botController, FireManager fireManager) : base(botController) 
    {
        this.fireManager = fireManager; // Takes in FireManager as a parameter to pass variables into this state
    }

    public override void Enter()
    {
        botController.UpdateDisplayWithDelay("ASSESSING: Gathering data...", 0.1f); // Displays the message with a delay of 0.1s using the FireBotFSM's method
        timer = 8.0f; // Resets timer to 8s when it enters this state
        initialDisplayComplete = false; // Resets the flag to false again when it enters this state
    }

    public override void Execute()
    {
        timer -= Time.deltaTime; // Decreases the timer by the time during runtime

        if (!initialDisplayComplete && timer <= 5.0f) // Checks if initial display shown is not complete & timer is less or equal to 5s
        {
            initialDisplayComplete = true;
            FireAssessment(); // Runs this method to determine agent type & equipment required for later states
            botController.SetDisplay($"ASSESSING: Completed. \n Type: {fireManager.fireType} \n Agent: {agentType} \n Tool: {botController.agentTool} \n Location: {fireManager.fireSource}"); // Updates display based on results from FireAssessment
        }
        else if(initialDisplayComplete && timer <= 0) // Checks if initial display is marked true and timer is 0s
        {
            botController.ChangeState(botController.equipmentState); // Transitions to Equipment state
        }
    }

    public override void Exit() { }

    private void FireAssessment()
    {
        agentType = fireManager.fireType == "Electrical" ? "CO2" : "Water"; // Determines agent type based on type of fire - Electrical/Combustible 
        botController.agentTool = fireManager.fireSize > 3.0f ? "Fire Hose" : "Fire Extinguisher"; // Detemrines tool type based on size of fire - Hose/Extinguisher with condition of being bigger/smaller than 3.0f
        location = fireManager.fireSource; // Stores location of fire from FireManager
    }
}