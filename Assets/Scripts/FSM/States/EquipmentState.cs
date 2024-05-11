using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentState : FBState
{
    public EquipmentState(FireBotFSM botController) : base(botController) { }

    public override void Enter()
    {
        botController.SetDisplay("EQUIPMENT: Fire-Bot is gearing up based on fire data.");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        Debug.Log("Exited EQUIPMENT state");
    }
}
