using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int pointSum;
    public int stagePoint;
    public float time;
    public float currentTime;
    public bool isExistSeletedCard;
    public bool isGameEnd;
    public Card selectedCard;
    public Action OnTimeOut;
    public Action OnSelectSameCards;
    public Action<List<Card>> OnSelectDifferentCards;
    public Action OnGameStart;
    public Action OnGameClear;
    public Action OnResetGame;

    public TMP_Text timeText;
    public TMP_Text pointText;
    public Button startButton;
    public Button backButton;

    public override void Awake() {
        base.Awake();
        OnTimeOut += GameOver;
        OnSelectSameCards += GetPoint;
        OnGameClear += GameOver;
        
        OnResetGame += HideBackButton;
        OnResetGame += ExposeStartButton;
        OnResetGame += HideText;
        OnResetGame += ResetGameInfo;
        
        OnGameStart += ExposeBackButton;
        OnGameStart += HideStartButton;
        OnGameStart += ExposeText;
    }

    private void Update() {
        if(!isGameEnd){
            CountDown();
        }
    }

    private void OnDestroy() {
        OnTimeOut -= GameOver;
        OnSelectSameCards -= GetPoint;
        OnGameClear -= GameOver;
        OnResetGame -= HideBackButton;
        OnResetGame -= ExposeStartButton;
        OnResetGame -= HideText;
        OnResetGame -= ResetGameInfo;

        OnGameStart -= ExposeBackButton;
        OnGameStart -= HideStartButton;
        OnGameStart -= ExposeText;
    }

    public void UpdateTimeText(){
        timeText.text = "Time: " + Mathf.Ceil(currentTime).ToString() + " sec";
    }

    public void GetPoint(){
        pointSum += stagePoint;
        UpdatePointText();
    }

    public void UpdatePointText(){
        pointText.text = "Point: " + pointSum;
    }

    public void CountDown(){
        currentTime -= Time.deltaTime;
        if(IsTimeOver(currentTime)){
            currentTime = 0;
            OnTimeOut?.Invoke();
        }

        UpdateTimeText();
    }

    public bool IsSameCard(Card card){
        if(selectedCard.cardSO.cardId == card.cardSO.cardId){
            return true;
        }
        return false;
    }
    public void GetCardInfo(Card card){
        if(isExistSeletedCard){
            if(IsSameCard(card)){
                OnSelectSameCards?.Invoke();
            }
            else{
                List<Card> cards = new List<Card>
                {
                    selectedCard,
                    card
                };
                OnSelectDifferentCards?.Invoke(cards);
            }
            selectedCard = null;
            isExistSeletedCard = false;
            
            return;
        }
        
        selectedCard = card;
        isExistSeletedCard = true;
    }

    public void SetGameInfo(int stagePoint, float time){
        this.time = time;
        this.stagePoint = stagePoint;
    }

    public bool IsTimeOver(float time){
        if(time > 0){
            return false;
        }

        return true;
    }

    public void GameOver(){
        isGameEnd = true;
    }

    public void StartGame(){
        OnGameStart?.Invoke();
    }

    // 뒤로가기 버튼 & 다시하기 버튼을 눌렀을 때
    public void ResetGame(){
        OnResetGame?.Invoke();
    }

    public void HideText(){
        timeText.enabled = false;
        pointText.enabled = false;
    }

    public void ExposeText(){
        timeText.enabled = true;
        pointText.enabled = true;
    }

    public void HideStartButton(){
        startButton.enabled = false;
    }

    public void ExposeStartButton(){
        startButton.enabled = true;
    }

    public void HideBackButton(){
        backButton.enabled = false;
    }

    public void ExposeBackButton(){
        backButton.enabled = true;
    }

    public void ResetGameInfo(){
        pointSum = 0;
        stagePoint = 0;
        time = 0;
        currentTime = 0;
        isExistSeletedCard = false;
        isGameEnd = false;
        selectedCard = null;
    }
}

