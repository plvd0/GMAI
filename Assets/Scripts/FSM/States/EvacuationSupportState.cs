using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EvacuationSupportState : FBState
{
    public EvacuationSupportState(FireBotFSM botController) : base(botController) {}

    public override void Enter()
    {
        botController.SetDisplay("EVACUATION SUPPORT: Fire-Bot is now evacuating the Bot.");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        Debug.Log("Exited EVACUATION SUPPORT state");
    }
}
