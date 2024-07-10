
public class GameClearPopup : PopupWithButton
{
    public override void FillContent(string content){
        Content.text = $"최종 Stage을 클리어 했습니다.\n 최종 점수: {content}";
    }
}
