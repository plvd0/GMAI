using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssessingState : FBState
{
    private FireManager fireManager;

    private float timer = 5.0f;
    public string agentType;
    public string agentTool;
    public string location;

    private bool assessmentShown = false;
    private bool assessmentComplete = false;

    public AssessingState(FireBotFSM botController, FireManager fireManager) : base(botController) 
    {
        this.fireManager = fireManager;
    }

    public override void Enter()
    {
        botController.SetDisplay("ASSESSING: Gathering data...");
        timer = 5.0f;
        assessmentComplete = false;
    }

    public override void Execute()
    {
        if(!assessmentComplete)
        {
            timer -= Time.deltaTime;

            if(timer <= 5.0f && !assessmentComplete)
            {
                FireAssessment();
                assessmentComplete = true;
                botController.SetDisplay($"Fire Type: {fireManager.fireType}, Size: {fireManager.fireSize}m³, Agent: {agentType}, Tool: {agentTool}, Location: {fireManager.fireSource}");
            }
        }
        else if(!assessmentShown && timer <= 2.0f)
        {
            assessmentShown = true;
        }
        else if(assessmentShown && timer <= 0f)
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
        agentTool = fireManager.fireSize > 3.0f ? "Fire Hose" : "Fire Extinguisher";
        location = fireManager.fireSource;
    }
}
