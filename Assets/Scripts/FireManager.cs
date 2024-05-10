using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public GameObject kitchenRoomLocation;
    public GameObject livingRoomLocation;
    public GameObject bedRoomLocation;
    public GameObject botPrefab;

    public FireBotFSM botController;

    void Start()
    {
        InvokeRepeating("GenerateFire", 1.0f, 2.0f);
    }

    public void GenerateFire()
    {
        Debug.Log("Attempting to start fire");
        if(Random.value < 0.1)
        {
            bool isKitchen = Random.value < 0.5f;
            GameObject fireLocation = isKitchen ? kitchenRoomLocation : livingRoomLocation;

            bool isElectrical = isKitchen ? Random.value < 0.7f : Random.value < 0.3f;
            string fireType = isElectrical ? "Electrical" : "Combustible";

            float fireSize = Random.Range(1.0f, 5.0f);

            Fire fireComponent = fireLocation.GetComponent<Fire>();
            fireComponent.InitializeFire(fireType, fireSize);

            Debug.Log($"Fire started in {(isKitchen ? "Kitchen" : "Living Room")} - Type: {fireType}, Size: {fireSize}");
            CancelInvoke("GenerateFire");

            if(Random.value < 0.8f)
            {
                SpawnBot(bedRoomLocation);
            }
        }
    }

    void SpawnBot(GameObject location)
    {
        Instantiate(botPrefab, location.transform.position, Quaternion.identity);
        Debug.Log("Bot spawned in Bedroom");
    }
}
