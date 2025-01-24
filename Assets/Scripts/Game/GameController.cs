using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Human> humans;
    
    private Stack<Human> _freeHumans;
    private List<HumanData> _humanDatas;

    public event Action<string, Guid> OnAddHuman;
    
    private void Awake()
    {
        _freeHumans = new Stack<Human>();
        _humanDatas = new List<HumanData>();
    }
    
    private void Start()
    {
        humans.ForEach(x => _freeHumans.Push(x));
    }
    
    private void AddHuman(Guid humanGuid)
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
            
            OnAddHuman?.Invoke(humanData.HumanName, humanData.Guid);
        }
    }
}
