using System.Collections;
using System.Collections.Generic;
using Cox.PlayerControls;
using UnityEngine;


///<summary> Stores information about a ship and delivers it to the Cox.PlayerControls.SpacecraftController. </summary>
[CreateAssetMenu(menuName = "Controller/Ship")]
public class ShipHandler : ScriptableObject
{
    public string shipName;
    [TextArea( 2, 50 )]
    public string bio = "Ship bio...";
    public Sprite shipIcon, nameArt;
    public GameObject shipPrefab, displayShip;
    [HideInInspector]
    public WeaponsController weaponsController;
    [HideInInspector]
    public CameraController cameraController;

    [Header("Ship Stats")]
    public float maxHealth = 700;
    public float
        maxShield = 1300,
        shieldRechargeRate = 100,
        acceleration = 30,
        minSpeed = 25,
        maxSpeed = 500,
        roll = 45, 
        pitch = 55, 
        yaw = 25;

    private void Awake() {
        weaponsController = shipPrefab.GetComponent<WeaponsController>();
    }
}
