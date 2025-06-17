using Managers;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerGemUI : MonoBehaviour
    {
        [Header("Text reference")]
        [SerializeField] TextMeshProUGUI gemText;

        int _amount;

        void Start()
        {
            RefreshText();
            EventManager.Instance.OnResourcesChanged += RefreshText;
        }
        void OnDisable()
        {
            EventManager.Instance.OnResourcesChanged -= RefreshText;
        }
        void RefreshText()
        {
            if (_amount != PlayerStash_resources.Instance.Gems || _amount == 0)
            {
                _amount = PlayerStash_resources.Instance.Gems;
                gemText.text = _amount.ToString();
            }
        }
    }
}