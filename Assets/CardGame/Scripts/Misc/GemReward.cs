using System.Collections;
using DG.Tweening;
using Managers;
using Player;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace Misc
{
    [RequireComponent(typeof(GemPool))]
    public class GemReward : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float moveTime = 1;
        [SerializeField] float createDelay = 0.5f;
        [SerializeField] SoundData sound;
        [SerializeField] Vector2 offset;
        [Header("Setup")]
        [SerializeField] Transform moveCoinsTo;
        GemPool pool;


        void Start()
        {
            pool = GetComponent<GemPool>();
            EventManager.Instance.OnGemCreate += Create;
            EventManager.Instance.OnGemReceived += GetGemReceived;
        }

        void Create(Vector3 fromPosition, int amount, int maxObjects = -1)
        {
            if (maxObjects == -1) maxObjects = amount;
            int weight = amount / maxObjects;
            for (int i = 0; i < maxObjects; i++)
            {
                StartCoroutine(Animation(fromPosition, weight));
            }

        }

        IEnumerator Animation(Vector3 from, int amount)
        {
            float mult = 0.2f;
            float randomDelay = Random.Range(createDelay * (1 - mult), createDelay * (1 + mult));
            yield return new WaitForSeconds(randomDelay);

            var coin = pool.GetCoin();
            PlaySound();

            Vector2 randomOffset = RandomOffset();
            coin.CoinTransform.localPosition = (Vector2)from + randomOffset;
            coin.CoinTransform.localScale = Vector3.zero;

            var appearTime = 0.2f;
            var doScale = coin.CoinTransform.DOScale(Vector3.one, appearTime);
            yield return doScale.WaitForCompletion();

            var randomMoveTime = RandomTime();
            var doMove = coin.CoinTransform.DOLocalMove(moveCoinsTo.localPosition, randomMoveTime);
            yield return doMove.WaitForCompletion();

            Event(amount);
            // PlaySound();

            var disappearTime = 0.2f;
            var doScaleZero = coin.CoinTransform.DOScale(Vector3.zero, disappearTime);
            yield return doScaleZero.WaitForCompletion();

            coin.ReturnToPool();
        }


        void Event(int amount) => EventManager.Instance.GemReceived(amount);
        void GetGemReceived(int value) => PlayerStash_resources.Instance.AddGem(value);


        Vector2 RandomOffset()
        {
            float randomX = Random.Range(-offset.x, offset.x);
            float randomY = Random.Range(-offset.y, offset.y);
            return new Vector2(randomX, randomY);
        }

        float RandomTime()
        {
            float multiplier = 0.2f;
            return Random.Range(moveTime * (1 - multiplier), moveTime * (1 + multiplier));
        }

        void PlaySound() => AudioManager.Instance.PlaySound(sound);
    }
}
