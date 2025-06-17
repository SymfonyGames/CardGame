using UnityEngine;

namespace BossGame.Actions
{
    public class AttackAction : ActionCard
    {
        public override void Use()
        {
            var dmg = Owner.Damage;
            Enemy.TakeDamage(dmg);
            Disable();
        }
    }
}