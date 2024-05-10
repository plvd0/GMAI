using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssessingState : FBState
{
    public AssessingState(FireBotFSM botController) : base(botController) { }

    public override void Enter()
    {
        botController.SetDisplay("ASSESSING: Fire-Bot is gathering data on fire.");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        Debug.Log("Exited ASSESSING state");
    }
}
