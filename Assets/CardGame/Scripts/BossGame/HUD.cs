using UnityEngine;
using UnityEngine.UI;

namespace BossGame
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] StatUI dmgStat;
        [SerializeField] StatUI armorStat;
        [SerializeField] HitpointUI hp;
        [SerializeField] Image art;
        [SerializeField] Image charge;

        
        public StatUI DmgStat => dmgStat;

        public StatUI ArmorStat => armorStat;

        public HitpointUI Hitpoints => hp;

        public Image Art => art;

        public Image Charge => charge;
    }
}
