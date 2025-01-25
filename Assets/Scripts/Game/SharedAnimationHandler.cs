using System.Collections.Generic;
using UnityEngine;

public class SharedAnimationHandler : BaseAnimation
{
    [SerializeField] private List<BaseAnimation> targets;

    private bool _isStarted;
    public override void Animate()
    {
        if (!_isStarted)
        {
            _isStarted = true;
            targets.ForEach(x => x.Animate());
        }
    }

    public override void StopAnimation()
    {
        if (_isStarted)
        {
            _isStarted = false;
            targets.ForEach(x => x.StopAnimation());
        }
    }
}