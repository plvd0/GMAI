using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public GameObject kitchenRoomLocation; // References the GameObject of the Kitchen
    public GameObject livingRoomLocation; // References the GameObject of the Living Room
    public GameObject bedRoomLocation; // References the GameObject of the Bot spawnpoint
    public GameObject botPrefab; // References the Bot
    public GameObject fire; // References the Fire that will be instantiated

    public FireBotFSM botController; // References the FSM

    public bool fireActivated {  get; set; } = false; // Tracks whether the Fire has been activated, set to False at start
    public Vector3 firePosition {  get; set; } // Stores the position of the Fire

    public string fireType; // Stores the type of fire - Electrical/Combustible
    public string fireSource; // Stores the location of fire - Kitchen/Living Room
    public float fireSize; // Stores the size of fire - range from 1-5

    void Start()
    {
        if (!fireActivated) // If no fire is activated initially, runs the code below
        {
            InvokeRepeating("GenerateFire", 1.0f, 2.0f); // Runs the GenerateFire method every 2s starting from 1s after runtime begins
        }
    }

    public void GenerateFire()
    {
        Debug.Log("Attempting to start fire");
        if (Random.value < 0.5) // 50% chance that a fire will start
        {
            bool isKitchen = Random.value < 0.5f; // 50% chance that the fire starts in Kitchen
            GameObject fireLocation = isKitchen ? kitchenRoomLocation : livingRoomLocation; // Chooses the location of fire using ternary operator
            fireSource = isKitchen ? "Kitchen" : "Living Room"; // Sets the location based on the result above

            bool isElectrical = isKitchen ? Random.value < 0.7f : Random.value < 0.3f; // Determines type of fire based on room, 70% for Electrical, 30% for Combustible
            fireType = isElectrical ? "Electrical" : "Combustible"; // Sets the type of fire based on the result above

            fireSize = Random.Range(1.0f, 5.0f); // Sets the size of fire between 1-5

            Debug.Log($"Fire started in {(isKitchen ? "Kitchen" : "Living Room")}, Type: {fireType}, Size: {fireSize}");
            CancelInvoke("GenerateFire"); // Stops further invoking in the Start() method as fire is active

            if (Random.value < 0.6f) // 60% chance that a Bot will spawn, which Fire-Bot needs to rescue
            {
                SpawnBot(bedRoomLocation); // Runs the method that passes the location that Bot will spawn
            }

            CreateFire(fireLocation.transform.position, fireType, fireSize); // Creates the fire based on the results above, which is position, type & size
            fireActivated = true; // Marks the Fire as activiated - True
        }
    }

    public GameObject GetFireSource() // Method that returns the fire source based on the result above as a GameObject rather than string now
    {
        return fireSource == "Kitchen" ? kitchenRoomLocation : livingRoomLocation;
    }

    private void CreateFire(Vector3 position, string type, float size)
    {
        fire = GameObject.CreatePrimitive(PrimitiveType.Cube); // Creates a cube to represent the fire as the object
        fire.transform.position = position; // Positions the fire at the location - Kitchen/Living Room
        fire.transform.localScale = new Vector3(size, size, size); // Sizes the fire based on the generated result

        Color fireColor = type == "Electrical" ? Color.blue : Color.red; // Assigns a color to fire based on type using ternary operator - Blue for Electric/Red for Combustible
        fire.GetComponent<Renderer>().material.color = fireColor;

        firePosition = position; // Updates the stored firePosition
    }

    private void SpawnBot(GameObject location)
    {
        Instantiate(botPrefab, location.transform.position, Quaternion.identity); // Spawns the prefab passed in at the location in world space, with a 'default' rotation
    }
}