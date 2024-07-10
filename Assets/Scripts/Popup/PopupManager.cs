using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public Stack<GameObject> managingPopups;
    public PopupFactory popupFactory;

    public StageManager stageManager;

    private void Awake() {
        managingPopups = new Stack<GameObject>();
    }

    public void RemovePopup(){
        if(managingPopups.Count > 0){
            var popup = managingPopups.Pop();
            popup.GetComponent<PopupWithButton>().OnClicked = null;
            Destroy(popup);
        }
    }

    public void CreatePopup(PopupType popupType, string content){
        var popup = popupFactory.GetPopup(popupType);
        //popup.GetComponent<PopupWithButton>().OnClicked += stageManager.MoveNextStage;
        var PopupWithButton = popup.GetComponent<PopupWithButton>();
        PopupWithButton.OnClicked += RemovePopup;
        PopupWithButton.FillContent(content);
        managingPopups.Push(popup);
    }
}
