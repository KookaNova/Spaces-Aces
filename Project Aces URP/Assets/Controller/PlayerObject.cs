using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cox.PlayerControls{
[CreateAssetMenu(menuName = "Controller/Player")]
public class PlayerObject : ScriptableObject
{
    public CharacterHandler chosenCharacter;
    public ShipHandler chosenShip;

    public void ChangeCharacter(CharacterHandler newCharacter){
        chosenCharacter = newCharacter;
    }
    public void ChangeShip(ShipHandler newShip){
        chosenShip = newShip;
    }

}
}
