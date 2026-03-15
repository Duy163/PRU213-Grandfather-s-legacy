using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerShipData
{
    public Vector3 position;
    public Vector3 rotation;

    public float acceleration;
    public float maxSpeed;
    public float turnStrength;
    public float waterDrag;
    public float idleDrag;

    // Ship components
    public int maxSpeedLv;
    public int accelerationLv;
    public int turnStrengthLv;
    public bool hasLamp;


    public PlayerShipData()
    {
        maxSpeed = 30f;
        acceleration = 20f;
        turnStrength = 1f;
        waterDrag = 2f;
        idleDrag = 2f;

        maxSpeedLv = 0;
        accelerationLv = 0;
        turnStrengthLv = 0;
        hasLamp = false;

        position = new Vector3(150, 0, 60);
        rotation = new Vector3(0, 0, 0);
    }
}
