using System.Collections.Generic;
using Managers;
using Misc;
using UnityEngine;

namespace HeroAbilities
{
    public class SwordSlash : HeroAbility
    {
        [SerializeField]  List<int> damage;
        [SerializeField] GameObject vfx;
        [SerializeField] GameObject vfx2;

        void Attack(Cell cell)
        {
            var drop = GeneratorSystem.Instance.Data.DefaultDrop;

            if (cell.Card && cell.Card is CardCreature c)
            {
                c.Hit(10);
                return;
                var anyDrop = c.Data.GetRandomDrop();
                bool isNull = anyDrop;
                c.DropArtefact(isNull, drop);
                c.PlayVFX(vfx2);
            }
        }

        protected override void UseAbility()
        {
            var move = MoveSystem.Instance;
            if (!move) return;

            var drop = GeneratorSystem.Instance.Data.DefaultDrop;
            var heal = 0;
            
            if (_hero.Data == heroes[0])
            {
                foreach (var cell in move.Cells)
                {
                    if (!cell.Card) continue;
                    if (cell.Card is CardCreature c)
                    {
                        var id = data.Level - 1;
                        var dmg = id < damage.Count
                            ? damage[id < 0 ? 0 : id]
                            : damage[^1];
                        c.Hit(dmg);
                    }
                    if (cell.Card is CardBoss b)
                    {
                        var id = data.Level - 1;
                        var dmg = id < damage.Count
                            ? damage[id < 0 ? 0 : id]
                            : damage[^1];
                        b.Hit(dmg);
                    }
                    //       heal += c.Health;
                    //
                    // var anyDrop = c.Data.GetRandomDrop();
                    // bool isNull = anyDrop;
                    // c.DropArtefact(isNull, drop);
                    // c.PlayVFX(vfx);
                }

                //       heal += c.Health;
            }
            else
            {
                Attack(move.MidLine[1]);
                Attack(move.TopLine[1]);
                Attack(move.BotLine[1]);
            }


            //  _hero.AddHealth(heal);
        }
    }
}