using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Level;
using Managers;
using Misc;
using Plugins.AudioManager.audio_Manager;
using Plugins.LevelLoader;
using UnityEngine;

[RequireComponent(typeof(GeneratorList))]
[RequireComponent(typeof(GeneratorSetup))]
public class GeneratorSystem : MonoBehaviour
{
    #region Singleton

    //-------------------------------------------------------------
    public static GeneratorSystem Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else gameObject.SetActive(false);
        DoAwake();
    }


    //-------------------------------------------------------------

    #endregion

    SoundData createSound;

    List<Cell> rootCells = new();
    int _generatedLines;
    public bool manualSetLines;
    [SerializeField] int totalEnemyLines;

    GeneratorList _list;
    GeneratorSetup _generatorSetup;

    public GeneratorData Data => _generatorSetup.Data;
    public bool IsGeneratePossible => _generatedLines < totalEnemyLines;

    void DoAwake()
    {
        _generatorSetup = GetComponent<GeneratorSetup>();
        _list = GetComponent<GeneratorList>();
        _list.InitGenerators(_generatorSetup.Data, config);
    }

    void Start()
    {
        if (!manualSetLines)
            totalEnemyLines = Loader.Instance.GetTotalEnemyLines();
        else
            Debug.LogError("!!!!!!!!!!!!!!!!!!!! LINES SETED MANUALY !!!!!!!!!!!!!!");

        LevelProgress.Instance.SetTotalLines(totalEnemyLines);
        AddMiniBossIcons();

        rootCells = new List<Cell>();
        foreach (var item in MoveSystem.Instance.GetRootCells())
        {
            rootCells.Add(item);
        }

        EventManager.Instance.OnCreateArtefactAtCell += CreateArtefact;
        EventManager.Instance.OnCreateAtCell += CreateAtCell;
        EventManager.Instance.OnPortalCreateArtefactAtCell += PortalCreateArtefact;
    }

    void OnDisable()
    {
        EventManager.Instance.OnCreateArtefactAtCell -= CreateArtefact;
        EventManager.Instance.OnCreateAtCell -= CreateAtCell;
        EventManager.Instance.OnPortalCreateArtefactAtCell -= PortalCreateArtefact;
    }

    public void SpawnHero()
    {
        var cell = rootCells[rootCells.Count / 2];
        var heroCard = _list.HeroCreator.CreateHero(_generatorSetup.Theme);

        SetParent(cell, heroCard);
        AnimationMove(heroCard);
        LinkCardToCell(cell, heroCard);

        PlaySound();
    }

    void AddMiniBossIcons()
    {
        if (!_list.IsMiniBoss || !_generatorSetup.Data.MiniBossIcon) return;

        foreach (var spawnAt in _list.MiniBossGenerator.SpawnsAtProgress)
        {
            var miniBossLine = (int) (spawnAt * totalEnemyLines);
            var at = (miniBossLine - 1) / (float) totalEnemyLines;
            LevelProgress.Instance.ProgressUI.AddIcon(_generatorSetup.Data.MiniBossIcon, at);
        }
    }

    public void SpawnRandomCards()
    {
        _generatedLines++;
        var isBoss = _generatedLines == totalEnemyLines && _list.IsFinalBoss;
        var isMiniBoss = IsMiniBoss();

        if (isBoss)
        {
            CreateBoss(rootCells[1]);
            return;
        }

        if (isMiniBoss)
        {
            CreateMiniBoss(rootCells[1]);
            return;
        }


        if (Random.Range(0, 1f) < _list.PortalChance)
        {
            var card = _list.Portals.Spawn(_generatorSetup.Theme);
            var cell = rootCells[1];
            SetCell(card, cell);
            AnimationMove(card);


            var c = CreateArtefact(Data.RoadCard);
            c.ThisIsRoad();
            c.ArtGray();
            var cell0 = rootCells[0];
            SetCell(c, cell0);
            AnimationMove(c);

            var c2 = CreateArtefact(Data.RoadCard);
            c.ThisIsRoad();
            c.ArtGray();
            var cell2 = rootCells[2];
            SetCell(c2, cell2);
            AnimationMove(c2);

            return;
        }

        var emptyId = Random.Range(0, 1f) < _list.EmptyCellChance
            ? Random.Range(0, 3)
            : -1;

        for (var i = 0; i < rootCells.Count; i++)
        {
            if (emptyId == i) continue;

            var cell = rootCells[i];
            CreateRandom(cell);
        }
    }

    bool IsMiniBoss()
    {
        if (!_list.MiniBossGenerator)
            return false;

        var isMiniBoss = false;
        foreach (var spawnBossAt in _list.MiniBossGenerator.SpawnsAtProgress)
        {
            var miniBossLine = (int) (spawnBossAt * totalEnemyLines);
            if (_generatedLines == miniBossLine)
                isMiniBoss = true;
        }

        return isMiniBoss;
    }

    public ConfigData config;

    public void SpawnCreatures(List<CardDataCreature> creatures)
    {
        _generatedLines++;
        var prefab = Data.CreaturePrefab;
        for (var i = 0; i < rootCells.Count; i++)
        {
            var cell = rootCells[i];
            if (!creatures[i]) continue;
            var card = Instantiate(prefab);
            card.Init(creatures[i], _generatorSetup.Theme.Data.Theme, config);
            SetCell(card, cell);
            AnimationMove(card);
        }
    }

    public void SpawnArtifacts(List<CardDataArtifact> artifacts)
    {
        _generatedLines++;
        var prefab = Data.ArtefactPrefab;
        for (var i = 0; i < rootCells.Count; i++)
        {
            var cell = rootCells[i];
            if (!artifacts[i]) continue;
            var card = Instantiate(prefab);
            card.Init(artifacts[i], _generatorSetup.Theme.Data.Theme);
            SetCell(card, cell);
            AnimationMove(card);
        }
    }

    public void SpawnMiniBoss(CardDataBoss miniBoss)
    {
        _generatedLines++;
        var prefab = Data.BossPrefab;
        var card = Instantiate(prefab);
        card.Init(miniBoss, _generatorSetup.Theme.Data.Theme);
        SetCell(card, rootCells[1]);
        AnimationMove(card);
    }

    public void SpawnRandomAt(Cell cell)
    {
        var card = GetRandom();
        if (!card) return;

        SetParent(cell, card);
        AnimationScale(card);
        LinkCardToCell(cell, card);
    }

    private void CreateAtCell(ScriptableObject card, Cell cell)
    {
        switch (card)
        {
            case CardDataArtifact artifact:
            {
                var c = CreateArtefact(artifact);
                if (card is CardDataArtifact b && b == Data.RoadCard)
                {
                    c.ThisIsRoad();
                }
                SetCell(c, cell);
                return;
            }
            case CardDataCreature creature:
                CreateCreature(creature, cell);
                return;
        }
    }

    void CreateArtefact(CardDataArtifact artefact, Cell cell)
    {
        var card = CreateArtefact(artefact);
        SetCell(card, cell);
    }

    Card CreateArtefact(CardDataArtifact artefact)
    {
        var artefactPrefab = _generatorSetup.Data.ArtefactPrefab;
        var card = Instantiate(artefactPrefab);

        card.Init(artefact, _generatorSetup.Theme.Data.Theme);
        card.Set(Data);
        card.ArtNormal();
      //  if (artefact.Potions.Count == 0) card.ArtNormal();
       // else card.forceEmptyMaterial = true;


        AnimationScale(card);
        return card;
    }


    void CreateCreature(CardDataCreature creature, Cell cell)
    {
        var creaturePrefab = _generatorSetup.Data.CreaturePrefab;
        var card = Instantiate(creaturePrefab);

        card.Init(creature, _generatorSetup.Theme.Data.Theme, config);
        card.Set(Data);
        card.ArtNormal();

        SetCell(card, cell);
        AnimationScale(card);
    }

    void PortalCreateArtefact(CardDataArtifact artefact, Cell cell, bool flip)
    {
        var artefactPrefab = _generatorSetup.Data.ArtefactPrefab;
        var card = Instantiate(artefactPrefab);

        card.Init(artefact, _generatorSetup.Theme.Data.Theme, true);
        card.gameObject.layer = LayerMask.NameToLayer("Focus");
        card.Set(Data);
        card.ArtNormal();
        //if (artefact.Potions.Count == 0) card.ArtNormal();
        //else card.forceEmptyMaterial = true;

        SetParent(cell, card);
        AnimationScale(card);

        cell.Card = card;
        card.Cell = cell;

        if (flip) card.Flip();

        PlaySound();
    }

    void CreateRandom(Cell cell)
    {
        var card = GetRandom();
        if (!card)
        {
            Debug.LogError("WHY NO CARD HERE");
            return;
        }

        SetCell(card, cell);
        AnimationMove(card);
    }

    void SetCell(Card card, Cell cell)
    {
        SetParent(cell, card);
        LinkCardToCell(cell, card);
        PlaySound();
    }

    void CreateBoss(Cell cell)
    {
        var card = CreateBoss();
        Debug.Log("BOSS: " + card, card);
        if (!card) return;

        SetParent(cell, card);
        AnimationMove(card);
        LinkCardToCell(cell, card);

        PlaySound();
    }

    void CreateMiniBoss(Cell cell)
    {
        var card = CreateMiniBoss();
        Debug.Log("mini boss: " + card, card);
        if (!card) return;

        SetParent(cell, card);
        AnimationMove(card);
        LinkCardToCell(cell, card);

        PlaySound();
    }

    Card GetRandom()
    {
        var r = Random.Range(0, 100);
        var rc = _list.GetRoadChance(LevelProgress.Instance.Value) * 100f;
        var road = (int) (rc);

        if (r < road)
        {
            var c = CreateArtefact(Data.RoadCard);
            c.ThisIsRoad();
            c.ArtGray();
            return c;
        }

        var cardChance = Random.Range(0f, 1);
        var chancesSum = 0f;

        for (var i = 0; i < _list.curveChances.Count; i++)
        {
            chancesSum += i < _list.chances.Count ? _list.chances[i] : _list.curveChances[i];

            if (cardChance <= chancesSum)
            {
                var generator = i < _list.items.Count ? _list.items[i] : _list.items[^1];
                var card = generator.Spawn(_generatorSetup.Theme);
                card.ArtGray();
                TryFlip(card);
                return card;
            }
        }

        var defaultGen = _list.items[0];
        return defaultGen.Spawn(_generatorSetup.Theme);
    }

    Card CreateBoss()
    {
        var generator = _list.BossGenerator;
        var chancesTotal = generator.SpawnChances.Sum();
        var randomChance = Random.Range(0f, chancesTotal);
        var sum = 0f;
        foreach (var chance in generator.SpawnChances)
        {
            sum += chance;
            if (randomChance > sum) continue;
            var card = generator.Spawn(_generatorSetup.Theme);
            var size = _generatorSetup.Data.BossSize;
            card.transform.localScale = new Vector3(size, size, size);
            card.ArtGray();
            TryFlip(card);
            return card;
        }

        return null;
    }

    Card CreateMiniBoss()
    {
        var generator = _list.MiniBossGenerator;
        var chancesTotal = generator.SpawnChances.Sum();
        var randomChance = Random.Range(0f, chancesTotal);
        var sum = 0f;
        foreach (var chance in generator.SpawnChances)
        {
            sum += chance;
            if (randomChance > sum) continue;
            var card = generator.Spawn(_generatorSetup.Theme);
            var size = _generatorSetup.Data.MiniBossSize;
            card.transform.localScale = new Vector3(size, size, size);
            card.ArtGray();
            TryFlip(card);
            return card;
        }

        return null;
    }

    void TryFlip(Card card)
    {
        if (!card) return;

        var rotateChance = Random.Range(0f, 1f);
        if (rotateChance < card.RotateChance)
            card.Flip();
    }

    float TotalGeneratorsChance()
        => _list.Generators.Sum(generator => generator.SpawnChances.Sum());

    static void LinkCardToCell(Cell cell, Card card)
    {
        cell.Card = card;
        card.Cell = cell;
    }

    static void SetParent(Cell cell, Card card)
    {
        card.transform.SetParent(cell.transform, false);
    }


    static void AnimationScale(Card card)
    {
        card.transform.DOScale(Vector3.one, 0.8f);
        card.transform.localScale = Vector3.zero;
    }

    static void AnimationMove(Card card)
    {
        card.transform.DOLocalMove(Vector3.zero, 0.8f);
        card.transform.localPosition = new Vector3(0, 420, 0);
    }

    void PlaySound() => AudioManager.Instance.PlaySound(createSound);
}