using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeroSelect
{
    public class HeroViewAbility : MonoBehaviour
    {
        public Image icon;
        public TextMeshProUGUI desc;
        public Material gray;
        public Material normal;
        public void Refresh(Sprite spr, string msg,bool isOwned)
        {
            icon.sprite = spr;
            desc.text = msg;
            icon.material = isOwned ? normal : gray;
        }
    
    }
}
