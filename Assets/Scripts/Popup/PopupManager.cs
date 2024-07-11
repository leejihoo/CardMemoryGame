using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Video;

public class PopupManager : MonoBehaviour
{
    #region Field
    private Stack<GameObject> _managingPopups;
    [SerializeField] private PopupFactory popupFactory;
    #endregion

    #region LifeCycle
    private void Awake() {
        _managingPopups = new Stack<GameObject>();
    }
    #endregion

    #region Method
    private void RemovePopup(){
        if(_managingPopups.Count > 0){
            var popup = _managingPopups.Pop();
            popup.GetComponent<PopupWithButton>().OnClicked = null;
            popup.transform.DOKill();
            popup.GetComponent<CanvasGroup>()?.DOKill();
            Destroy(popup);
        }
    }

    public void CreatePopup(PopupType popupType, string content){
        var popup = popupFactory.GetPopup(popupType);
        var PopupWithButton = popup.GetComponent<PopupWithButton>();
        ListenButtonEvent(PopupWithButton);
        PopupWithButton.FillContent(content);
        _managingPopups.Push(popup);
    }

    private void ListenButtonEvent(PopupWithButton popupWithButton){
        popupWithButton.OnClicked += RemovePopup;
        popupWithButton.OnClicked += () => SoundManager.Instance.PlayCommonSfxAt("SFX_UI_Button_Mouse_Thick_Generic_1");
    }
    #endregion
}
