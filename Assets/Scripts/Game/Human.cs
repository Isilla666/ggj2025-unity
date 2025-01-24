using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private string humanName;
    [SerializeField] private List<HumanAnimationModel> humanAnimationModels;
    
    private List<BaseAnimation> _currentAnimations;

    public string HumanName => humanName;

    private void Awake()
    {
        _currentAnimations = new List<BaseAnimation>();
    }

    public void ChangeAnimation(HumanAnimation humanAnimation)
    {
        _currentAnimations.ForEach(x=>x.StopAnimation());
        _currentAnimations.Clear();
        
        foreach (var humanAnimationModel in humanAnimationModels)
        {
            if (humanAnimationModel.HumanAnimation == humanAnimation)
            {
                foreach (var baseAnimation in humanAnimationModel.Animations)
                {
                    baseAnimation.Animate();
                    _currentAnimations.Add(baseAnimation);
                }
            }
        }
    }
}