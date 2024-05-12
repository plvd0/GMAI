using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EducateState : FBState
{
    public EducateState(FireBotFSM botController) : base(botController) { }

    public override void Enter()
    {
        botController.SetDisplay("EDUCATE: Fire-Bot educates the Bots on fire prevention.");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        Debug.Log("Exited EDUCATE state");
    }
}
