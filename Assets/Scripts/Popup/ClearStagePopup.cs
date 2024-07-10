
public class ClearStagePopup : PopupWithButton
{
    public override void FillContent(string content){
        Content.text = $"Stage{content}을 클리어 했습니다.\n 다음 스테이지로 이동하겠습니까?";
    }
}
