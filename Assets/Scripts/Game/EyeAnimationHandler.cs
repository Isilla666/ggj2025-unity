using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EyeAnimationHandler : BaseAnimation
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 openEyeTime;
    [SerializeField] private Vector2 closeEyeTime;
    [SerializeField] private Sprite openEye;
    [SerializeField] private Sprite closeEye;
    
    private Coroutine _coroutine;

    private void Start()
    {
        Animate();
    }

    public override void Animate()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(ChangeSprite());
    }

    public override void StopAnimation()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        spriteRenderer.sprite = openEye;
    }

    private IEnumerator ChangeSprite()
    {
        spriteRenderer.sprite = openEye;
        yield return new WaitForSeconds(Random.Range(0f, 4f));
        spriteRenderer.sprite = closeEye;
        yield return new WaitForSeconds(Random.Range(closeEyeTime.x, closeEyeTime.y));
        while (true)
        {
            spriteRenderer.sprite = openEye;
            yield return new WaitForSeconds(Random.Range(openEyeTime.x, openEyeTime.y));
            spriteRenderer.sprite = closeEye;
            yield return new WaitForSeconds(Random.Range(closeEyeTime.x, closeEyeTime.y));
        }
        // ReSharper disable once IteratorNeverReturns
    }
}