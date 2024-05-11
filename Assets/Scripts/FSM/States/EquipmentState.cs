using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentState : FBState
{
    private float timer = 5.0f;
    private bool equipmentDeployed = false;

    public EquipmentState(FireBotFSM botController) : base(botController) { }

    public override void Enter()
    {
        botController.SetDisplay("EQUIPMENT: Fire-Bot is gearing up based on fire data.");
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if(timer <= 3.0f && !equipmentDeployed)
        {
            DeployEquipment();
            equipmentDeployed = true;
        }
        else if(timer <= 0)
        {
            botController.ChangeState(botController.extinguishingState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited EQUIPMENT state");
    }

    private void DeployEquipment()
    {
        GameObject equipmentPrefab = botController.agentTool == "Fire Hose" ? botController.fireHosePrefab : botController.fireExtinguisherPrefab;

        GameObject equipmentInstance = GameObject.Instantiate(equipmentPrefab, botController.fireBot.transform.position + botController.toolOffset, Quaternion.identity);
        equipmentInstance.transform.SetParent(botController.fireBot.transform);
        equipmentInstance.transform.localPosition = botController.toolOffset;

        botController.SetDisplay($"EQUIPMENT: Fire-Bot takes out the {botController.agentTool}.");
    }
}
