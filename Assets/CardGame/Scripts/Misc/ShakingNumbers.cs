using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Misc
{
    public class ShakingNumbers : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] TMP_Text txt;
        [SerializeField] bool shakeFromStart;
       
        [Header("Settings")]
        [SerializeField] float angleMultiplier = 0f;
        [SerializeField] float speedMultiplier = 5f;
        [SerializeField] float curveScale = 15f;

   
        
        bool _hasTextChanged;

        struct VertexAnim
        {
            public float angleRange;
            public float angle;
            public float speed;
        }

        void Start()
        {
            if (shakeFromStart)
                StartCoroutine(ShakeRoutine());
        }

        public void ApplyShake()
        {
            StopAllCoroutines();
            StartCoroutine(ShakeRoutine());
        }


        void OnEnable()
        {
            // Subscribe to event fired when text object has been regenerated.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
        }

        void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
            StopAllCoroutines();
        }

        void ON_TEXT_CHANGED(Object obj)
        {
            if (obj == txt)
                _hasTextChanged = true;
        }


        IEnumerator ShakeRoutine()
        {
            txt.ForceMeshUpdate();
            var textInfo = txt.textInfo;

            var loopCount = 0;
            _hasTextChanged = true;

            var vertexAnim = new VertexAnim[1024];
            for (var i = 0; i < 1024; i++)
            {
                vertexAnim[i].angleRange = Random.Range(10f, 25f);
                vertexAnim[i].speed = Random.Range(1f, 3f);
            }

            var cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

            while (true)
            {
                if (_hasTextChanged)
                {
                    cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
                    _hasTextChanged = false;
                }

                var characterCount = textInfo.characterCount;
                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }

                var numbers
                    = (from info in textInfo.characterInfo where IsNumber(info.character) select info.index).ToArray();

                foreach (var n in numbers)
                {
                    var charInfo = textInfo.characterInfo[n];
                    if (!charInfo.isVisible) continue;

                    var anim = vertexAnim[n];
                    var materialIndex = textInfo.characterInfo[n].materialReferenceIndex;
                    var i = textInfo.characterInfo[n].vertexIndex;
                    var vert = cachedMeshInfo[materialIndex].vertices;

                    Vector2 charMidBaseline = (vert[i + 0] + vert[i + 2]) / 2;
                    Vector3 offset = charMidBaseline;

                    var move = textInfo.meshInfo[materialIndex].vertices;
                    move[i + 0] = vert[i + 0] - offset;
                    move[i + 1] = vert[i + 1] - offset;
                    move[i + 2] = vert[i + 2] - offset;
                    move[i + 3] = vert[i + 3] - offset;

                    anim.angle = Mathf.SmoothStep(-anim.angleRange, anim.angleRange,
                        Mathf.PingPong(loopCount / 25f * anim.speed, 1f));
                    var shake = new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f), 0);
                    var matrix = Matrix4x4.TRS(shake * curveScale,
                        Quaternion.Euler(0, 0, Random.Range(-5f, 5f) * angleMultiplier), Vector3.one);

                    move[i + 0] =
                        matrix.MultiplyPoint3x4(move[i + 0]);
                    move[i + 1] =
                        matrix.MultiplyPoint3x4(move[i + 1]);
                    move[i + 2] =
                        matrix.MultiplyPoint3x4(move[i + 2]);
                    move[i + 3] =
                        matrix.MultiplyPoint3x4(move[i + 3]);

                    move[i + 0] += offset;
                    move[i + 1] += offset;
                    move[i + 2] += offset;
                    move[i + 3] += offset;

                    vertexAnim[n] = anim;
                }

                // Push changes into meshes
                for (var i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    txt.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                loopCount += 1;

                yield return new WaitForSeconds(0.1f / speedMultiplier);
            }
        }

        public bool IsNumber(char c) =>
            c.ToString() switch
            {
                "0" => true,
                "1" => true,
                "2" => true,
                "3" => true,
                "4" => true,
                "5" => true,
                "6" => true,
                "7" => true,
                "8" => true,
                "9" => true,
                _ => false
            };
    }
}