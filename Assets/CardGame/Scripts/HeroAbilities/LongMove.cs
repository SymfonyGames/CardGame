using Misc;

namespace HeroAbilities
{
    public class LongMove : HeroAbilityPassive
    {
        protected override void OnInit()
        {
            base.OnInit();
            Hero.walkDistance++;
        }
    }
}