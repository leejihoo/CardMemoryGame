using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayoutGroup : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup grid;

    public void SetStageSize(int row){
        grid.constraintCount = row;
    }

    public void SetCardSize(float Width, float Height){
        if(grid == null){
            grid = GetComponent<GridLayoutGroup>();
        }
        grid.cellSize = new Vector2(Width,Height);
    }
}
