using Managers;
using Misc;
using UnityEngine;

namespace HeroAbilities
{
    public class DefenseStance : HeroAbility
    {
        int blockCount;
        [SerializeField] int maxBlocks;
        [SerializeField] GameObject vfx;
        protected override void UseAbility()
        {
            blockCount = maxBlocks;
            if (_hero.Data == heroes[0])
            {
                _hero.SetImmune(true);
                _hero.OnDamage -= Decrease;
                _hero.OnDamage += Decrease;
                _hero.SetShieldVFX(true);
            }
            else
            {
                var move = MoveSystem.Instance;
                Attack(move.BotLine[0]);
                Attack(move.BotLine[1]);
                Attack(move.BotLine[2]);
            }
        }
        [SerializeField] GameObject vfx2;
        void Decrease(float obj)
        {
            blockCount--;
            if (blockCount > 0) return;
            _hero.SetImmune(false);
            _hero.OnDamage -= Decrease;
            _hero.SetShieldVFX(false);
            _hero.PlayVFX(vfx);
        }
        void Attack(Cell cell)
        {
            var drop = GeneratorSystem.Instance.Data.DefaultDrop;

            if (cell.Card && cell.Card is CardCreature c)
            {
                c.Hit(10);
                var anyDrop = c.Data.GetRandomDrop();
                bool isNull = anyDrop;
                c.DropArtefact(isNull, drop);
                c.PlayVFX(vfx2);
            }
        }
    }
}