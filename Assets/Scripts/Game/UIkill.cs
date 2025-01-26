using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIkill : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image button;
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite pressed;
    [SerializeField] private TMP_Text title;

    private bool _isEnabled;
    
    public void ShowKill(string userName, Human human)
    {
        if(!_isEnabled)
        {
            _isEnabled = true;
            
            icon.sprite = human.FailIcon;
            title.text = userName;
            gameObject.SetActive(true);
            DOTween.Sequence().AppendInterval(5f)
                .AppendCallback(() =>
                {
                    button.sprite = pressed;
                })
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    button.sprite = normal;
                })
                .AppendInterval(1f)
                .OnComplete(() =>
                {
                    _isEnabled = false;
                });
        }
    }
}
