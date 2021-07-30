using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Controller/Ship")]
public class ShipHandler : ScriptableObject
{
    public string shipName;
    public Sprite shipIcon;
    public GameObject shipPrefab;
    public WeaponsController weaponsController;

    private void Awake() {
        weaponsController = shipPrefab.GetComponent<WeaponsController>();
    }
}
