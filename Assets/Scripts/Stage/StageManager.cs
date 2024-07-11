using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageSO[] stageSOArray;
    public int remainingCardPair;
    public int currentStage;
    public Action ClearStage;
    public DynamicGridLayoutGroup grid;
    public const int lastStage = 3;

    public CardManager cardManager;

    public Queue<Card> managedCards = new Queue<Card>();

    public TMP_Text stageText;

    public PopupManager popupManager;

    private void Awake() {
        if(cardManager == null){
            cardManager = FindObjectOfType<CardManager>();
        }

        GameManager.Instance.OnSelectSameCards += RemoveCardPair;
        GameManager.Instance.OnGameStart += MoveNextStage;
        GameManager.Instance.OnGameStart += ExposeGrid;
        GameManager.Instance.OnGameStart += ExposeStageText;
        GameManager.Instance.OnResetGame += HideGrid;
        GameManager.Instance.OnResetGame += HideStageText;
        GameManager.Instance.OnResetGame += ResetStageInfo;
        ClearStage += () => popupManager.CreatePopup(PopupType.STAGECLEAR, currentStage.ToString());
        ClearStage += GameManager.Instance.StopCountDown;
    }

    private void OnDisable() {
        if(GameManager.Instance != null){
            GameManager.Instance.OnSelectSameCards -= RemoveCardPair;
            GameManager.Instance.OnGameStart -= MoveNextStage;
            GameManager.Instance.OnGameStart -= ExposeGrid;
            GameManager.Instance.OnGameStart -= ExposeStageText;
            GameManager.Instance.OnResetGame -= HideGrid;
            GameManager.Instance.OnResetGame -= HideStageText;
            GameManager.Instance.OnResetGame -= ResetStageInfo;
        }
        
        ClearStage = null;
    }

    private void OnDestroy() {
        
    }

    public void MoveNextStage(){
        currentStage += 1;
        var stageSO = stageSOArray[currentStage-1];
        GameManager.Instance.SetGameInfo(stageSO.stagePoint,stageSO.stageTimeLimit);
        SetStage(stageSO);
    }

    public bool IsExistNextStage(){
        if(currentStage == lastStage){
            return false;
        }
        return true;
    }

    public void RemoveCardPair(List<Card> cards){
        remainingCardPair -= 1;
        if(!IsExistRemainingCardPair()){
            if(IsExistNextStage()){
                if(GameManager.Instance.isPlayingGame){
                    ClearStage?.Invoke();                    
                }
            }
            else{
                GameManager.Instance.OnGameClear?.Invoke();
            }
        }
    }
    public bool IsExistRemainingCardPair(){
        if(remainingCardPair > 0){
            return true;
        }

        return false;
    }

    public void SetStage(StageSO stageSO){
        DeleteAllCards();

        //grid.EableSizeFitter();
        grid.SetCardSize(stageSO.cardWidth, stageSO.cardHeight);
        grid.SetStageSize(stageSO.row);

        remainingCardPair = (int)(stageSO.row * stageSO.col * 0.5);
        var stageCards = cardManager.CreateRandomCardDeck(remainingCardPair);
        InstantiateCardDeck(stageCards);
        
        //grid.uneableSizeFitter();

        stageText.text = "Stage: " + stageSO.stageNumber.ToString();
        
    }

    public void InstantiateCardDeck(List<Card> cards){
        foreach(var card in cards){
            var instant = Instantiate(card,grid.transform,false);
            instant.OnClickCard += cardManager.FlipCardToFrontImage;
            instant.OnClickCard += (Card card) => SoundManager.Instance.PlayCommonMonsterSfxAt(instant.transform.position,"cardPlace2",false);
            managedCards.Enqueue(instant);
        }
    }

    public void DeleteAllCards(){
        while(managedCards.Count > 0){
            var card = managedCards.Dequeue();
        instant.OnClickCard += (Card card) => UIAnimationManager.Instance.ExecuteAnimation(instant.transform,AnimaitonType.Rotation);
            card.OnClickCard = null;
            Destroy(card.gameObject);
        }
    }

    public void HideStageText(){
        stageText.gameObject.SetActive(false);
    }

    public void ExposeStageText(){
        stageText.gameObject.SetActive(true);
    }

    public void HideGrid(){
        grid.gameObject.SetActive(false);
    }

    public void ExposeGrid(){
        grid.gameObject.SetActive(true);
    }

    public void ResetStageInfo(){
        remainingCardPair = 0;
        currentStage = 0;
    }
}
