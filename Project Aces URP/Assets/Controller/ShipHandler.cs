using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Controller/Ship")]
public class ShipHandler : ScriptableObject
{
    public string shipName;
    public Sprite shipIcon;
    public GameObject shipPrefab;
    [HideInInspector]
    public WeaponsController weaponsController;
    [HideInInspector]
    public CameraController cameraController;

    [Header("Ship Stats")]
    public float maxHealth = 2000;
    public float
        acceleration = 10,
        minSpeed = 67,
        cruiseSpeed = 357,
        maxSpeed = 500,
        roll = 12, 
        pitch = 15, 
        yaw = 3;

    private void Awake() {
        weaponsController = shipPrefab.GetComponent<WeaponsController>();
    }
}
