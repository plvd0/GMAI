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
    public GameObject fireHosePrefab;
    public GameObject fireExtinguisherPrefab;
    public GameObject sceneDestination;

    [HideInInspector] 
    public string agentTool;
    [HideInInspector]
    public Vector3 toolOffset = new Vector3(-1, 0, 0);

    public FireManager fireManager;
    public Bot bot;

    // Declaring the different states here
    public FBState currentState;
    public IdleState idleState;
    public ActivationState activationState;
    public AssessingState assessingState;
    public EquipmentState equipmentState;
    public ExtinguishingState extinguishingState;
    public SearchRescueState searchRescueState;
    public EvacuationSupportState evacuationSupportState;
    public PostFireState postFireState;

    void Start()
    {
        idleState = new IdleState(this, fireManager);
        activationState = new ActivationState(this);
        assessingState = new AssessingState(this, fireManager);
        equipmentState = new EquipmentState(this);
        extinguishingState = new ExtinguishingState(this, fireManager);
        searchRescueState = new SearchRescueState(this, bot);
        evacuationSupportState = new EvacuationSupportState(this);
        postFireState = new PostFireState(this);

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

    public void HearHelp(Vector3 position)
    {
        if(currentState is ExtinguishingState)
        {
            ChangeState(searchRescueState);
        }
    }
}
