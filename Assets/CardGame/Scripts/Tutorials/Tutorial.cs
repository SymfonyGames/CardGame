using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Tutorials
{
    public class Tutorial : MonoBehaviour
    {
        [Header("Start type")]
        [SerializeField] private TutorialStartType startType;
        [Header("Settings")]
        [SerializeField] private float uiAppearTime;
        ITutorialItem[] _items;
        private int _currentId;
        CardHero _hero;

        enum TutorialStartType
        {
            None,
            FromStart
        }

        void Start()
        {
            EventManager.Instance.OnPlayerCreated += MoveHero;
            Invoke(nameof(Delay1), 0.1f);
            Invoke(nameof(Delay2), 0.2f);
        }

        void MoveHero(CardHero hero)
        {
            _hero = hero;
            Invoke(nameof(move),3);
        }

        void move()
        {
            _hero.transform.DOLocalMove(Vector3.zero, 1f);
        }
        void Delay1()
        {
            _items = GetComponentsInChildren<ITutorialItem>();
        }

        void Delay2()
        {
            if (_items.Length == 0)
            {
                Debug.LogWarning("There's no tutorial items");
                return;
            }

            var config = new TutorialConfig
            {
                uiAppearTime = uiAppearTime
            };
            foreach (var item in _items)
            {
                item.Init(config);
            }

            if (startType == TutorialStartType.FromStart)
                StartTutorial();
        }

        public void StartTutorial()
        {
            foreach (var item in _items)
                item.OnComplete += OnItemComplete;

            StartCoroutine(StartItem(_currentId));
        }

        void OnItemComplete()
        {
            Debug.LogWarning("Item complete");
            if (_currentId < _items.Length - 1)
            {
                Debug.LogWarning("Start next item");
                _currentId++;
                StartCoroutine(StartItem(_currentId));
            }
            else
                TutorialComplete();
        }

        IEnumerator StartItem(int id)
        {
            Debug.LogWarning("ItemStarted [" + id + "], delay:" + _items[id].Delay);
            yield return new WaitForSeconds(_items[id].Delay);
            _items[id].StartItem();
        }


        void TutorialComplete()
        {
            foreach (var item in _items)
                item.OnComplete -= OnItemComplete;
        }
    }
}