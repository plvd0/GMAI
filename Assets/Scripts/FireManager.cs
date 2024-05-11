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
    public bool fireActivated {  get; private set; } = false;
    public Vector3 firePosition {  get; private set; }

    public string fireType;
    public string fireSource;
    public float fireSize;

    void Start()
    {
        InvokeRepeating("GenerateFire", 1.0f, 2.0f);
    }

    public void GenerateFire()
    {
        Debug.Log("Attempting to start fire");
        if (Random.value < 0.5)
        {
            bool isKitchen = Random.value < 0.5f;
            GameObject fireLocation = isKitchen ? kitchenRoomLocation : livingRoomLocation;
            fireSource = isKitchen ? "Kitchen" : "Living Room";

            bool isElectrical = isKitchen ? Random.value < 0.7f : Random.value < 0.3f;
            fireType = isElectrical ? "Electrical" : "Combustible";

            fireSize = Random.Range(1.0f, 5.0f);

            Debug.Log($"Fire started in {(isKitchen ? "Kitchen" : "Living Room")} - Type: {fireType}, Size: {fireSize}");
            CancelInvoke("GenerateFire");

            if (Random.value < 0.6f)
            {
                SpawnBot(bedRoomLocation);
            }

            CreateFire(fireLocation.transform.position, fireType, fireSize);
            fireActivated = true;
        }
    }

    public GameObject GetFireSource() 
    {
        return fireSource == "Kitchen" ? kitchenRoomLocation : livingRoomLocation;
    }

    private void CreateFire(Vector3 position, string type, float size)
    {
        GameObject fire = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fire.transform.position = position;
        fire.transform.localScale = new Vector3(size, size, size);

        Color fireColor = type == "Electrical" ? Color.blue : Color.red;
        fire.GetComponent<Renderer>().material.color = fireColor;

        firePosition = position;
    }

    private void SpawnBot(GameObject location)
    {
        Instantiate(botPrefab, location.transform.position, Quaternion.identity);
        Debug.Log("Bot spawned in");
    }
}