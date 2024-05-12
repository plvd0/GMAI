using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class FireBotFSM : MonoBehaviour
{
    public TextMeshProUGUI displayText; // References the display text component that will update throughout runtime of FSM

    public GameObject kitchen; // References the GameObject of the Kitchen
    public GameObject livingRoom; // References the GameObject of the Living Room

    public GameObject fireBot; // References the GameObject of the Fire-Bot
    public GameObject fireStation; // References the GameObject of the Fire Station, used in Departure state

    public GameObject fireHosePrefab; // References the GameObject of the Fire Hose, used in Equipment state
    public GameObject fireExtinguisherPrefab; // References the GameObject of the Fire Extinguisher, used in Equipment state

    public GameObject sceneDestination; // References the GameObject of the Scene, used in Activation state
    public GameObject assemblyArea; // References the GameObject of the Assembly, used in Evacuation state

    [HideInInspector]
    public GameObject currentEquipment; // Stores which tool is currently equipped - Fire Hose/Extinguisher
    [HideInInspector]
    public bool inAssemblyArea = false; // Tracks whether the Bot that Fire-Bot will evacuate is inside the Assembly area, set to False at start
    [HideInInspector] 
    public string agentTool; // Name of the tool equipped by the Fire-Bot - Fire Hose/Extinguisher
    [HideInInspector]
    public Vector3 toolOffset = new Vector3(-1, 0, 0); // Offset the tool when equipped by Fire-Bot
    public Vector3 botPos {  get; set; } // Stores the position of the Bot that Fire-Bot will rescue

    public FireManager fireManager;

    // Declaring the different states here for the FSM
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
        // Instantiating all states with the parameters required
        idleState = new IdleState(this, fireManager);
        activationState = new ActivationState(this);
        assessingState = new AssessingState(this, fireManager);
        equipmentState = new EquipmentState(this);
        extinguishingState = new ExtinguishingState(this, fireManager);
        searchRescueState = new SearchRescueState(this);
        evacuationSupportState = new EvacuationSupportState(this, assemblyArea);
        postFireState = new PostFireState(this, assemblyArea);
        educateState = new EducateState(this, assemblyArea, fireManager);
        departureState = new DepartureState(this, fireStation, fireManager);

        // Initializes the start of FSM to be Idle
        ChangeState(idleState);
    }

    void Update()
    {
        currentState?.Execute(); // Executes the current state's execute method during the runtime of its state
    }

    public void ChangeState(FBState newState)
    {
        if (currentState != null) // If current state exists, run the code below
        {
            Debug.Log($"Exiting {currentState.GetType().Name}");
            currentState.Exit();
        }
        currentState = newState;
        Debug.Log($"Entering {newState.GetType().Name}");
        currentState.Enter();
    }

    public void SetDisplay(string text)
    {
        displayText.text = text; // Updates the display text on the Canvas during the FSM
    }

    public void UpdateDisplayWithDelay(string message, float delay)
    {
        StartCoroutine(UpdateDisplayCoroutine(message, delay)); // Starts a coroutine to delay updating the display
    }

    private IEnumerator UpdateDisplayCoroutine(string message, float delay)
    {
        yield return new WaitForSeconds(delay); // Waits for the specified delay passed in as a parameter
        SetDisplay(message); // Sets the message using the method above
    }

    void OnTriggerEnter(Collider other)
    {
        if (currentState is SearchRescueState) // If the current state that the FSM is in is Search & Rescue, runs code below
        {
            if (other.CompareTag("Bot")) // If the collider is tagged with 'Bot', it will switch to Evacuation state
            {
                ChangeState(evacuationSupportState);
            }
        }
    }
}