public class TimeOutPopup : PopupWithButton
{
    public override void FillContent(string content){
        Content.text = $"시간 제한이 끝났습니다.\n 최종 점수: {content}";
    }
}
