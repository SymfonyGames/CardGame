using System;
using UnityEngine;

namespace BossGame
{
    public class Hitpoints : MonoBehaviour
    {

        float _hp;
        float _maxHp;
      public  float Value => _hp / _maxHp;

      public float HP => _hp;

      public event Action OnDeath = delegate { };
        // ReSharper disable once InconsistentNaming
        public void Init(float maxHP )
        {
            _maxHp = maxHP;
            _hp = maxHP;
 
 
        }
        
        public void Damage(float amount)
        {
            if (_hp <= 0) return;

            _hp -= amount;
            
            if (_hp < 0)
            {
                _hp = 0;
                OnDeath();
            }
        }

        public void Heal(float value)
        {
            _hp += value;
            if (_hp > _maxHp)
            {
                _hp = _maxHp;
            }
        }



  
    }
}