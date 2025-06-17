using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Managers;
using UnityEngine;

namespace Level
{
    public class LevelBackgroundLoop : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] bool useLoop;
        [SerializeField] bool test;
        [SerializeField] Vector2 scrollSize;
    
        [Header("Setup")]
        [SerializeField] Transform background_1;
        [SerializeField] Transform background_2;

        Vector2 topPosition,bottomPosition;
        TweenerCore<Vector3, Vector3, VectorOptions> tween_1;
        TweenerCore<Vector3, Vector3, VectorOptions> tween_2;

        void Update()
        {
            if (test)
            {
                test = false;
                CardsMove(1f);
            }
        }

        void Start()
        {
            if (!useLoop)
                background_2.gameObject.SetActive(false);
            else
                EventManager.Instance.OnCardsMoveDown += CardsMove;

            topPosition = background_2.localPosition;
            bottomPosition = -topPosition;
        }

        void CardsMove(float moveTime)
        {
            Coroutine swapPositionRoutine = StartCoroutine(SwapBackgroundPositionToTop());
            tween_1 = background_1.DOLocalMove((Vector2) background_1.localPosition + scrollSize, moveTime).OnComplete(() => StopCoroutine(swapPositionRoutine));
            tween_2 = background_2.DOLocalMove((Vector2) background_2.localPosition + scrollSize, moveTime);
        }
    
        IEnumerator SwapBackgroundPositionToTop()
        {
            while (true)
            {
                if (background_1.localPosition.y <= bottomPosition.y)
                {
                    float differnceY = bottomPosition.y- background_1.localPosition.y;
                    Vector2 correctPosition = new Vector2(topPosition.x, topPosition.y - differnceY);
                    tween_1.startValue = correctPosition;
                }
                if (background_2.localPosition.y <= bottomPosition.y)
                {
                    float differnceY = bottomPosition.y- background_2.localPosition.y;
                    Vector2 correctPosition = new Vector2(topPosition.x, topPosition.y - differnceY);
                    tween_2.startValue = correctPosition;
                }
                yield return null;
            }
        }
    }
}