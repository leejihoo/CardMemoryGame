using UnityEngine;

[CreateAssetMenu(fileName = "CardSO", menuName = "CreateCardSO")]
public class CardSO : ScriptableObject
{
    public CardNumber cardNumberInfo;
    public CardPattern cardPatternInfo;
    public string cardId;
    public Sprite backImage;
    public Sprite frontImage;
}
