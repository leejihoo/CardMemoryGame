
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<Card> cardDeck;

    private void Awake() {
        GameManager.Instance.OnSelectDifferentCards += FlipCardsToBackImage;
        GameManager.Instance.OnSelectSameCards += UneableCard;
    }
    private void OnDisable() {
        Debug.Log("CardManager Ondisable");
        if(GameManager.Instance != null){
            GameManager.Instance.OnSelectDifferentCards -= FlipCardsToBackImage;
            GameManager.Instance.OnSelectSameCards -= UneableCard;
        }
    }

    public void FlipCardsToBackImage(List<Card> cards){
        foreach(var card in cards){
            card.isFliped = false;
            card.cardImage.sprite = card.cardSO.backImage;
        }
    }

    // 카드를 랜덤 생성할 때 OnClicked에 붙인다.
    public void FlipCardToFrontImage(Card card){
        card.isFliped = true;
        card.cardImage.sprite = card.cardSO.frontImage;
    }

    public List<Card> CreateRandomCardDeck(int cardPairCount){
        List<Card> randomCards = new List<Card>();
        var cardCount = cardPairCount * 2;

        while(randomCards.Count < cardCount){
            var randomNum = Random.Range(0,51);
            var randomCard = cardDeck[randomNum];
            
            if(IsExistDuplicateCard(randomCard.cardSO.cardId,randomCards)){
                continue;
            }

            // 쌍으로 넣는다.
            for(int i = 0; i<2; i++){
                randomCards.Add(randomCard);
            }
        }

        SuffleCardDeck(randomCards);

        return randomCards;
    }

    public void SuffleCardDeck(List<Card> cards){
        // Unity Documentation Random 내용 사용
        for (int i = 0; i < cards.Count; i++) {
            var temp = cards[i];
            var randomIndex = Random.Range(0, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    public void UneableCard(Card card){
        card.enabled = false;
    }

    public bool IsExistDuplicateCard(string cardId, List<Card> randomCards){
        var result = randomCards.Exists(x => x.cardSO.cardId == cardId);
        return result;
    }
}
