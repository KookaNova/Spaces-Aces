using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerProfileData : ScriptableObject
{
    public string accountName;
    public Color  namePlateColor;
    public Sprite namePlate, icon;

    public int coin, premium, currentXp, currentLevel, levelUpPoint;

    public void AddExp(int experience){
        if(currentXp + experience >= levelUpPoint){
            currentXp = (currentXp + experience)-levelUpPoint;
            LevelUp();

        }


    }

    public void ChangeName(string newName){
        accountName = newName;
    }

    private void LevelUp(){
        currentLevel++;
    }


}
