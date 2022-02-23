using UnityEngine.UIElements;

public class ScoreBoardCard : VisualElement
{
    VisualElement portrait;

    Label playerName;
    Label character;
    Label ship;
    Label kills;
    Label score;
    Label deaths;

    public bool isFriendly = false;
    

    public new class UxmlFactory : UxmlFactory<ScoreBoardCard, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }

    public ScoreBoardCard(){
        this.name = "NewCard";
        this.AddToClassList("cardItem");

        portrait = new VisualElement();
        portrait.AddToClassList("tabPortrait");
        portrait.name = "TabPortrait";

        playerName = new Label();
        playerName.name = "PlayerName";
        playerName.text = "Player Name";
        playerName.AddToClassList("nas1");

        character = new Label();
        character.name = "CharacterName";
        character.text = "Character Name";
        character.AddToClassList("nas1");

        ship = new Label();
        ship.name = "ShipName";
        ship.text = "Ship Name";
        ship.AddToClassList("nas1");

        VisualElement stats = new VisualElement();
        stats.name = "Stats";
        stats.style.flexDirection = FlexDirection.Row;
        stats.style.justifyContent = Justify.SpaceBetween;
        stats.style.width = Length.Percent(100);

        kills = new Label();
        kills.name = "Kills";
        kills.text =  "#";
        kills.AddToClassList("bans1");

        score = new Label();
        score.name = "score";
        score.text =  "#";
        score.AddToClassList("bans1");

        deaths = new Label();
        deaths.name = "deaths";
        deaths.text =  "#";
        deaths.AddToClassList("bans1");

        stats.Add(kills);
        stats.Add(score);
        stats.Add(deaths);

        this.Add(portrait);
        this.Add(playerName); 
        this.Add(character);
        this.Add(ship); 
        this.Add(stats);
    }

    public void SetData(bool friendly, string _player, string _char, string _ship, int _kills, int _score, int _deaths, CharacterHandler handler){
        playerName.text = _player;
        character.text = _char;
        ship.text = _ship;
        kills.text = _kills.ToString();
        score.text = _score.ToString();
        deaths.text = _deaths.ToString();

        portrait.style.backgroundImage = handler.portrait;

        isFriendly = friendly;

        if(isFriendly){
            this.AddToClassList("friendly");
        }
        else{
            this.AddToClassList("enemy");
        }

    }   

}
