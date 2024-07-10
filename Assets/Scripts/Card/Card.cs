using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour,IPointerClickHandler
{
    public CardSO cardSO;
    public Image cardImage;
    public bool isFliped;
    public Action<Card> OnClickCard;

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
    private void OnDestroy() {
        OnClickCard -= GameManager.Instance.GetCardInfo;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isFliped && !GameManager.Instance.isdelay){
            OnClickCard?.Invoke(this);
        }
    }
}
