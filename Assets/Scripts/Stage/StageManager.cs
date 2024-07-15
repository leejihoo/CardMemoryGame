using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Field
    private int _remainingCardPair;
    private int _currentStage;
    private Action OnClearStage;
    private const int _lastStage = 5;
    private Queue<Card> _managedCards = new Queue<Card>();
    [SerializeField] private StageSO[] stageSOArray;
    [SerializeField] private DynamicGridLayoutGroup grid;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private PopupManager popupManager;
    #endregion

    #region LifeCycle
    private void Awake() {
        if(cardManager == null){
            cardManager = FindObjectOfType<CardManager>();
        }

        if(popupManager == null){
            popupManager = FindObjectOfType<PopupManager>();
        }
    }

    private void OnEnable() {
        SubscribeEvents();
    }

    private void OnDisable() {
        UnsubscribeEvents();
    }
    #endregion

    #region Method
    public void MoveNextStage(){
        _currentStage += 1;
        var stageSO = stageSOArray[_currentStage-1];
        GameManager.Instance.SetGameInfo(stageSO.stagePoint,stageSO.stageTimeLimit);
        SetStage(stageSO);
    }

    private bool IsExistNextStage(){
        if(_currentStage == _lastStage){
            return false;
        }
        return true;
    }

    private void RemoveCardPair(List<Card> cards){
        _remainingCardPair -= 1;
        if(!IsExistRemainingCardPair()){
            if(IsExistNextStage()){
                if(GameManager.Instance.IsPlayingGame){
                    OnClearStage?.Invoke();                    
                }
            }
            else{
                GameManager.Instance.OnGameClear?.Invoke();
            }
        }
    }

    private bool IsExistRemainingCardPair(){
        if(_remainingCardPair > 0){
            return true;
        }

        return false;
    }

    private void SetStage(StageSO stageSO){
        // 이전 스테이지 카드 제거
        DeleteAllCards();
        SetBoard(stageSO);
        
        _remainingCardPair = (int)(stageSO.row * stageSO.col * 0.5);
        var stageCards = cardManager.CreateRandomCardDeck(_remainingCardPair);
        InstantiateCardDeck(stageCards);

        UpdateStageText(stageSO);
    }

    private void SetBoard(StageSO stageSO){
        grid.SetCardSize(stageSO.cardWidth, stageSO.cardHeight);
        grid.SetStageSize(stageSO.row);
    }

    private void UpdateStageText(StageSO stageSO){
        stageText.text = "Stage: " + stageSO.stageNumber.ToString();
    }

    private void InstantiateCardDeck(List<Card> cards){
        foreach(var card in cards){
            var instant = Instantiate(card,grid.transform,false);
            SubsribeOnClickCard(instant);
            _managedCards.Enqueue(instant);
        }
    }

    private void SubsribeOnClickCard(Card instant){
        instant.OnClickCard += cardManager.FlipCardToFrontImage;
        instant.OnClickCard += (Card card) => SoundManager.Instance.PlayCommonSfxAt(instant.transform.position,"cardPlace2",false);
        instant.OnClickCard += (Card card) => UIAnimationManager.Instance.ExecuteAnimation(instant.transform,AnimaitonType.Rotation);
    }

    private void DeleteAllCards(){
        while(_managedCards.Count > 0){
            var card = _managedCards.Dequeue();
            card.OnClickCard = null;
            card.transform.DOKill();
            Destroy(card.gameObject);
        }
    }

    private void HideStageText(){
        stageText.gameObject.SetActive(false);
    }

    private void ExposeStageText(){
        stageText.gameObject.SetActive(true);
    }

    private void HideGrid(){
        grid.gameObject.SetActive(false);
    }

    private void ExposeGrid(){
        grid.gameObject.SetActive(true);
    }

    private void ResetStageInfo(){
        _remainingCardPair = 0;
        _currentStage = 0;
    }

        #region Subscribe
    private void SubscribeEvents(){
        SubscribeOnSelectSameCards();
        SubscribeOnGameStart();
        SubscribeOnResetGame();
        SubscribeOnClearStage();
    }

    private void SubscribeOnSelectSameCards(){
        GameManager.Instance.OnSelectSameCards += RemoveCardPair;
    }

    private void SubscribeOnGameStart(){
        GameManager.Instance.OnGameStart += MoveNextStage;
        GameManager.Instance.OnGameStart += ExposeGrid;
        GameManager.Instance.OnGameStart += ExposeStageText;
    }

    private void SubscribeOnResetGame(){
        GameManager.Instance.OnResetGame += HideGrid;
        GameManager.Instance.OnResetGame += HideStageText;
        GameManager.Instance.OnResetGame += ResetStageInfo;
    }

    private void SubscribeOnClearStage(){
        OnClearStage += () => popupManager.CreatePopup(PopupType.STAGECLEAR, _currentStage.ToString());
        OnClearStage += GameManager.Instance.StopCountDown;
        OnClearStage += () => SoundManager.Instance.PlayCommonSfxAt("DM-CGS-26");
    }
        #endregion

        #region Unsubscribe
    private void UnsubscribeEvents(){
        if(GameManager.Instance != null){
            UnsubscribeOnSelectSameCards();
            UnsubscribeOnGameStart();
            UnsubscribeOnResetGame();
        }

        UnsubscribeOnClearStage();
    }

    private void UnsubscribeOnSelectSameCards(){
        GameManager.Instance.OnSelectSameCards -= RemoveCardPair;
    }

    private void UnsubscribeOnGameStart(){
        GameManager.Instance.OnGameStart -= MoveNextStage;
        GameManager.Instance.OnGameStart -= ExposeGrid;
        GameManager.Instance.OnGameStart -= ExposeStageText;
    }

    private void UnsubscribeOnResetGame(){
        GameManager.Instance.OnResetGame -= HideGrid;
        GameManager.Instance.OnResetGame -= HideStageText;
        GameManager.Instance.OnResetGame -= ResetStageInfo;
    }

    private void UnsubscribeOnClearStage(){
        OnClearStage = null;
    }
            #endregion
    
    #endregion
}
