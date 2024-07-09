using DG.Tweening;
using UnityEngine;

public abstract class UIAnimation
{
    public abstract void ExecuteAnimation(Transform target, float animationDuration, Ease ease);

    public static UIAnimation GetUIAnimation(AnimaitonType animaitonType)
    {
        if (animaitonType == AnimaitonType.Scale)
        {
            return new UIAnimationByScale();
        }
        else if (animaitonType == AnimaitonType.Alpha)
        {
            return new UIAnimationByAlpha();
        }
        else if (animaitonType == AnimaitonType.Rotation)
        {
            return new UIAnimationByRotation();
        }

        return null;
    }
}