using DG.Tweening;
using UnityEngine;

public class UIAnimationByAlpha : UIAnimation
{
    public override void ExecuteAnimation(Transform target, float animationDuration, Ease ease)
    {
        target.GetComponent<CanvasGroup>().DOFade(1f, animationDuration).SetEase(ease);
    }
}
