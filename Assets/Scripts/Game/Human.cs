using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private string humanName;
    [SerializeField] private Sprite failIcon;
    [SerializeField] private Sprite winIcon;
    [SerializeField] private float fadeDuration;
    [SerializeField] private List<HumanAnimationModel> humanAnimationModels;
    [SerializeField] private List<Fader> faders;
    [SerializeField] private TMP_Text playerNameTMP;
    [SerializeField] private AudioClip bubbleClip;
    
    private List<BaseAnimation> _currentAnimations;

    public string HumanName => humanName;
    public string PlayerName { get; private set; }

    public Sprite FailIcon => failIcon;

    public Sprite WinIcon => winIcon;

    private void Awake() => _currentAnimations = new List<BaseAnimation>();

    private void Start() => ChangeAnimation(HumanAnimation.Idle);

    private void OnEnable() => faders.ForEach(x => x.UnFade());

    public void AddPlayerName(string namePlayer)
    {
        PlayerName = namePlayer;
        playerNameTMP.text = namePlayer;
    }

    [Button]
    public void FadeHuman() => faders.ForEach(x => x.Fade(fadeDuration));

    public void ShowHuman() => faders.ForEach(x => x.UnFade());
    
    [Button]
    public void ChangeAnimation(HumanAnimation humanAnimation)
    {
        _currentAnimations.ForEach(x => x.StopAnimation());
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
            AudioSource.PlayClipAtPoint(bubbleClip, transform.position);
            Invoke(nameof(FadeHuman), 5f);
        }
    }
}