using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Player
{
    [System.Serializable]
    public class HeroesOwnedData
    {
        [FormerlySerializedAs("selectedHero")]
        public HeroData selected;
        public List<HeroData> heroes = new();
        public List<HeroData> team = new();
    }
}