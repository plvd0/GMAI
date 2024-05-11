using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireBotFSM : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public GameObject kitchen;
    public GameObject livingRoom;

    public GameObject fireBot;
    public GameObject fireTruck;
    public GameObject sceneDestination;

    public FireManager fireManager;

    // Declaring the different states here
    public FBState currentState;
    public IdleState idleState;
    public ActivationState activationState;
    public AssessingState assessingState;
    public EquipmentState equipmentState;

    void Start()
    {
        idleState = new IdleState(this, fireManager);
        activationState = new ActivationState(this);
        assessingState = new AssessingState(this, fireManager);
        equipmentState = new EquipmentState(this);

        ChangeState(idleState);
    }

    void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(FBState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SetDisplay(string text)
    {
        displayText.text = text;
    }
}
