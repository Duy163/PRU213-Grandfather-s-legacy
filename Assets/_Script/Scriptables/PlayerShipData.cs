using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerShipData
{
    public Vector3 position;
    public Quaternion rotation;
    public float currentSpeed;
    public float shipDurability;
    public float panicLevel;

    public float acceleration;
    public float maxSpeed;
    public float turnStrength;
    public float waterDrag;
    public float idleDrag;

    // Ship components
    public string engineType;
    public string lightType;
    public string rodType;
    public string netType;
    public string dredgeType;

    // Upgrades
    public List<string> installedUpgrades;
    public int hullTier;
    public int cargoCapacity;

    public PlayerShipData()
    {
        acceleration = 50f;
        maxSpeed = 100f;
        turnStrength = 1.5f;
        waterDrag = 2f;
        idleDrag = 2f;


        position = new Vector3(150, 0, 60);
        rotation = Quaternion.identity;
        currentSpeed = 0f;
        shipDurability = 100f;
        panicLevel = 0f;
        installedUpgrades = new List<string>();
        cargoCapacity = 20;
    }
}
