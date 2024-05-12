using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PostFireState : FBState
{
    private GameObject assemblyArea;

    public PostFireState(FireBotFSM botController, GameObject assemblyArea) : base(botController) 
    {
        this.assemblyArea = assemblyArea; // Takes in assemblyArea GameObject as a parameter to pass into this state
    }

    public override void Enter()
    {
        botController.SetDisplay("POST-FIRE: Fire-Bot has finished extinguishing the fire.");
    }

    public override void Execute()
    {
        Collider[] collider = Physics.OverlapSphere(assemblyArea.transform.position, 10.0f); // Detects all colliders within the Assembly area position using a sphere of radius 10f
        bool botsInAssembly = collider.Any(c => c.CompareTag("Bot")); // Checks if any colliders inside the radius is tagged as 'Bot'

        if (botsInAssembly) // Checks if flagged as true/false
        {
            botController.StartCoroutine(ChangeState(botController.educateState, 2.0f)); // If Bots are present, transitions to Educate state
        }
        else
        {
            botController.StartCoroutine(ChangeState(botController.departureState, 2.0f)); // If Bots are not present, transitions to Departure state
        }
    }

    public override void Exit() {}

    private IEnumerator ChangeState(FBState newState, float delay) // Helps coroutine in changing the state after a delay to display the message
    {
        yield return new WaitForSeconds(delay);
        botController.ChangeState(newState);
    }
}