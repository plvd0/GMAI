using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class FireBotFSM : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    public GameObject kitchen;
    public GameObject livingRoom;

    public GameObject fireBot;
    public GameObject fireHosePrefab;
    public GameObject fireExtinguisherPrefab;
    public GameObject sceneDestination;

    public GameObject assemblyArea;
    public bool inAssemblyArea = false;

    [HideInInspector] 
    public string agentTool;
    [HideInInspector]
    public Vector3 toolOffset = new Vector3(-1, 0, 0);

    public Vector3 botPos {  get; set; }

    public FireManager fireManager;

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
    public EducateState educateState;
    public DepartureState departureState;

    void Start()
    {
        idleState = new IdleState(this, fireManager);
        activationState = new ActivationState(this);
        assessingState = new AssessingState(this, fireManager);
        equipmentState = new EquipmentState(this);
        extinguishingState = new ExtinguishingState(this, fireManager);
        searchRescueState = new SearchRescueState(this);
        evacuationSupportState = new EvacuationSupportState(this, assemblyArea);
        postFireState = new PostFireState(this, assemblyArea);
        educateState = new EducateState(this);
        departureState = new DepartureState(this);

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

    public void UpdateDisplayWithDelay(string message, float delay)
    {
        StartCoroutine(UpdateDisplayCoroutine(message, delay));
    }

    private IEnumerator UpdateDisplayCoroutine(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetDisplay(message);
    }

    void OnTriggerEnter(Collider other)
    {
        if (currentState is SearchRescueState)
        {
            if (other.CompareTag("Bot"))
            {
                ChangeState(evacuationSupportState);
            }
        }
    }
}
