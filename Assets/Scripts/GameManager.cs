using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    #region Field
    private int _pointSum;
    private int _stagePoint;
    private float _time;
    private float _currentTime;
    private bool _isExistSeletedCard;
    private Card _selectedCard;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button backButton;
    [SerializeField] private PopupManager popupManager;
    public bool IsPlayingGame{ get; private set; }
    public bool Isdelay { get; private set; }
    public Action OnTimeOut;
    public Action<List<Card>> OnSelectSameCards;
    public Action<List<Card>> OnSelectDifferentCards;
    public Action OnGameStart;
    public Action OnGameClear;
    public Action OnResetGame;
    #endregion

    #region LifeCycle
    private void OnEnable() {
        SubscribeEvents();
    }

    private void Update() {
        if(IsPlayingGame){
            CountDown();
        }
    }

    private void OnDisable() {
        UnsubscribeEvents();
    }
    #endregion

    #region Method
    private void UpdateTimeText(){
        timeText.text = "Time: " + Mathf.Ceil(_currentTime).ToString() + " sec";
    }

    private void GetPoint(List<Card> cards){
        if(IsPlayingGame){
            _pointSum += _stagePoint;
            UpdatePointText();
        }
    }

    private void UpdatePointText(){
        pointText.text = "Point: " + _pointSum;
    }

    private void CountDown(){
        _currentTime -= Time.deltaTime;
        if(IsTimeOver(_currentTime)){
            _currentTime = 0;
            OnTimeOut?.Invoke();
        }

        UpdateTimeText();
    }

    private bool IsSameCard(Card card){
        // 자기자신을 선택하면 안된다.
        if(_selectedCard.cardSO.cardId == card.cardSO.cardId && !System.Object.ReferenceEquals(_selectedCard,card)){
            return true;
        }
        return false;
    }

    public void GetCardInfo(Card card){
        if(_isExistSeletedCard){
            StartCoroutine(DelayAndHandleCardSelection(card));
            return;
        }
        
        _selectedCard = card;
        _isExistSeletedCard = true;
    }

     private IEnumerator DelayAndHandleCardSelection(Card card){
        Isdelay = true;
        yield return StartCoroutine(Delay());
        List<Card> cards = new List<Card>{ _selectedCard, card};
        
        if(!IsPlayingGame){
            yield break;;
        }

        if(IsSameCard(card)){
            OnSelectSameCards?.Invoke(cards);
        }
        else{    
           OnSelectDifferentCards?.Invoke(cards);
        }

        _selectedCard = null;
        _isExistSeletedCard = false;
        Isdelay = false;         
     }

    public void SetGameInfo(int stagePoint, float time){
        this._time = time;
        _currentTime = time;
        this._stagePoint = stagePoint;
        IsPlayingGame = true;
    }

    private bool IsTimeOver(float time){
        if(time > 0){
            return false;
        }

        return true;
    }

    private void GameOver(){
        IsPlayingGame = false;
        Isdelay = true;
    }

    // 계층 창의 start button event에 등록 됨
    public void StartGame(){
        OnGameStart?.Invoke();
    }

    // 뒤로가기 버튼 & 다시하기 버튼을 눌렀을 때
    public void ResetGame(){
        OnResetGame?.Invoke();
    }

    private void HideText(){
        timeText.gameObject.SetActive(false);
        pointText.gameObject.SetActive(false);
    }

    private void ExposeText(){
        timeText.gameObject.SetActive(true);
        pointText.gameObject.SetActive(true);
    }

    private void HideStartButton(){
        startButton.gameObject.SetActive(false);
    }

    private void ExposeStartButton(){
        startButton.gameObject.SetActive(true);
    }

    private void HideBackButton(){
        backButton.gameObject.SetActive(false);
    }

    private void ExposeBackButton(){
        backButton.gameObject.SetActive(true);
    }

    private void ResetGameInfo(){
        _pointSum = 0;
        _stagePoint = 0;
        _time = 0;
        _currentTime = 0;
        _isExistSeletedCard = false;
        _selectedCard = null;
        IsPlayingGame = false;
        Isdelay = false;

        UpdatePointText();
    }

    private IEnumerator Delay(){
        yield return new WaitForSeconds(0.8f);
    }

    public void StopCountDown(){
        IsPlayingGame = false;
    }

        #region Subscribe
    public void SubscribeEvents(){
        SubscribeOnTimeOut();
        SubscribeOnSelectSameCards();
        SubscribeOnGameClear();
        SubscribeOnResetGame();
        SubscribeOnGameStart();
        SubscribeOnSelectDifferentCards();
    }

    public void SubscribeOnTimeOut(){
        OnTimeOut += GameOver;
        OnTimeOut += () => popupManager.CreatePopup(PopupType.TIMEOUT, _pointSum.ToString());
        OnTimeOut += () => SoundManager.Instance.PlayCommonSfxAt("DM-CGS-23");
    }
    public void SubscribeOnSelectSameCards(){
        OnSelectSameCards += GetPoint;
        OnSelectSameCards += (List<Card> cards) => SoundManager.Instance.PlayCommonSfxAt("coin_1_a");
    }

    public void SubscribeOnGameClear(){
        OnGameClear += GameOver;
        OnGameClear += () => popupManager.CreatePopup(PopupType.GAMECLEAR, _pointSum.ToString());
        OnGameClear += () => SoundManager.Instance.PlayCommonSfxAt("DM-CGS-26");
    }

    public void SubscribeOnResetGame(){
        OnResetGame += HideBackButton;
        OnResetGame += ExposeStartButton;
        OnResetGame += HideText;
        OnResetGame += ResetGameInfo;
    }    

    public void SubscribeOnGameStart(){
        OnGameStart += ExposeBackButton;
        OnGameStart += HideStartButton;
        OnGameStart += ExposeText;
    }

    public void SubscribeOnSelectDifferentCards(){
        OnSelectDifferentCards += (List<Card> cards) => SoundManager.Instance.PlayCommonSfxAt("beep_1_a");
    }
        #endregion

        #region Unsubscribe
    public void UnsubscribeEvents(){
        UnsubscribeOnTimeOut();
        UnsubscribeOnSelectSameCards();
        UnsubscribeOnGameClear();
        UnsubscribeOnResetGame();
        UnsubscribeOnGameStart();
        UnsubscribeOnSelectDifferentCards();
    }

    public void UnsubscribeOnTimeOut(){
        OnTimeOut = null;
    }
    public void UnsubscribeOnSelectSameCards(){
        OnSelectSameCards = null;
    }

    public void UnsubscribeOnGameClear(){
        OnGameClear = null;
    }

    public void UnsubscribeOnResetGame(){
        OnResetGame = null;

    }    

    public void UnsubscribeOnGameStart(){
        OnGameStart = ExposeBackButton;

    }

    public void UnsubscribeOnSelectDifferentCards(){
        OnSelectDifferentCards = null;
    }
        #endregion
    #endregion
}

