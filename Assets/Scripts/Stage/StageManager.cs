using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageSO[] stageSOArray;
    public int remainingCardPair;
    public int currentStage;
    public Action ClearStage;
    public DynamicGridLayoutGroup grid;
}
