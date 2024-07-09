using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageSO[] stageSOArray;
    public int remainingCardPair;
    public int currentStage;
    public Action ClearStage;
    public DynamicGridLayoutGroup grid;
    public const int lastStage = 3;

    private void Awake() {
        GameManager.Instance.OnSelectSameCards += RemoveCardPair;
    }

    private void OnDestroy() {
        GameManager.Instance.OnSelectSameCards -= RemoveCardPair;
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
    public void RemoveCardPair(){
        remainingCardPair -= 1;

        if(!IsExistRemainingCardPair()){
            if(IsExistNextStage()){
                ClearStage?.Invoke();
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
        grid.SetCardSize(stageSO.cardWidth, stageSO.cardHeight);
        grid.SetStageSize(stageSO.row);
    }
}
