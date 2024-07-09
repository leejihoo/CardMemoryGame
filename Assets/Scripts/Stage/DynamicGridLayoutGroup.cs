using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayoutGroup : MonoBehaviour
{
    public GridLayoutGroup grid;
    public void SetStageSize(){

    }
    public void SetCardSize(){
        
    public void SetStageSize(int row){
        grid.constraintCount = row;
    }
    public void SetCardSize(float Width, float Height){
        grid.cellSize = new Vector2(Width,Height);
    }
}
