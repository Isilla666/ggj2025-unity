using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image button;
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite pressed;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text message;
    
    private bool _isEnabled;
    
    public void ShowWin(Human human)
    {
        if(!_isEnabled)
        {
            _isEnabled = true;
            
            icon.sprite = human.WinIcon;
            title.text = human.PlayerName;
            gameObject.SetActive(true);
            _isEnabled = false;
        }
    }
}