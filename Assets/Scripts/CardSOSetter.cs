using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardSOSetter : MonoBehaviour
{
    public List<CardSO> cardSoList;
    public List<Sprite> cardFront;
    public Sprite cardBack;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<cardSoList.Count; i++){
            var cardName = cardSoList[i].cardPatternInfo.ToString() + "_" + cardSoList[i].cardNumberInfo.ToString();
            cardSoList[i].cardId = cardName;
            cardSoList[i].name = cardName;
            cardSoList[i].backImage = cardBack;
            cardSoList[i].frontImage = cardFront[i];

            string assetPath = AssetDatabase.GetAssetPath(cardSoList[i]);
            AssetDatabase.RenameAsset(assetPath, cardName);
        }

        AssetDatabase.SaveAssets();
    }

}
