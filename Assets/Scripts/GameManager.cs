using System;
using System.Collections;
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
    public bool isPlayingGame;
    public bool isdelay;
    public Card selectedCard;
    public Action OnTimeOut;
    public Action<List<Card>> OnSelectSameCards;
    public Action<List<Card>> OnSelectDifferentCards;
    public Action OnGameStart;
    public Action OnGameClear;
    public Action OnResetGame;

    public TMP_Text timeText;
    public TMP_Text pointText;
    public Button startButton;
    public Button backButton;
    public PopupManager popupManager;

    public override void Awake() {
        base.Awake();
        OnTimeOut += GameOver;
        OnTimeOut += () => popupManager.CreatePopup(PopupType.TIMEOUT, pointSum.ToString());
        OnSelectSameCards += GetPoint;
        OnGameClear += GameOver;
        OnGameClear += () => popupManager.CreatePopup(PopupType.GAMECLEAR, pointSum.ToString());
        
        OnResetGame += HideBackButton;
        OnResetGame += ExposeStartButton;
        OnResetGame += HideText;
        OnResetGame += ResetGameInfo;
        
        OnGameStart += ExposeBackButton;
        OnGameStart += HideStartButton;
        OnGameStart += ExposeText;
    }

    private void Update() {
        
        if(!isGameEnd && isPlayingGame){
            CountDown();
        }
    }

    private void OnDisable() {
        Debug.Log("GameManager Ondisable");
        //OnTimeOut -= GameOver;
        OnTimeOut = null;
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

    private void OnDestroy() {
        Debug.Log("GameManager OnDestroy");
    }

    public void UpdateTimeText(){
        timeText.text = "Time: " + Mathf.Ceil(currentTime).ToString() + " sec";
    }

    public void GetPoint(List<Card> cards){
        if(isPlayingGame){
            pointSum += stagePoint;
            UpdatePointText();
        }
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
        if(selectedCard.cardSO.cardId == card.cardSO.cardId && !System.Object.ReferenceEquals(selectedCard,card)){
            return true;
        }
        return false;
    }
    public void GetCardInfo(Card card){
        if(isExistSeletedCard){
            StartCoroutine(DelayAndHandleCardSelection(card));
            return;
        }
        
        selectedCard = card;
        isExistSeletedCard = true;
    }

     private IEnumerator DelayAndHandleCardSelection(Card card){
        isdelay = true;
        yield return StartCoroutine(Delay());
        List<Card> cards = new List<Card>{ selectedCard, card};
        if(IsSameCard(card)){
            OnSelectSameCards?.Invoke(cards);
        }
        else{    
           OnSelectDifferentCards?.Invoke(cards);
        }
        selectedCard = null;
        isExistSeletedCard = false;
        isdelay = false;         
     }

    public void SetGameInfo(int stagePoint, float time){
        this.time = time;
        currentTime = time;
        this.stagePoint = stagePoint;
        isPlayingGame = true;
    }

    public bool IsTimeOver(float time){
        if(time > 0){
            return false;
        }

        return true;
    }

    public void GameOver(){
        isGameEnd = true;
        isPlayingGame = false;
        isdelay = true;
    }

    public void StartGame(){
        OnGameStart?.Invoke();
    }

    // 뒤로가기 버튼 & 다시하기 버튼을 눌렀을 때
    public void ResetGame(){
        OnResetGame?.Invoke();
    }

    public void HideText(){
        timeText.gameObject.SetActive(false);
        pointText.gameObject.SetActive(false);
    }

    public void ExposeText(){
        timeText.gameObject.SetActive(true);
        pointText.gameObject.SetActive(true);
    }

    public void HideStartButton(){
        startButton.gameObject.SetActive(false);
    }

    public void ExposeStartButton(){
        startButton.gameObject.SetActive(true);
    }

    public void HideBackButton(){
        backButton.gameObject.SetActive(false);
    }

    public void ExposeBackButton(){
        backButton.gameObject.SetActive(true);
    }

    public void ResetGameInfo(){
        pointSum = 0;
        stagePoint = 0;
        time = 0;
        currentTime = 0;
        isExistSeletedCard = false;
        isGameEnd = false;
        selectedCard = null;
        isPlayingGame = false;
        isdelay = false;

        UpdatePointText();
    }

    public IEnumerator Delay(){
        yield return new WaitForSeconds(0.5f);
    }

    public void StopCountDown(){
        isPlayingGame = false;
    }
}

