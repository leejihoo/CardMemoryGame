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
            UIAnimationManager.Instance.ExecuteAnimation(popup.transform,AnimaitonType.Scale);
            ListenResetButtonEvent(popup);
            return popup;
        }
        else if(popupType == PopupType.STAGECLEAR){
            var popup = Instantiate(clearStagePopup, parent);
            UIAnimationManager.Instance.ExecuteAnimation(popup.transform,AnimaitonType.Scale);
            ListenMoveStageButtonEvent(popup);
            return popup;
        }
        else if(popupType == PopupType.TIMEOUT){
            var popup = Instantiate(timeOutPopup, parent);
            UIAnimationManager.Instance.ExecuteAnimation(popup.transform,AnimaitonType.Alpha);
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
