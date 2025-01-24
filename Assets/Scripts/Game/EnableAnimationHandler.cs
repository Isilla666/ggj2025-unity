using System.Collections.Generic;
using UnityEngine;

public class EnableAnimationHandler : BaseAnimation
{
    [SerializeField] private List<GameObject> targets;
    
    public override void Animate()
    {
        targets.ForEach(x=>x.SetActive(true));
    }

    public override void StopAnimation()
    {
        targets.ForEach(x=>x.SetActive(false));
    }
}