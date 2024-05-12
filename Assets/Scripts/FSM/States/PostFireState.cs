using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PostFireState : FBState
{
    private GameObject assemblyArea;

    public PostFireState(FireBotFSM botController, GameObject assemblyArea) : base(botController) 
    {
        this.assemblyArea = assemblyArea;
    }

    public override void Enter()
    {
        botController.SetDisplay("POST-FIRE: Fire-Bot has finished extinguishing fire, deciding next step...");
    }

    public override void Execute()
    {
        Collider[] collider = Physics.OverlapSphere(assemblyArea.transform.position, 5.0f);
        bool botsInAssembly = collider.Any(c => c.CompareTag("Bot"));

        if (botsInAssembly)
        {
            botController.ChangeState(botController.educateState);
        }
        else
        {
            botController.ChangeState(botController.departureState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited POST-FIRE state");
    }
}
