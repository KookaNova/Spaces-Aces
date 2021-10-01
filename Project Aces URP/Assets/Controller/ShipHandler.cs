using System.Collections;
using System.Collections.Generic;
using Cox.PlayerControls;
using UnityEngine;


///<summary> Stores information about a ship and delivers it to the Cox.PlayerControls.SpacecraftController. </summary>
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
    public float maxHealth = 700;
    public float
        maxShield = 1300,
        shieldRechargeRate = 15,
        acceleration = 10,
        minSpeed = 67,
        maxSpeed = 500,
        roll = 12, 
        pitch = 15, 
        yaw = 3;

    private void Awake() {
        weaponsController = shipPrefab.GetComponent<WeaponsController>();
    }
}
