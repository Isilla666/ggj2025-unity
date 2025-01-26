using DG.Tweening;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public void Fade(float time)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.DOFade(0f, time);
        }
    }

    public void UnFade()
    {
        if (spriteRenderer != null)
            spriteRenderer.DOFade(1f, 0.2f);
    }
    
    private void OnValidate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
