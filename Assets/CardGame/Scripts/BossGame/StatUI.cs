using TMPro;
using UnityEngine;

namespace BossGame
{
    public class StatUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI txt;

        public void Refresh(int min,int max)
        {
            txt.text = min + "-" + max;
        }
        public void Refresh(int amount)
        {
            txt.text = amount.ToString()+"%";
        }
    }
}
