using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FBState
{
    private FireManager fireManager;

    public IdleState(FireBotFSM botController, FireManager fireManager) : base(botController) 
    {
        this.fireManager = fireManager;
    }

    public override void Enter()
    {
        botController.SetDisplay("IDLE: Fire-Bot is on-call to respond.");
    }

    public override void Execute()
    {
        if (fireManager.fireActivated) // Checks if fire is active
        {
            botController.ChangeState(botController.activationState);
        }
    }

    public override void Exit() 
    {
        Debug.Log("Exited IDLE state");
    }
}
