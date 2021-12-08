using System;
using UnityEngine.UIElements;

public class ProfileIcon : VisualElement
{
    //define elements
    public Image emblem, nameplate;
    public Label nameLabel, levelLabel;
    public string playerName = "newPlayer";
    public int level = -5;

    public new class UxmlFactory : UxmlFactory<ProfileIcon, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }

    public ProfileIcon(){
        //create new image element
        PlayerProfileData data = SaveData.LoadProfile();
        playerName = data.profileName;
        level = data.currentLevel;

        emblem = new Image();
        emblem.name = "emblem";
        nameplate = new Image();
        nameplate.name = "nameplate";
        nameLabel = new Label();
        nameLabel.name = "playerName";
        levelLabel = new Label();
        levelLabel.name = "currentLevel";
        //add image to root
        Add(nameplate);
        var left = new VisualElement();
        var right = new VisualElement();
        nameplate.Add(left);
        nameplate.Add(right);
        left.style.flexDirection = FlexDirection.Row;
        left.style.alignItems = Align.Center;
        left.style.justifyContent = Justify.FlexStart;
        right.style.flexDirection = FlexDirection.Row;
        right.style.justifyContent = Justify.FlexEnd;
        left.Add(emblem);
        left.Add(nameLabel);
        right.Add(levelLabel);
        nameLabel.text = playerName;
        levelLabel.text = level.ToString();
        //add a class to the image so it can be controlled by USS
        emblem.AddToClassList("p_Emblem");
        nameplate.AddToClassList("p_Nameplate");
        nameLabel.AddToClassList("p_Name");
        levelLabel.AddToClassList("p_Level");

    }
}
