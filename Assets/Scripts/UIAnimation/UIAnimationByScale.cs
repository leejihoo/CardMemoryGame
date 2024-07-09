using DG.Tweening;
using UnityEngine;

public class UIAnimationByScale : UIAnimation
{

    public override void ExecuteAnimation(Transform target, float animationDuration, Ease ease)
    {
        target.localScale *= 0.1f;
        target.DOScale(new Vector3(1, 1), animationDuration).SetEase(ease);
    }
}
