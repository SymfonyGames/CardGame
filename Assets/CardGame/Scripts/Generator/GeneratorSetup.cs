using Level;
using UnityEngine;

public class GeneratorSetup : MonoBehaviour
{
    [Header("Get prefabs:")] 
    [SerializeField] GeneratorData generatorData;

    [Header("Get cards skins:")]
    [SerializeField] LevelTheme levelTheme;

    public GeneratorData Data => generatorData;
    public LevelTheme Theme => levelTheme;
    
}
