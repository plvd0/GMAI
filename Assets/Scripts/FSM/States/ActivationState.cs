using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationState : FBState
{
    public ActivationState(FireBotFSM botController) : base(botController) { }

    public override void Enter()
    {
        botController.SetDisplay("ACTIVATION: Fire-Bot has detected fire, gearing up now.");
    }

    public override void Execute()
    {
        botController.ChangeState(botController.assessingState);
    }

    public override void Exit()
    {
        Debug.Log("Exited ACTIVATION state");
    }
}
