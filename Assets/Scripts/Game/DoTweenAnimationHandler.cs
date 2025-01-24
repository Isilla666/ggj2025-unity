using DG.Tweening;
using UnityEngine;

public class DoTweenAnimationHandler : BaseAnimation
{
    [SerializeField] private DOTweenAnimation doTweenAnimation;
    
    public override void Animate()
    {
        doTweenAnimation.DOPlay();
    }

    public override void StopAnimation()
    {
        doTweenAnimation.DORewind();
    }
}