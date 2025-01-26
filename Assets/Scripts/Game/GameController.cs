using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening.Core;
using ForTests.Examples;
using InGameBehaviours;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private BackendUserManager backendUserManager;
    [SerializeField] private SonyEricssonPlayer sonyEricssonPlayer;
    [SerializeField] private Pool pool;
    [SerializeField] private List<Human> humans;
    [SerializeField] private UIkill uiKill;
    [SerializeField] private UIWin uiWin;
    [SerializeField] private GameObject codeGameObject;


    private Stack<Human> _freeHumans;
    private Dictionary<string, HumanData> _humans;

    // private List<HumanData> _humanDatas;
    private Coroutine _coWaitForDead;
    private List<string> _killList;
    private List<string> _healthList;
    private List<string> _tryaskaList;

    private bool _musicStarted;
    private int msRange = 1000;
    private int bindRange = 0;
    public event Action<string, string> OnAddHuman;

    private void Awake()
    {
        _freeHumans = new Stack<Human>(16);
        _humans = new Dictionary<string, HumanData>(16);
        _killList = new List<string>(16);
        _healthList = new List<string>(16);
        _tryaskaList = new List<string>(16);
    }

    public void RestartGame() => SceneManager.LoadScene("MainScene");

    private void Start()
    {
        backendUserManager.OnUserConnectedEvent += AddHuman;
        backendUserManager.OnUserDisconnectedEvent += BackendUserManagerOnOnUserDisconnectedEvent;
        backendUserManager.OnUserStopEvent += BackendUserManagerOnOnUserStopEvent;
        backendUserManager.OnUserStartShakeEvent += BackendUserManagerOnOnUserStartShakeEvent;
        backendUserManager.OnUserNoTimeStopEvent += BackendUserManagerOnOnUserNoTimeStopEvent;
        humans.ForEach(x => _freeHumans.Push(x));
    }

    private void Update()
    {
        int bindMs = 0;
        for (int i = 48; i < 58; i++)
        {
            if (Input.GetKeyDown((KeyCode) i))
            {
                bindMs = (i - 48) * 1000;
                break;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
            bindMs = bindMs / 10;

        if (bindMs > 0)
            bindRange = bindMs;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(3, 3, 2000, 20), $"{bindRange}  {msRange}");
    }

    private void BackendUserManagerOnOnUserNoTimeStopEvent(string humanGuid)
    {
        //todo проверка

        // Если мы перестали трясти - добавляемся в health list
        if (_musicStarted)
        {
            _healthList.Add(humanGuid);

            if (_humans.TryGetValue(humanGuid, out var humanData))
            {
                humanData.Human.ChangeAnimation(HumanAnimation.Idle);
            }
        }


        //
        // if (_humans.TryGetValue(humanGuid, out var humanData))
        // {
        //     humanData.Health--;
        //
        //     if (humanData.Health <= 0)
        //     {
        //         _killList.Add(humanGuid);
        //     }
        // }
    }

    private void BackendUserManagerOnOnUserStartShakeEvent(string humanGuid)
    {
        //todo проверка
        if (_musicStarted)
        {
            if (_humans.TryGetValue(humanGuid, out var humanData))
            {
                humanData.Human.ChangeAnimation(HumanAnimation.Dancing);
            }

            // Если начали трясти - убираемся из _healthList;
            _healthList.Remove(humanGuid);
        }
    }

    private void BackendUserManagerOnOnUserStopEvent(string humanGuid, int time)
    {
        if (!_musicStarted)
        {
            _tryaskaList.Remove(humanGuid);

            if (time > 1000)
            {
                _killList.Add(humanGuid);
            }

            if (_humans.TryGetValue(humanGuid, out var humanData))
            {
                humanData.Human.ChangeAnimation(HumanAnimation.Idle);
            }
        }
    }

    private void BackendUserManagerOnOnUserDisconnectedEvent(string humanGuid)
    {
        _killList.Add(humanGuid);
        /*foreach (var humanData in _humanDatas)
        {
            if (humanData.Guid == humanGuid)
            {
                humanData.Human.FadeHuman();

            }
        }*/
    }

    [Button]
    public void StartGame()
    {
        codeGameObject.SetActive(false);
        if (bindRange > 0)
            msRange = bindRange;

        bindRange = 0;
        _musicStarted = true;
        backendUserManager.StateStart();
        pool.EnableBubbles();
        foreach (var humanData in _humans.Values)
            humanData.Human.ChangeAnimation(HumanAnimation.Idle);

        _healthList.AddRange(_humans.Keys);
        _tryaskaList.AddRange(_humans.Keys);

        sonyEricssonPlayer.NextClip(ClipEnd);
    }

    private void ClipEnd()
    {
        if (_coWaitForDead != null)
        {
            StopCoroutine(_coWaitForDead);
        }

        _coWaitForDead = StartCoroutine(WaitForDead());
    }

    private IEnumerator WaitForDead()
    {
        _musicStarted = false;
        backendUserManager.StateStop();
        pool.DisableBubbles();

        // TODO еще один лист, куда запишем всех кто остановился.
        // ПО истечению 3х секунд киляем еще и тех, кто вообще не думал останавливаться.

        if (_healthList.Count > 0)
        {
            foreach (var guid in _healthList)
            {
                if (_humans.TryGetValue(guid, out var humanData))
                {
                    humanData.Health--;
                    if (humanData.Health <= 0)
                        _killList.Add(guid);
                }
            }
        }

        _healthList.Clear();
        yield return new WaitForSeconds(2f);
        var bannedPlayers = _killList.Union(_tryaskaList).ToList();

        if (bannedPlayers.Count > 0)
        {
            foreach (var humanData in _humans.Values)
            {
                if (bannedPlayers.Contains(humanData.Guid))
                {
                    humanData.Human.ChangeAnimation(HumanAnimation.PoopMoment);
                }
                else
                {
                    humanData.Human.ChangeAnimation(HumanAnimation.Haha);
                }
            }

            yield return new WaitForSeconds(4f);


            foreach (var bannedPlayer in bannedPlayers)
            {
                if (_humans.TryGetValue(bannedPlayer, out var banned))
                {
                    uiKill.ShowKill(banned.Human);
                    break;
                }
            }

            yield return new WaitForSeconds(5f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }


        foreach (var killId in bannedPlayers)
        {
            if (_humans.TryGetValue(killId, out var humanData))
            {
                _freeHumans.Push(humanData.Human);
                _humans.Remove(killId);
                backendUserManager.BanUser(killId, "Вы протрясли свою победу!");
            }
        }

        _tryaskaList.Clear();
        _killList.Clear();

        var lastPlayer = _humans.Count <= 1;
        if (lastPlayer)
        {
            var lastHuman = _humans.Values.FirstOrDefault()?.Human;
            if (lastHuman == null)
                lastHuman = humans.First();
            
            uiWin.ShowWin(lastHuman);
        }
        else
        {
            StartGame();
        }        
        
    }

    private void AddHuman(string humanGuid)
    {
        if (_freeHumans.Count > 0)
        {
            var human = _freeHumans.Pop();
            var humanData = new HumanData
            {
                HumanName = human.HumanName,
                Guid = humanGuid,
                Human = human,
                Health = 3,
            };

            human.gameObject.SetActive(true);

            // Это нужно, потому что после того, как пользователь Poop, он Fade в 0
            human.ShowHuman();
            human.ChangeAnimation(HumanAnimation.Idle);

            if (backendUserManager.GetUsersWithNames().TryGetValue(humanGuid, out var playerNickname))
                human.AddPlayerName(playerNickname);

            _humans[humanGuid] = humanData;
            backendUserManager.SendClientCharacter(humanData.Guid, humanData.HumanName);
            OnAddHuman?.Invoke(humanData.HumanName, humanData.Guid);
        }
    }
}