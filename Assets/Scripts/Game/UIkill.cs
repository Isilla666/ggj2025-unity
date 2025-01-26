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
    [SerializeField] private TMP_Text message;

    private bool _isEnabled;

    private List<string> words = new()
    {
        "Чувак, эта вечеринка полный отстой.",
        "Да я просто в носу ковырял, алё, чо за агр?",
        "Лаги! Дедовский сервак не тянет мою мощь.",
        "Я не проиграл, я просто элегантно ретировался!",
    };

    public void ShowKill(Human human)
    {
        if(!_isEnabled)
        {
            _isEnabled = true;
            
            icon.sprite = human.FailIcon;
            title.text = human.PlayerName;
            message.text = words[Random.Range(0, words.Count)];
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
                    gameObject.SetActive(false);
                });
        }
    }
}