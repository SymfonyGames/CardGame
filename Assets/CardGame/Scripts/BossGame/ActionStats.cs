using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BossGame
{
    [System.Serializable]
    public class ActionStats
    {
        public AnimationCurve curve;
        public float factor = 4f;
        public List<ActionData> actions = new();
        public bool None => actions.Count == 0;

        public ActionData Random
        {
            get
            {
                var r = UnityEngine.Random.Range(0, 100) * 0.01f;
                var sum = 0f;

                for (var i = 0; i < actions.Count; i++)
                {
                    var chance = GetChance(i);
                    sum += chance;

                    if (r <= sum)
                    {
                        return actions[i];
                    }
                }

                Debug.LogError("Null returned");
                return null;
            }
        }

        public float GetChance(int curveId)
        {
            if (actions.Count == 1) return 1;
            var f = 1 / factor;

            var point = (float) curveId / (actions.Count - 1);
            var value = curve.Evaluate(point);

            var factorValue = value + f;
            var factorTotal = TotalChance + f * actions.Count;

            return factorValue / factorTotal;
        }

        float TotalChance => actions
            .Select((t, i) => i / (float) (actions.Count - 1))
            .Sum(curve.Evaluate);
    }
}