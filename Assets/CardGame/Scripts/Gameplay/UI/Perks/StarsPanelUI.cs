using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI.Perks
{
    public class StarsPanelUI : MonoBehaviour
    {
        [SerializeField] List<StarUI> perkStars = new();

        public void Set(int lvl)
        {
            for (int i = 0; i < lvl; i++)
            {
                perkStars[i].Enable();
            }

            for (int i = lvl; i < perkStars.Count; i++)
            {
                perkStars[i].Disable();
            }
        }
    }
}
