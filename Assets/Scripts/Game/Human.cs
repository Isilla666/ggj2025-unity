using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private string humanName;
    [SerializeField] private float fadeDuration;
    [SerializeField] private List<HumanAnimationModel> humanAnimationModels;
    [SerializeField] private List<Fader> faders;
    
    private List<BaseAnimation> _currentAnimations;

    public string HumanName => humanName;

    private void Awake()
    {
        _currentAnimations = new List<BaseAnimation>();
    }

    private void Start()
    {
        ChangeAnimation(HumanAnimation.Idle);
    }

    [Button]
    public void FadeHuman()
    {
        faders.ForEach(x=>x.Fade(fadeDuration));
    }

    [Button]
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

        if (humanAnimation == HumanAnimation.PoopMoment)
        {
            Invoke(nameof(FadeHuman), 5f);
        }
    }
}