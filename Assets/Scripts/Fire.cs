using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public string fireType {  get; private set; }
    public float fireSize { get; private set; }
    public bool isBurning { get; private set; }

    public void InitializeFire(string type, float size)
    {
        fireSize = size;
        fireType = type;

        isBurning = true;
    }

    public void ExtinguishFire()
    {
        isBurning = false;
    }
}
