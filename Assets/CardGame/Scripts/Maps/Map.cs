using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataSave;
using Level;
using Managers;
using Player;
using Plugins.AudioManager.audio_Manager;
using Plugins.LevelLoader;
using UnityEngine;

namespace Maps
{
    public class Map : MonoBehaviour
    {
        [SerializeField] UserDataController userData;
        [SerializeField] List<MapDataSettings> maps;
        int _currentMapId;
        bool isLoading;

        void Awake()
        {
            Hide();
        }
 
        public void Show()
        {
            maps[_currentMapId].mapUI.RefreshPointers(userData.MapSave[_currentMapId].sequencesData);
            maps[_currentMapId].mapUI.SetCurrentPointer(userData.MapSave[_currentMapId]);
            maps[_currentMapId].mapUI.MoveMapToCurrentPointer();
            maps[_currentMapId].mapUI.MoveHeroChipInstantToCurrent();
            maps[_currentMapId].mapUI.SetHeroChip(PlayerStash_heroes.Instance.PlayerData.selectedHero);
            maps[_currentMapId].mapUI.Show();
            AudioManager.Instance.PlaySound(maps[_currentMapId].data.OpenSound);
        }

        public void Hide()
        {
            maps[_currentMapId].mapUI.Hide();
            AudioManager.Instance.PlaySound(maps[_currentMapId].data.CloseSound);
        }

        public void ShowAtLevelComplete()
        {
            maps[_currentMapId].mapUI.MoveMapToCurrentPointer();
            maps[_currentMapId].mapUI.MoveHeroChipAnimated(_lastPointer, maps[_currentMapId].mapUI.CurrentPointer);
            maps[_currentMapId].mapUI.SetHeroChip(PlayerStash_heroes.Instance.PlayerData.selectedHero);
            maps[_currentMapId].mapUI.Show();
        }

        public void ShowAfterTutorial()
        {
            var mapSave = userData.MapSave;
            mapSave[_currentMapId].currentTheme = maps[_currentMapId].data.StartingTheme;
            mapSave[_currentMapId].currentPointId = 1;
            userData.SetMapData(mapSave);
            maps[_currentMapId].mapUI.SetCurrentPointer(userData.MapSave[_currentMapId]);

            //maps[_currentMapId].mapUI.MoveHeroChipAnimated(maps[_currentMapId].mapUI.CurrentPointer);
            //maps[_currentMapId].mapUI.SetHeroChip(PlayerStash_heroes.Instance.PlayerData.choosenHero);
            maps[_currentMapId].mapUI.MoveHeroChipInstantToCurrent();
            maps[_currentMapId].mapUI.MoveMapToCurrentPointer();
            maps[_currentMapId].mapUI.Show();
        }

        public void SetHeroIcon(CardDataHero hero)
        {
            if (hero) maps[_currentMapId].mapUI.SetHeroChip(hero);
        }


        IEnumerator Start()
        {
            _currentMapId = 0;

            while (!userData.IsInitialized)
            {
                yield return null;
            }

            FirstMapSaveInit();

            foreach (var map in maps)
            {
                map.mapUI.Init(map.data);
                map.mapUI.OnPointerClick += OnMapPointerClick;
                map.mapUI.OnPlayButton += OnPlayButton;
            }

            EventManager.Instance.OnLevelWin += OnLevelWin;
        
            if (showAtStart) Show();
        }
        public bool showAtStart;
        int _lastPointerId;
        ThemeData _lastTheme;
        MapPointer _lastPointer;

        void OnLevelWin()
        {
            _lastPointerId = userData.MapSave[_currentMapId].currentPointId;
            _lastTheme = userData.MapSave[_currentMapId].currentTheme;
            ;
            _lastPointer = maps[_currentMapId].mapUI.GetMapPointer(_lastTheme, _lastPointerId);

            //Level complete save to data
            var curTheme = userData.MapSave[_currentMapId].currentTheme;
            var curSeq = Loader.Instance.CurrentSequence;
            var curLvlId = Loader.Instance.CurrentLevel;
            if (curSeq == null)
            {
                return;
            }

            userData.MapLevelComplete(_currentMapId, curTheme, curSeq, curLvlId);

            //Unlock next level
            var levelsCount = curSeq.Count;
            var nextId = curLvlId + 1;
            var unlockSeq = curSeq;
            var unlockTheme = curTheme;

            if (nextId > levelsCount)
            {
                var seqId = GetSequenceId(curSeq);
                var totalSeq = maps[_currentMapId].data.Sequences.Count;
                if (seqId + 1 < totalSeq)
                {
                    nextId = 1;
                    seqId++;
                    unlockSeq = maps[_currentMapId].data.Sequences[seqId].sequence;
                    unlockTheme = maps[_currentMapId].data.Sequences[seqId].theme;
                }
                else
                {
                    nextId = curLvlId;
                }
            }

            userData.MapLevelUnlock(_currentMapId, unlockTheme, unlockSeq, nextId);

            //Refresh map pointer
            maps[_currentMapId].mapUI.RefreshPointers(userData.MapSave[_currentMapId].sequencesData);

            //set new - current map theme and pointId
            userData.MapSave[_currentMapId].currentTheme = unlockTheme;
            userData.MapSave[_currentMapId].currentPointId = nextId;
            maps[_currentMapId].mapUI.SetCurrentPointer(userData.MapSave[_currentMapId]);

            userData.SaveMapData();
        }


        void FirstMapSaveInit()
        {
            if (userData.MapSave is List<MapSave> mapSave)
            {
                var saveRequired = false;
                for (var i = 0; i < maps.Count; i++)
                {
                    if (mapSave.Count < i) continue;

                    var newMapSave = new MapSave
                    {
                        currentTheme = maps[i].data.StartingTheme,
                        currentPointId = 1,
                        sequencesData = new List<MapSequenceDataSave>()
                    };

                    foreach (var seq in maps[i].data.Sequences)
                    {
                        var newSeqData = new MapSequenceDataSave
                        {
                            theme = seq.theme,
                            sequence = seq.sequence,
                            unlockedLvlIds = seq.unlockedLvlIds
                        };
                        newMapSave.sequencesData.Add(newSeqData);
                    }

                    mapSave.Add(newMapSave);
                    saveRequired = true;
                }

                if (saveRequired)
                {
                    userData.SetMapData(mapSave);
                }
            }
        }


        public void OnPlayButton()
        {
            if (!Loader.Instance) return;
            if (isLoading) return;
            isLoading = true;


            var curTheme = userData.MapSave[_currentMapId].currentTheme;
            var seq = maps[_currentMapId].data.Sequences.FirstOrDefault(s => s.theme == curTheme);
            if (seq != null)
                Loader.Instance.LoadLevel(seq.sequence, userData.MapSave[_currentMapId].currentPointId - 1);
            else
                Debug.LogWarning("Can't load level, not in list");
        }


        void OnMapPointerClick(MapUI map, MapLocation location, MapPointer pointer)
        {
            if (pointer.IsLock)
            {
                Debug.Log("Map pointer is locked");
            }
            else
            {
                if (map.CurrentPointer == pointer)
                {
                    Debug.Log("Map current pointer clicked");
                }
                else
                {
                    Debug.Log("Map pointer click");
                    var mapSave = userData.MapSave;
                    mapSave[_currentMapId].currentTheme = location.Theme;
                    mapSave[_currentMapId].currentPointId = pointer.Id;
                    userData.SetMapData(mapSave);

                    map.SetCurrentPointer(userData.MapSave[_currentMapId]);
                    // map.MoveMapToCurrentPointer();
                    map.MoveHeroChipAnimated(null, pointer);
                }
            }
        }


        int GetSequenceId(LevelSequenceData sequence)
        {
            var seqId = -1;
            var totalSequence = maps[_currentMapId].data.Sequences.Count;

            for (int i = 0; i < totalSequence; i++)
            {
                var seq = maps[_currentMapId].data.Sequences[i];
                if (seq.sequence == sequence) seqId = i;
            }

            return seqId;
        }
    }
}