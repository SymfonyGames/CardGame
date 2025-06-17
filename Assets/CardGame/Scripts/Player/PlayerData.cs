 
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Game/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [FormerlySerializedAs("choosenHero")] [Header("Choosen:")]
        public CardDataHero selectedHero;
    
    
    
    }
}
