using System;
using System.Collections;
using System.Collections.Generic;
using ForTests.Examples;
using InGameBehaviours;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private BackendUserManager backendUserManager;
    [SerializeField] private SonyEricssonPlayer sonyEricssonPlayer;
    [SerializeField] private Pool pool;
    [SerializeField] private List<Human> humans;
    
    private Stack<Human> _freeHumans;
    private List<HumanData> _humanDatas;
    private Coroutine _coWaitForDead;
    private List<string> _killList;
    private bool _musicStarted;
    public event Action<string, string> OnAddHuman;
    
    private void Awake()
    {
        _freeHumans = new Stack<Human>();
        _humanDatas = new List<HumanData>();
        _killList = new List<string>();
    }
    
    private void Start()
    {
        backendUserManager.OnUserConnectedEvent += AddHuman;
        backendUserManager.OnUserDisconnectedEvent += BackendUserManagerOnOnUserDisconnectedEvent;
        backendUserManager.OnUserStopEvent += BackendUserManagerOnOnUserStopEvent;
        backendUserManager.OnUserStartShakeEvent += BackendUserManagerOnOnUserStartShakeEvent;
        backendUserManager.OnUserNoTimeStopEvent += BackendUserManagerOnOnUserNoTimeStopEvent;
        humans.ForEach(x => _freeHumans.Push(x));
    }
    
    private void BackendUserManagerOnOnUserNoTimeStopEvent(string humanGuid)
    {
        //todo проверка
        foreach (var humanData in _humanDatas)
        {
            if (humanData.Guid == humanGuid)
            {
                humanData.Health--;
                if (humanData.Health <= 0)
                {
                    _killList.Add(humanData.Guid);
                }
            }
        }
    }
    
    private void BackendUserManagerOnOnUserStartShakeEvent(string humanGuid)
    {
        //todo проверка
        foreach (var humanData in _humanDatas)
        {
            if (humanData.Guid == humanGuid)
            {
                humanData.Human.ChangeAnimation(HumanAnimation.Dancing);
            }
        }
    }
    
    private void BackendUserManagerOnOnUserStopEvent(string humanGuid, int time)
    {
        
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
    
    public void StartGame()
    {
        _musicStarted = true;
        pool.EnableBubbles();
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
        pool.DisableBubbles();
        yield return new WaitForSeconds(2f);
        if (_killList.Count > 0)
        {
            foreach (var killMan in _killList)
            {
                foreach (var humanData in _humanDatas)
                {
                    if (humanData.Guid == killMan)
                    {
                        humanData.Human.ChangeAnimation(HumanAnimation.PoopMoment);
                    }
                    else
                    {
                        if (!_killList.Contains(humanData.Guid))
                        {
                            humanData.Human.ChangeAnimation(HumanAnimation.Haha);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(10f);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        _musicStarted = true;
        pool.EnableBubbles();
        sonyEricssonPlayer.NextClip(ClipEnd);
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
                Human = human
            };
            
            _humanDatas.Add(humanData);
            backendUserManager.SendClientCharacter(humanData.Guid, humanData.HumanName);
            OnAddHuman?.Invoke(humanData.HumanName, humanData.Guid);
        }
    }
}
