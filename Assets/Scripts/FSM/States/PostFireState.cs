using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostFireState : FBState
{
    public PostFireState(FireBotFSM botController) : base(botController) { }

    public override void Enter()
    {
        botController.SetDisplay("POST-FIRE: Fire-Bot is now fighting the fire.");
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        Debug.Log("Exited POST-FIRE state");
    }
}
