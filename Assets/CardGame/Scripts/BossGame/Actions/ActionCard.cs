using System;
using Managers;
using UnityEngine;

namespace BossGame.Actions
{
    public abstract class ActionCard : MonoBehaviour
    {
   
        [SerializeField] ActionCardUI ui;
        public Vector3 Position => transform.position;
        public ActionData Data { get; private set; }
        public ActionCell Cell { get; private set; }
        protected GamePlayer Owner;
        protected GamePlayer Enemy;
        public event Action<ActionCard> OnUse = delegate { };
        void OnEnable() => ui.OnClick += Click;
        void OnDisable() => ui.OnClick -= Click;

        public void Disable()
        {
            if (Data.ChargeBonus > 0) 
                EventManager.Instance.ChargePlayer(Owner, Data.ChargeBonus);
            OnUse(this);
            Destroy(gameObject);
        }

        void Click()
            => EventManager.Instance.ActionTouch(this);

        public void SetCell(ActionCell cell) => Cell = cell;
        public void SetData(ActionData data) => Data = data;
        public void SetArt(Sprite icon) => ui.Set(icon);

        public void SetPlayers(GamePlayer owner, GamePlayer enemy)
        {
            Enemy = enemy;
            Owner = owner;
        }

        public virtual void Use()
        {
            Debug.LogError("USED");
        }
    }
}