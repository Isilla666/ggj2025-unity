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

    public void ShowKill(Human human)
    {
        if (!_isEnabled)
        {
            _isEnabled = true;

            icon.sprite = human.FailIcon;
            title.text = human.PlayerName;
            gameObject.SetActive(true);
            DOTween.Sequence().AppendInterval(3f)
                .AppendCallback(() => { button.sprite = pressed; })
                .AppendInterval(0.5f)
                .AppendCallback(() => { button.sprite = normal; })
                .AppendInterval(0.5f)
                .OnComplete(() =>
                {
                    _isEnabled = false;
                    gameObject.SetActive(false);
                });
        }
    }
}