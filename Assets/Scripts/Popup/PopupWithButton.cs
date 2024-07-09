using System;
using UnityEngine.UI;

public abstract class PopupWithButton : Popup
{
    private void Awake() {
        Button.onClick.AddListener(ClickButton);
    }
    
    public Button Button;
    public Action OnClicked;
    public virtual void ClickButton(){
        OnClicked?.Invoke();
    }
}
