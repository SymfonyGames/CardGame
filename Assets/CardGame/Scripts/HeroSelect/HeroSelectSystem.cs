using System.Collections.Generic;
using Maps;
using Player;
using UnityEngine;

namespace HeroSelect
{
    public class HeroSelectSystem : MonoBehaviour
    {
        [SerializeField] Canvas canvas;
        [SerializeField] Map map;
        [Space(20)]
        [SerializeField] List<HeroSelect> heroesCells = new();

        void Start()
        {
            if (Application.isPlaying)
                Disable();

            foreach (var item in heroesCells)
                item.OnHeroSelected += HeroSelected;
        }

        public void HeroSelected(CardDataHero hero)
        {
            PlayerStash_heroes.Instance.Select(hero);
            Debug.Log("Hero selected: " + hero.name);

            Disable();
            map.Show();
            map.SetHeroIcon(hero);
        }


        public void Enable()
        {
            if (canvas) canvas.enabled = true;
        }

        public void Disable()
        {
            if (canvas) canvas.enabled = false;
        }
    }
}