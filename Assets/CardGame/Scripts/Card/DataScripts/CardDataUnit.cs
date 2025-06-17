using Plugins.AudioManager.audio_Manager;
using Plugins.ColorAttribute;
using UnityEngine;
using UnityEngine.Serialization;

public class CardDataUnit : CardData
{
    [FormerlySerializedAs("healthMin")]
    [FormerlySerializedAs("health")]
    [ContentColor(0.5f, 1f, 0.5f, 1f)]
    [Space(30)]
    [SerializeField] int dmgMin;
    [FormerlySerializedAs("healthMax")] [SerializeField] int dmgMax;
    [SerializeField] int shield;
    [FormerlySerializedAs("coinReward")] 
    [SerializeField] int goldDrop;
    [SerializeField] int silverDrop;
    [SerializeField] float expDrop=25;
    [Space(10)]
    [SerializeField] SoundData attackSound;
    [SerializeField] SoundData deathSound;
    [SerializeField] ParticleSystem attackVFX;
    [SerializeField] ParticleSystem attackHitVFX;
    public int DmgMin => dmgMin;
    public int SilverDrop => silverDrop;
    public int Shield => shield;
    public int GoldDrop => goldDrop;
    public float ExpDrop => expDrop;
    public SoundData AttackSound => attackSound;
    public SoundData DeathSound => deathSound;
    public ParticleSystem AttackVFX => attackVFX;

    public int DmgMax => dmgMax;

    public ParticleSystem AttackHitVFX => attackHitVFX;
}