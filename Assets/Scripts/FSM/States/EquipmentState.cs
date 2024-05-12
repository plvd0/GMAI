using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentState : FBState
{
    private float timer = 5.0f; // Stores a countdown timer for 5s
    private bool equipmentDeployed = false; // Tracks whether the equipment has been deployed, set to False at start

    public EquipmentState(FireBotFSM botController) : base(botController) { }

    public override void Enter()
    {
        botController.UpdateDisplayWithDelay("EQUIPMENT: Fire-Bot is gearing up based on data.", 0.1f); // Displays the message with a delay of 0.1s using the FireBotFSM's method
    }

    public override void Execute()
    {
        timer -= Time.deltaTime; // Decreases the timer by the time during runtime

        if (timer <= 3.0f && !equipmentDeployed) // Checks if equipment isn't deployed & timer is less or equal to 3s
        {
            DeployEquipment(); // Runs the method to instantiate the correct equipment
            equipmentDeployed = true;
        }
        else if(timer <= 0)
        {
            botController.ChangeState(botController.extinguishingState); // Transitions to Extinguishing state
        }
    }

    public override void Exit() {}

    private void DeployEquipment()
    {
        GameObject equipmentPrefab = botController.agentTool == "Fire Hose" ? botController.fireHosePrefab : botController.fireExtinguisherPrefab; // Determines equipment type based on the agentTool variable passed in from FireBotFSM

        botController.currentEquipment = GameObject.Instantiate(equipmentPrefab, botController.fireBot.transform.position + botController.toolOffset, Quaternion.identity); // Instantiates the equipment at Fire-Bot position, with a offset variable from FireBotFSM
        botController.currentEquipment.transform.SetParent(botController.fireBot.transform); // Sets the parent of equipment to Fire-Bot, thus it moves with Fire-Bot
        botController.currentEquipment.transform.localPosition = botController.toolOffset; // Sets local position of equipment relative to Fire-Bot using offset

        botController.SetDisplay($"EQUIPMENT: Fire-Bot takes out the {botController.agentTool}."); // Updates display based on equipment used based on fire size
    }
}