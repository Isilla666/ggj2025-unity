using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float randomDelay;
    [SerializeField] private bool isRandom;
    [SerializeField] private bool withRandomDelay;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;
    
    
    private int _currentSpriteIndex = 0;
    
    void Start()
    {
        if (isRandom)
        {
            _currentSpriteIndex = Random.Range(0, sprites.Count);
        }
        StartCoroutine(ChangeSprite());
    }

    private IEnumerator ChangeSprite()
    {
        while (true)
        {
            _currentSpriteIndex++;
            spriteRenderer.sprite = sprites[_currentSpriteIndex % sprites.Count];
            var addedDelay = 0f;
            if (withRandomDelay)
            {
                addedDelay = Random.Range(0f, randomDelay);
            }
            yield return new WaitForSeconds(delay + addedDelay);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
