using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(GeneratorList))]
[RequireComponent(typeof(GeneratorSetup))]
public class GeneratorJob : MonoBehaviour
{
    //[Header("Sound")] 
    //SoundData createSound;
    //[Header("Generate in:")] 
    //[SerializeField] Cell[] rootCells;

    //public Cell[] RootCells => rootCells;
    //GeneratorList generatorsFinder;
    //GeneratorSetup generatorSetup;
    //public GeneratorData Data => generatorSetup.Data;
    //void Awake()
    //{
    //    generatorSetup = GetComponent<GeneratorSetup>();
    //    generatorsFinder = GetComponent<GeneratorList>();
    //    generatorsFinder.InitGenerators(generatorSetup.Data);
    //}
    //void Start()
    //{
    //    EventManager.Instance.OnCreateArtefactAtCell += CreateArtefact;
    //    EventManager.Instance.OnPortalCreateArtefactAtCell += PortalCreateArtefact;
    //}


    //public void CreateHero()
    //{
    //    var cell = rootCells[rootCells.Length / 2];
    //    var heroCard = generatorsFinder.HeroCreator.CreateHero( generatorSetup.Theme);

    //    CardParentRoot(cell, heroCard);
    //    AnimationRoot(heroCard);
    //    LinkCardToCell(cell, heroCard);

    //    PlaySound();
    //}

    //public void CreateRandomCardsAtRootCells()
    //{
    //    foreach (var cell in rootCells)
    //    {
    //        InRootCell(cell);
    //    }
    //}

    //public void CreateRandomCardAtCell(Cell cell)
    //{
    //    var card = TryCreateRandomCard(cell);
    //    if (!card) return;

    //    CardParentBasic(cell, card);
    //    AnimationBasic(card);
    //    LinkCardToCell(cell, card);
    //}

    //void CreateArtefact(CardDataArtifact artefact, Cell cell)
    //{
    //    var artefactPrefab = generatorSetup.Data.ArtefactPrefab;
    //    var card = Instantiate(artefactPrefab);
        
    //    card.Init(artefact, generatorSetup.Theme.Data.Theme);
       
    //    CardParentBasic(cell, card);
    //    AnimationBasic(card);

    //    cell.Card = card;
    //    card.CurrentCell = cell;

    //    PlaySound();
    //}
    //void PortalCreateArtefact( CardDataArtifact artefact, Cell cell, bool flip )
    //{
    //    var artefactPrefab = generatorSetup.Data.ArtefactPrefab;
    //    var card = Instantiate(artefactPrefab);

    //    card.Init(artefact, generatorSetup.Theme.Data.Theme, true);
    //    card.gameObject.layer = LayerMask.NameToLayer("Focus");
        
    //    CardParentBasic(cell, card);
    //    AnimationBasic(card);

    //    cell.Card = card;
    //    card.CurrentCell = cell;
        
    //    if(flip) card.Flip();

    //    PlaySound();
    //}
    //void InRootCell(Cell cell)
    //{
    //    var card = TryCreateRandomCard(cell);
    //    if (!card) return;

    //    CardParentRoot(cell, card);
    //     AnimationRoot(card);
    //    LinkCardToCell(cell, card);

    //    PlaySound();
    //}
    
    
    //Card TryCreateRandomCard(Cell cell)
    //{
    //    var chancesTotal = TotalGeneratorsChance();
    //    var randomChance = Random.Range(0f, chancesTotal);

    //    var chancesSum = 0f;
    //    foreach (var generator in generatorsFinder.Generators)
    //    {
    //        for (int i = 0; i < generator.SpawnChances.Count; i++)
    //        {
    //            chancesSum += generator.SpawnChances[i];
    //            if (randomChance <= chancesSum)
    //            {
    //                var card = generator.CreateCard(generatorSetup.Theme);
    //                TryFlipCard(card, generator, i);
    //                return card;
    //            }
    //        }
    //    }

    //    return null;
    //}


    //void TryFlipCard(Card card, Generator generator, int i)
    //{
    //    if (generator.RotateChances.Count <= i) return;
    //    if (!card) return;
    //    var rotateChance = Random.Range(0f, 1f);
    //    if (rotateChance < generator.RotateChances[i]) card.Flip();
    //}

    //float TotalGeneratorsChance()
    //{
    //    var chancesTotal = 0f;
    //    foreach (var generator in generatorsFinder.Generators)
    //    {
    //        foreach (var chance in generator.SpawnChances)
    //        {
    //            chancesTotal += chance;
    //        }
    //    }

    //    return chancesTotal;
    //}
    //static void LinkCardToCell(Cell cell, Card card)
    //{
    //    cell.Card = card;
    //    card.CurrentCell = cell;
    //}
    //static void CardParentRoot(Cell cell, Card card)
    //{
    //    card.transform.SetParent(cell.transform, false);
     
    //}
    //static void CardParentBasic(Cell cell, Card card)
    //{
    //    card.transform.SetParent(cell.transform, false);
    //}
    //static void AnimationBasic(Card card)
    //{
    //    card.transform.DOScale(Vector3.one, 0.8f);
    //    card.transform.localScale = Vector3.zero;
    //}
    //static void AnimationRoot(Card card)
    //{
    //    card.transform.DOLocalMove(Vector3.zero, 0.8f);
    //    card.transform.localPosition = new Vector3(0, 420, 0);
    //}
    //void PlaySound() => AudioManager.Instance.PlaySound(createSound);
    
}