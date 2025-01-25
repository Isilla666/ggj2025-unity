using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private ParticleSystem bubblesParticleSystem;
    [SerializeField] private List<SpriteRenderer> bubbles;
    
    [Button]
    public void EnableBubbles()
    {
        bubbles.ForEach(x=>x.DOFade(1f, 0.5f).SetEase(Ease.InOutSine));
        bubblesParticleSystem.Play();
    }
    
    [Button]
    public void DisableBubbles()
    {
        bubbles.ForEach(x=>x.DOFade(0f, 0.5f).SetEase(Ease.InOutSine));
        bubblesParticleSystem.Stop();
    }
}
