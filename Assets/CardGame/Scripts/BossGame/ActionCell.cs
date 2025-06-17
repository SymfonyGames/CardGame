using System;
using BossGame.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace BossGame
{
    public class ActionCell : MonoBehaviour
    {
        [SerializeField] ActionCard card;
        [SerializeField] Image frame;
        [SerializeField] bool disableFrame = true;

        void Awake()
        {
            if (disableFrame) frame.enabled = false;
        }

        public bool HasCard => card;
        public bool Empty => !card;

        public ActionCard Card => card;

        public void Put(ActionCard actionCard)
        {
            card = actionCard;
            card.transform.SetParent(transform);
        }

        public void Remove(ActionCard actionCard)
        {
            if (!card) return;
            card = null;
        }
    }
}