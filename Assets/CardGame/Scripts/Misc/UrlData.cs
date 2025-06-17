using UnityEngine;

namespace Misc
{
    [CreateAssetMenu(fileName = "Url Links data", menuName = "Game/Url Links Data")]
    public class UrlData : ScriptableObject
    {
        [SerializeField] string musicUrl;
        [SerializeField] string artUrl;

        public string MusicUrl => musicUrl;
        public string ArtUrl => artUrl;
    }
}
