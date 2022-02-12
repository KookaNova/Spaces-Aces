using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Subtitle : VisualElement
{
    Label description;

    public new class UxmlFactory : UxmlFactory<Subtitle, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }

    public Subtitle(){
        description = new Label();
        description.text = "Character: Words spoken by character.";
        description.AddToClassList("subtitle");
        this.Add(description);
    }

    public void SetData(string subtitle){
        description.text = subtitle;
    }

    public IEnumerator Timer(){
        yield return new WaitForSeconds(3);
        this.AddToClassList("opacityOut");

        this.RegisterCallback<TransitionEndEvent>(ev => RemoveFromHierarchy());
        
    }
}