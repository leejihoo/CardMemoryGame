using UnityEngine;

[CreateAssetMenu(fileName ="StageSO",menuName ="CreateStageSO")]
public class StageSO : ScriptableObject
{
    public int stageNumber;
    public int stagePoint;
    public float stageTimeLimit;
    public int row;
    public int col;
    public int cardHeight;
    public int cardWidth; 
}
