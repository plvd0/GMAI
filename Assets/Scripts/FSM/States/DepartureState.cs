using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepartureState : FBState
{
    public DepartureState(FireBotFSM botController) : base(botController) { }

    public override void Enter()
    {
        botController.SetDisplay("DEPARTURE: Fire-Bot packs up, leaving the scene.");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        Debug.Log("Exited DEPARTURE state");
    }
}
