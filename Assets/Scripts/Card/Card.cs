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
        OnClickCard += GameManager.Instance.GetCardInfo;
    }

    private void OnDestroy() {
        OnClickCard -= GameManager.Instance.GetCardInfo;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isFliped){
            OnClickCard?.Invoke(this);
        }
    }
}
