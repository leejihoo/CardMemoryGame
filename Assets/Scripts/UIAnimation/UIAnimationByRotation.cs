using DG.Tweening;
using UnityEngine;

public class UIAnimationByRotation : UIAnimation
{
    public override void ExecuteAnimation(Transform target, float animationDuration, Ease ease)
    {
        target.DORotate(new Vector3(0, 360f), animationDuration, RotateMode.FastBeyond360).SetEase(ease);
    }
}
