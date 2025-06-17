using DG.Tweening;
using UnityEngine;

namespace BossGame
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] HUD hud;
        [SerializeField] CellsUI hands;
        [SerializeField] CellsUI table;

        public CellsUI Hands => hands;
        public CellsUI Table => table;
 
        public HitpointUI Hitpoints => hud.Hitpoints;
        public void RefreshCharge(float value)
        {
            DOVirtual.Float(hud.Charge.fillAmount, value, 0.3f, f =>  hud.Charge.fillAmount= f);
        }

        public void RefreshStats(int midDmg, int maxDmg, int armor)
        {
            hud.DmgStat.Refresh(midDmg, maxDmg);
            hud.ArmorStat.Refresh(armor);
        }
        
        public void SetArt(Sprite sprite) => hud.Art.sprite = sprite;
    }
}