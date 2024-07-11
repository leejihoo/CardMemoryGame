using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardSOSetter : MonoBehaviour
{
    #if UNITY_EDITOR
    public List<CardSO> cardSoList;
    public List<Sprite> cardFront;
    public Sprite cardBack;

    public GameObject card;

    // Start is called before the first frame update
    void Start()
    {
        InsertSO();
    }


    public void FillSO(){
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

    public void InsertSO(){
        for(int i =0; i<cardSoList.Count; i++){
            card.GetComponent<Card>().cardSO = cardSoList[i];
            var localPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/Prefabs/PlayingCards/{cardSoList[i].name}.prefab");
            PrefabUtility.SaveAsPrefabAsset(card, localPath);
        }
    }
    #endif
}
