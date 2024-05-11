using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationState : FBState
{
    private float speed = 5.0f;
    private Vector3 fireTruckOffset = new Vector3(3, 0, 0);

    public ActivationState(FireBotFSM botController) : base(botController) 
    {
        this.botController = botController;
    }

    public override void Enter()
    {
        botController.SetDisplay("ACTIVATION: Fire-Bot has detected fire, enroute to scene.");
    }

    public override void Execute()
    {
        Vector3 fireBotTargetPos = botController.sceneDestination.transform.position;
        Vector3 fireTruckTargetPos = botController.sceneDestination.transform.position + fireTruckOffset;

        if(!AtScene(botController.fireBot, fireBotTargetPos))
        {
            MoveToScene(botController.fireBot, fireBotTargetPos);
        }

        if(!AtScene(botController.fireTruck, fireTruckTargetPos))
        {
            MoveToScene(botController.fireTruck, fireTruckTargetPos);
        }

        if(AtScene(botController.fireBot, fireBotTargetPos) && AtScene(botController.fireTruck, fireTruckTargetPos))
        {
            botController.ChangeState(botController.assessingState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited ACTIVATION state");
    }

    private void MoveToScene(GameObject fireObj, Vector3 targetPos)
    {
        fireObj.transform.position = Vector3.MoveTowards(fireObj.transform.position, targetPos, speed * Time.deltaTime);
    }

    private bool AtScene(GameObject fireObj, Vector3 targetPos)
    {
        return Vector3.Distance(fireObj.transform.position, targetPos) < 0.1f;
    }
}
