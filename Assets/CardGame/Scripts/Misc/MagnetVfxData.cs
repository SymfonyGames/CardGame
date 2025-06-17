using System;
using UnityEngine;

namespace Misc
{
    [Serializable]
    public class MagnetVfxData
    {
        //public BankCurrencyEnum type;
      //  public BankVFX vfxPrefab;
        public ParticleSystemForceField magnetPrefab;
        public RectTransform magnetPosition;
        public ParticleSystemForceField Magnet { get; set; }
    }
}