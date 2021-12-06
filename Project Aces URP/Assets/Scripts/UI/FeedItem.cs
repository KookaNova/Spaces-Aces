using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FeedItem : VisualElement
{
    Label leftName;
    Label action;
    Label rightName;
    

    public new class UxmlFactory : UxmlFactory<FeedItem, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }

    public FeedItem(){
        this.name = "FeedBody";
        this.AddToClassList("feedItem");

        leftName = new Label();
        leftName.text = "SideA";
        leftName.name = "LeftName";
        leftName.AddToClassList("nas1");

        action = new Label();
        action.text = " => ";
        action.name = "Action";
        action.AddToClassList("nas1");

        rightName = new Label();
        rightName.text = "SideB";
        rightName.name = "RightName";
        rightName.AddToClassList("nas1");

        this.Add(leftName); 
        this.Add(action);
        this.Add(rightName); 


    }

    public void SetData(string _leftName, string _rightName, string _action){
        leftName.text = _leftName;
        rightName.text = _rightName;

        action.text = _action;
    }   

    public IEnumerator feedTimer(){
        yield return new WaitForSeconds(3);
        this.AddToClassList("opacityOut");

        this.RegisterCallback<TransitionEndEvent>(ev => RemoveFromHierarchy());

        

        
    }

}
