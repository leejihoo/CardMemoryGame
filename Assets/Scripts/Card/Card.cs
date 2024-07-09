using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour,IPointerClickHandler
{
    public CardSO cardSO;
    public Image cardImage;
    public bool isFliped;
    public Action<Card> OnClickCard;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickCard?.Invoke(this);
    }
}
