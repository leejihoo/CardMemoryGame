using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour,IPointerClickHandler
{
    #region Field
    public CardSO cardSO;
    public Image cardImage;
    public bool isFliped;
    public Action<Card> OnClickCard;
    #endregion

    #region LifeCycle
    private void Awake() {
        cardImage.sprite = cardSO.backImage;  
    }

    private void OnEnable() {
        OnClickCard += GameManager.Instance.GetCardInfo;
    }

    private void OnDisable() {
        if(GameManager.Instance != null){
            OnClickCard -= GameManager.Instance.GetCardInfo;
        }
    }
    #endregion

    #region Method
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isFliped && !GameManager.Instance.Isdelay){
            OnClickCard?.Invoke(this);
        }
    }
    #endregion
}
