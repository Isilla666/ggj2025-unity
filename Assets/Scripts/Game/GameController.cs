using System;
using System.Collections.Generic;
using InGameBehaviours;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private BackendUserManager backendUserManager;
    [SerializeField] private Pool pool;
    [SerializeField] private List<Human> humans;
    
    private Stack<Human> _freeHumans;
    private List<HumanData> _humanDatas;
    
    public event Action<string, string> OnAddHuman;
    
    private void Awake()
    {
        _freeHumans = new Stack<Human>();
        _humanDatas = new List<HumanData>();
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
                    humanData.Human.FadeHuman();
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
        foreach (var humanData in _humanDatas)
        {
            if (humanData.Guid == humanGuid)
            {
                humanData.Human.FadeHuman();
            }
        }
    }
    
    public void StartGame()
    {
        pool.EnableBubbles();
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
