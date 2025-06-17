using System;
using System.Collections.Generic;
using System.Linq;
using DataSave;
using DG.Tweening;
using HeroSelect;
using Level;
using Plugins.LevelLoader;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Maps
{
    public class MapUI : MonoBehaviour
    {
        [SerializeField] RectTransform mapContainer;
        [SerializeField] RectTransform canvasRect;
        [SerializeField] Canvas canvas;
        [SerializeField] MapHeroChip heroChip;
        [SerializeField] Button playButton;
        [SerializeField] Button closeButton;
        public TextMeshProUGUI playText;
        public TextMeshProUGUI themeText;
        [SerializeField] Button exitToMainMenu;
        [SerializeField] Button heroSelect;
        [SerializeField] HeroSelectSystem heroSelectSystem;


        public event Action<MapUI, MapLocation, MapPointer> OnPointerClick;
        public event Action OnPlayButton;

        float _chipBaseSize;
        MapPointer _currentPointer;

        MapLocation[] _mapLocations;
        MapData _mapData;

        public MapPointer CurrentPointer => _currentPointer;


        void Awake() => _chipBaseSize = heroChip.transform.localScale.x;

        public void Show()
        {
            if (canvas)
                canvas.enabled = true;
        }

        public void Hide()
        {
            if (canvas)
                canvas.enabled = false;
        }

        public void BackToMainMenu()
        {
            Loader.Instance.LoadMainMenu();
        }

        public void Init(MapData mapData)
        {
            _mapData = mapData;
            playButton.onClick.AddListener(() => OnPlayButton?.Invoke());

            if (SceneManager.GetActiveScene().name == "_MainMenu")
            {
                closeButton.onClick.AddListener(Hide);
                exitToMainMenu.gameObject.SetActive(false);
            }
            else
            {
                if (exitToMainMenu)
                    exitToMainMenu.onClick.AddListener(BackToMainMenu);

                closeButton.gameObject.SetActive(false);
            }


            if (heroSelect)
            {
                heroSelect.onClick.AddListener(heroSelectSystem.Enable);
            }

            FindMapLocations();
            
  
        }



        public void SetHeroChip(CardDataHero hero)
        {
            heroChip.SetIcon(hero.Art);
        }

        public void SetCurrentPointer(MapSave mapSave)
        {
            var currentLocation = _mapLocations.FirstOrDefault(l => l.Theme == mapSave.currentTheme);
            if (currentLocation == null) return;

            var currentPointer = currentLocation.Pointers.FirstOrDefault(p => p.Id == mapSave.currentPointId);
            if (currentPointer != null)
            {
                _currentPointer = currentPointer;
            }

            themeText.text = currentLocation.Theme.ToString();
            playText.text = "Level " + _currentPointer.Id;
        }

        public MapPointer GetMapPointer(ThemeData theme, int pointerId)
        {
            var loc = _mapLocations.FirstOrDefault(l => l.Theme == theme);
            if (loc == null) return null;

            var currentPointer = loc.Pointers.FirstOrDefault(p => p.Id == pointerId);
            if (currentPointer) return currentPointer;

            return null;
        }

        public void MoveHeroChipAnimated(MapPointer moveFrom, MapPointer moveTo)
        {
            heroChip.transform.localScale = new Vector3(_chipBaseSize, _chipBaseSize, _chipBaseSize);
            if (moveFrom) heroChip.transform.position = moveFrom.transform.position;

            heroChip.transform
                .DOScale(_chipBaseSize * _mapData.ChipMaxSize, 0.12f)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                    heroChip.transform
                        .DOMove(moveTo.transform.position, _mapData.ChipMoveDuration)
                        .OnComplete(() => heroChip.transform.DOScale(_chipBaseSize, 0.12f)
                            .SetEase(Ease.OutQuad))
                );
        }

        public void MoveHeroChipInstantToCurrent()
        {
            heroChip.transform.position = CurrentPointer.transform.position;
            heroChip.transform.localScale = new Vector3(_chipBaseSize, _chipBaseSize, _chipBaseSize);
        }

        void FindMapLocations()
        {
            _mapLocations = mapContainer.GetComponentsInChildren<MapLocation>();
            foreach (var location in _mapLocations)
            {
                location.Init();
                location.OnPointerClick += PointerClick;
            }
        }


        public void RefreshPointers(IReadOnlyList<MapSequenceDataSave> sequencesData)
        {
            ResetPointers();
            SetPointers(sequencesData);
        }

        public void MoveMapToCurrentPointer()
        {
            if (!CurrentPointer) return;

            var x = CurrentPointer.OffsetByX;
            var rx = canvasRect.rect.width / 2;
            var newPos = new Vector3(x - rx, mapContainer.localPosition.y, 0);
            mapContainer.localPosition = newPos;
        }

        void ResetPointers()
        {
            foreach (var location in _mapLocations)
            {
                foreach (var pointer in location.Pointers)
                {
                    pointer.SetSprite(_mapData.MapLevelIconLocked);
                    pointer.SetLock(true);
                }
            }
        }

        void SetPointers(IReadOnlyList<MapSequenceDataSave> sequencesData)
        {
            foreach (var seqData in sequencesData)
            {
                //search for theme in mapData
                var location = _mapLocations.FirstOrDefault(l => l.Theme == seqData.theme);
                if (location == null) continue;
                Debug.Log("Location finded: " + location.Theme);

                //set sprites and locks
                var unlockedLevelIDs = seqData.unlockedLvlIds;
                var completeLevelIDs = seqData.completeLevelIDs;
                Debug.Log("Unlocked  : " + unlockedLevelIDs.Count);
                Debug.Log("Completed  : " + completeLevelIDs.Count);
                Debug.Log("Location points  : " + location.Pointers.Length);

                for (int k = 1; k <= location.Pointers.Length; k++)
                {
                    var pointer = location.Pointers[k - 1];
                    if (completeLevelIDs.Contains(k))
                    {
                        pointer.SetSprite(_mapData.MapLevelIconComplete);
                    }
                    else
                    {
                        pointer.SetSprite
                        (
                            unlockedLevelIDs.Contains(k)
                                ? _mapData.MapLevelIconDefault
                                : _mapData.MapLevelIconLocked
                        );
                    }

                    pointer.SetLock(!unlockedLevelIDs.Contains(k));
                }

                if (unlockedLevelIDs.Count > 0)
                {
                    var lastUnlocked = unlockedLevelIDs[unlockedLevelIDs.Count - 1];
                    if (lastUnlocked < location.Pointers.Length)
                    {
                        var pointer = location.Pointers[lastUnlocked - 1];
                        pointer.SetSprite(_mapData.MapLevelIconLastUnlock);
                    }
                }
            }
        }

        void PointerClick(MapLocation location, MapPointer pointer)
            => OnPointerClick?.Invoke(this, location, pointer);
    }
}