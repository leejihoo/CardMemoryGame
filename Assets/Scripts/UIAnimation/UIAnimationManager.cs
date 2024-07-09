using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIAnimationManager : Singleton<UIAnimationManager>
{
    [Range(0,10)]
    [SerializeField] private float animationDuration;

    [SerializeField] private Ease easeForScale;
    [SerializeField] private Ease easeForAlpha;
    [SerializeField] private Ease easeForRotation;
    private Dictionary<AnimaitonType, UIAnimation> dictionary;

    public override void Awake()
    {
        base.Awake();
        dictionary = new Dictionary<AnimaitonType, UIAnimation>();
        foreach (var value in Enum.GetValues(typeof(AnimaitonType)))
        {
            var animationType = (AnimaitonType)value;
            dictionary.Add(animationType, UIAnimation.GetUIAnimation(animationType));
        }
        
        if (animationDuration == 0)
        {
            animationDuration = 0.3f;
        }

        if (easeForScale == Ease.Unset)
        {
            easeForScale = Ease.OutBack;
        }
        
        if (easeForAlpha == Ease.Unset)
        {
            easeForAlpha = Ease.InCubic;
        }
        
        if (easeForRotation == Ease.Unset)
        {
            easeForRotation = Ease.Linear;
        }
    }

    public void ExecuteAnimation(Transform target, float duration, Ease ease, AnimaitonType animaitonType)
    {
        var targetAnimation = dictionary[animaitonType];
        targetAnimation.ExecuteAnimation(target,duration,ease);
    }

    public void ExecuteAnimation(Transform target, AnimaitonType animaitonType)
    {
        var targetAnimation = dictionary[animaitonType];
        targetAnimation.ExecuteAnimation(target,animationDuration,GetEase(animaitonType));
    }

    private Ease GetEase(AnimaitonType animaitonType)
    {
        if (animaitonType == AnimaitonType.Scale)
        {
            return easeForScale;
        }
        else if (animaitonType == AnimaitonType.Alpha)
        {
            return easeForAlpha;
        }
        else if (animaitonType == AnimaitonType.Rotation)
        {
            return easeForRotation;
        }

        return Ease.Unset;
    }
}
