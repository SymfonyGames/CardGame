using System.Collections.Generic;
using Level;
using Misc;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{
    public virtual void Init(GeneratorData generatorData, ConfigData configData ) { }
    public virtual Card Spawn(LevelTheme theme) => null;

    public List<float> SpawnChances;
    public List<float> RotateChances;

}