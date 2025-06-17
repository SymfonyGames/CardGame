using Managers;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerSilverUI : MonoBehaviour
    {
        [Header("Text reference")] 
        [SerializeField] TextMeshProUGUI txt;

        int _amount  ;

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
            if (_amount == PlayerStash_resources.Instance.Silver && _amount != 0) return;
            _amount = PlayerStash_resources.Instance.Silver;
            txt.text = _amount.ToString();
        }
    }
}
