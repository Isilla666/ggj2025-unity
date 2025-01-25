using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenAnimationHandler : BaseAnimation
{
    [SerializeField] private List<DOTweenAnimation> targets;
    
    public override void Animate()
    {
        targets.ForEach(x=>x.DOPlay());
    }

    public override void StopAnimation()
    {
        targets.ForEach(x=>x.DORewind());
    }
}