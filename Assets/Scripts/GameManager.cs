using System;

public class GameManager : Singleton<GameManager>
{
    public int pointSum;
    public int stagePoint;
    public float time;
    public bool isExistSeletedCard;
    public Card selectedCardModel;
    public Action OnTimeOut;
    public Action OnSelectSameCards;
    public Action OnSelectDifferentCards;
    public Action OnGameStart;

    public void GetPoint(){

    }
    public void CountDown(){

    }
    public bool IsSameCard(){
        return false;
    }
    public void GetCardInfo(Card card){

    }
    public void SetGameInfo(int stagePoint, float time){

    }
}

