using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FBState
{
    public IdleState(FireBotFSM botController) : base(botController) { }

    public override void Enter()
    {
        botController.SetDisplay("IDLE: Fire-Bot is on-call to respond.");
    }

    public override void Execute()
    {
        if (ActiveFire(botController.kitchen) || ActiveFire(botController.livingRoom)) // Checks if fire is active in either Living Room or Kitchen
        {
            botController.ChangeState(botController.activationState);
        }
    }

    public override void Exit() 
    {
        Debug.Log("Exited IDLE state");
    }

    private bool ActiveFire(GameObject location)
    {
        Fire fireComponent = location.GetComponent<Fire>();
        return fireComponent != null && fireComponent.isBurning;
    }
}
