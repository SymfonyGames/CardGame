using TMPro;
using UnityEngine;

namespace Level
{
    public class RewardItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI valueText;

        public void EnableReward(int count)
        {
            gameObject.SetActive(true);
            valueText.text = count.ToString();
        }
    }
}