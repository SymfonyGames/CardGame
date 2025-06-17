using DG.Tweening;
using Managers;
using Misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public class PlayerMove : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] CardHero card;
        [SerializeField] Canvas _canvas;
        [SerializeField] float moveScale = 1.2f;
        [Header("DEBUG")]
        [SerializeField] bool busy;

        const float ATTACK_DISTANCE = 3.5f;
        const float DROP_DISTANCE = 2.3f;
        public bool IsBusy => busy;
        Vector3 _position;
        Cell _lastCell;
        bool _isDrag;

        void Start()
        {
            EventManager.Instance.OnEnablePlayer += Free;
            EventManager.Instance.OnDisablePlayer += Busy;
        }

        void OnDisable()
        {
            if (!EventManager.Instance) return;
            EventManager.Instance.OnDisablePlayer -= Busy;
            EventManager.Instance.OnEnablePlayer -= Free;
        }

        void Free() => busy = false;
        void Busy() => busy = true;

        public void Init(CardDataHero hero)
        {
            _hero = hero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (busy) return;
            _isDrag = true;

            // return;
            //   var delta = eventData.pointerCurrentRaycast.worldPosition - eventData.pointerPressRaycast.worldPosition;
            var delta = eventData.pointerCurrentRaycast.worldPosition - _position;
            var desire = _position + delta;
            desire = new Vector3(desire.x, desire.y, 10);

            var cur = transform.position;
            var p = Vector3.Lerp(cur, desire, smoothSpeed);
            transform.position = p;
        }

        public float smoothSpeed = 0.05f;
        float pX;
        float pY;

        void Update()
        {
            if (!_isDrag) return;

            //   var pos = Input.mousePosition;
            // //  var cp = Camera.main.ScreenToWorldPoint(pos);
            // var dx = pos.x -pX;
            // var r = (RectTransform)_canvas.transform;
            //
            // var l = r.rect.x / r.rect.y;
            // var b = 1080f / 1920;
            // var m = l / b;

            //  dx *= m;
            // Debug.LogError(l+", "+b+". DX: "+dx);
            //  Debug.LogError(dx);
            // transform.localPosition = new Vector2(dx+offsetX, 0);
            // transform.position = _position+ pos;

            var c = ClosestCell();

            if (c)
            {
                if (c.Card)
                    c.Card.Glow();
                if (_lastCell && _lastCell != c && _lastCell.Card)
                    _lastCell.Card.DisableGlow();
                _lastCell = c;
            }
            else
            {
                if (_lastCell && _lastCell.Card)
                    _lastCell.Card.DisableGlow();
            }
        }

        float offsetX;
        Vector2 shit;
        CardDataHero _hero;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (busy) return;

            _canvas.overrideSorting = true;
            _canvas.sortingOrder = 40;
            _position = transform.position;
            transform.DOScale(moveScale, 0.2f);
            //
            // var s = eventData.pressPosition;
            // var delta = eventData.pointerCurrentRaycast.worldPosition;
            //
            //
            // shit = Camera.main.WorldToScreenPoint(_position);
            // pX = Camera.main.WorldToScreenPoint(_position).x;
            // pX = Camera.main.WorldToScreenPoint(_position).y;
            //
            // offsetX = s.x - pX;
            // Debug.LogWarning(s+", "+pX);

            // var delta = eventData.pointerCurrentRaycast.worldPosition - _position;
            //  transform.position = _position + delta;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (busy) return;
            _isDrag = false;

            transform.DOScale(1, 0.2f);
            var closest = ClosestCell();
            if (!closest)
            {
                card.MoveBack();
                return;
            }

            card.Cell.InteractSystem.Interact(card.Cell, closest, true);
        }

        public bool InRange(Card target)
        {
            var distance = Vector2.Distance(this.card.Position, target.Position);
            return distance < ATTACK_DISTANCE * card.walkDistance;
        }


        Cell ClosestCell()
        {
            Cell cell = null;
            float min = 9999;

            foreach (var targetCell in card.Cell.AttackCells)
            {
                if (!targetCell.Card) continue;

                var distance = Vector2.Distance(card.Position, targetCell.Position);
                if (distance < DROP_DISTANCE * card.walkDistance && distance <= min)
                {
                    min = distance;
                    cell = targetCell;
                }
            }

            return cell;
        }
    }
}