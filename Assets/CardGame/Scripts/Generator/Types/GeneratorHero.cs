using Level;
using Managers;
using Player;
using UnityEngine;

public class GeneratorHero : MonoBehaviour
{
    [Header("GetHero in:")]
    [SerializeField] PlayerData playerData;
    CardHero _prefab;
    GeneratorData _generatorData;

    public void Init(GeneratorData generatorData)
    {
        _generatorData = generatorData;
        _prefab = generatorData.HeroPrefab;
    }

    public Card CreateHero(LevelTheme theme)
    {
        var hero = Instantiate(_prefab);
        hero.Init(playerData.selectedHero, theme.Data.Theme);
        hero.Set(_generatorData );
        EventManager.Instance.PlayerCreated(hero);
        return hero;
    }
}