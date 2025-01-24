using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI
{
    public class PlayerScoreGrid : MonoBehaviour
    {
        private const int MaxPlayers = 8;
        [SerializeField] private LayoutGroup layoutParent;
        [SerializeField] private PlayerScoreElement elementPrefab;

        private List<PlayerScoreElement> _elements;
        private Transform _parent;

        private void Start()
        {
            _parent = layoutParent.transform;
            _elements = new List<PlayerScoreElement>();

            foreach (Transform child in _parent)
                Object.Destroy(child.gameObject);
        }

        public void UpdateGrid(IEnumerable<PlayerWithScore> scores)
        {
            ClearGrid();
            
            var orderScore = scores.OrderByDescending(x => x.Score).Take(MaxPlayers);

            foreach (var withScore in orderScore)
            {
                var element = CreateGridElement(withScore);
                _elements.Add(element);
            }

            if (_elements.Count > 1)
                _elements.First().SetAsHighScore();
        }


        private PlayerScoreElement CreateGridElement(PlayerWithScore score)
        {
            var instance = Object.Instantiate(elementPrefab, _parent, false);
            instance.SetScore(score);
            return instance;
        }


        private void ClearGrid()
        {
            foreach (var element in _elements)
                Object.Destroy(element.gameObject);

            _elements.Clear();
        }
    }
}