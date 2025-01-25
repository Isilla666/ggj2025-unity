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
    
    private void OnValidate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
