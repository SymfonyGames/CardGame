using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using Managers;
using Player;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace Misc
{
    public enum BankCurrencyType
    {
        Gold,
        Silver
    }

    public class BankVfxPlayer : MonoBehaviour
    {
        public BankCurrencyType type;
        public int minAmountForTxt = 5;
        public bool useCoinAnim = true;
        public bool useFloatingText = true;
        //      [SerializeField] BankStatsAnimator bankAnimator;
        [SerializeField] GoldPool pool;
        [SerializeField] MagnetVfxData goldMagnet;
        ParticleSystemForceField goldField;
        [SerializeField] SoundData sound;
        public Vector3 floatingTextOffset;

        void Start()
        {
            goldField = Instantiate(goldMagnet.magnetPrefab, goldMagnet.magnetPosition);
            goldMagnet.Magnet = goldField;

            // EventManager.Instance.OnGoldVFX += PlayVfx;
          if(type == BankCurrencyType.Gold)  EventManager.Instance.OnCreateGoldVFX += PlayVfx;
          if(type == BankCurrencyType.Silver)   EventManager.Instance.OnCreateSilverVFX += PlayVfx;
 
        }

        const float Z_OFFSET = 5;

        void OnDisable()
        {
            //EventManager.Instance.OnGoldVFX -= PlayVfx;
            if (EventManager.Instance)
            {
                EventManager.Instance.OnCreateGoldVFX -= PlayVfx;
                EventManager.Instance.OnCreateSilverVFX -= PlayVfx;
            }
        }

        public DamageNumber floatingText;

        public int coinsVisualLimit = 20;

        void PlayVfx(Vector3 fromPos, int amount, int maxObj = -1)
        {
            if (useFloatingText && floatingText && amount >= minAmountForTxt)
                floatingText.Spawn(fromPos + floatingTextOffset, amount);


            StartCoroutine(Recieve(amount, 0.85f));
            AudioManager.Instance.PlaySound(sound);

            if (!useCoinAnim) return;

            var goldData = goldMagnet;
            var goldVfx = pool.Get();
            fromPos.z = Z_OFFSET;
            goldVfx.transform.position = fromPos;

            var e = goldVfx.Particle.emission;
            var b = new ParticleSystem.Burst(0.0f, amount < coinsVisualLimit ? amount : coinsVisualLimit);
            e.SetBurst(0, b);


            var externalForces = goldVfx.Particle.externalForces;
            externalForces.enabled = true;
            var forceField = goldData.Magnet;
            forceField.gameObject.SetActive(true);
            externalForces.AddInfluence(forceField);


            // case BankCurrencyEnum.GEM:
            //
            //     var gemData = _magnetFields[type];
            //     var gemVfx = Pool(gemData.vfxPrefab).Get();
            //     gemVfx.transform.position = fromPos;
            //
            //     if (gemVfx is BankVFX g)
            //     {
            //         var externalForces = g.Particle.externalForces;
            //         externalForces.enabled = true;
            //         var forceField = gemData.Magnet;
            //         forceField.gameObject.SetActive(true);
            //         externalForces.AddInfluence(forceField);
            //     }
        }

        IEnumerator Recieve(int amount, float delay)
        {
            yield return new WaitForSeconds(delay);
            switch (type)
            {
                case BankCurrencyType.Gold:
                    PlayerStash_resources.Instance.AddGold(amount);
                    break;
                case BankCurrencyType.Silver:
                    PlayerStash_resources.Instance.AddSilver(amount);
                    break;
            }
        }
        // bankAnimator.IncrementAnimation(type, amount);
    }
}