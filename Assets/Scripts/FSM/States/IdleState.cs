using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FBState
{
    private FireManager fireManager;

    public IdleState(FireBotFSM botController, FireManager fireManager) : base(botController) 
    {
        this.fireManager = fireManager; // References the FireManager as part of its parameter
    }

    public override void Enter()
    {
        botController.SetDisplay("IDLE: Fire-Bot is on call to respond."); // Sets the display text at the top of screen
    }

    public override void Execute()
    {
        if (fireManager.fireActivated) // Checks if the flag in FireManager is set to True
        {
            botController.ChangeState(botController.activationState); // Transitions to Activation state
        }
    }

    public override void Exit() {}
}
