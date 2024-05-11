using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssessingState : FBState
{
    private FireManager fireManager;

    private float timer = 8.0f;
    public string agentType;
    public string location;

    private bool initialDisplayComplete = false;

    public AssessingState(FireBotFSM botController, FireManager fireManager) : base(botController) 
    {
        this.fireManager = fireManager;
    }

    public override void Enter()
    {
        botController.SetDisplay("ASSESSING: Gathering data...");
        timer = 8.0f;
        initialDisplayComplete = false;
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;

        if (!initialDisplayComplete && timer <= 5.0f)
        {
            initialDisplayComplete = true;
            FireAssessment();
            botController.SetDisplay($"ASSESSING: Completed. \n Type: {fireManager.fireType} \n Agent: {agentType} \n Tool: {botController.agentTool} \n Location: {fireManager.fireSource}");
        }
        else if(initialDisplayComplete && timer <= 0)
        {
            botController.ChangeState(botController.equipmentState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited ASSESSING state");
    }

    private void FireAssessment()
    {
        agentType = fireManager.fireType == "Electrical" ? "CO2" : "Water";
        botController.agentTool = fireManager.fireSize > 3.0f ? "Fire Hose" : "Fire Extinguisher";
        location = fireManager.fireSource;
    }
}
