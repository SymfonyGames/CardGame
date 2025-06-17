using UnityEngine;

namespace Misc
{
    public class GameConfig : MonoBehaviour
    {
        [SerializeField] ConfigData config;

        public ConfigData Config => config;

  
    }
}
