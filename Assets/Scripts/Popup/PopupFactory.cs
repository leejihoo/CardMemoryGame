using UnityEngine;

public class PopupFactory : MonoBehaviour
{
    public GameObject gameClearPopup;
    public GameObject clearStagePopup;
    public GameObject timeOutPopup;
    private StageManager stageManager;

    private void OnEnable() {
        stageManager = FindObjectOfType<StageManager>();
    }
    
    public GameObject GetPopup(PopupType popupType){
        var parent = GameObject.Find("Canvas").transform;

        if(popupType == PopupType.GAMECLEAR){
            var popup = Instantiate(gameClearPopup, parent);
            ListenResetButtonEvent(popup);
            return popup;
        }
        else if(popupType == PopupType.STAGECLEAR){
            var popup = Instantiate(clearStagePopup, parent);
            ListenMoveStageButtonEvent(popup);
            return popup;
        }
        else if(popupType == PopupType.TIMEOUT){
            var popup = Instantiate(timeOutPopup, parent);
            ListenResetButtonEvent(popup);
            return popup;
        }

        return null;
    }

    public void ListenResetButtonEvent(GameObject popup){
        popup.GetComponent<PopupWithButton>().OnClicked += GameManager.Instance.ResetGame;
    }

    public void ListenMoveStageButtonEvent(GameObject popup){
        popup.GetComponent<PopupWithButton>().OnClicked += stageManager.MoveNextStage;
    }

    // public void Temp(){
    //     Debug.Log("temp test");
    // }
}
