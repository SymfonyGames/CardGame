// using System.Collections;
// using UnityEngine;
//
// namespace Misc
// {
//     public class BankStatsAnimator : MonoBehaviour
//     {
//         StatsPanelController _controller;
//         [SerializeField] float incrementDuration = 1;
//         [SerializeField] float animDelay;
//
//         public void Init(StatsPanelController controller)
//         {
//             _controller = controller;
//         }
//
//         public void IncrementAnimation(BankCurrencyEnum type, int amount)
//             => StartCoroutine(Increment(type, amount));
//
//         IEnumerator Increment(BankCurrencyEnum type, int amount)
//         {
//             var increment = amount;
//             
//             switch (type)
//             {
//                 case BankCurrencyEnum.Gold:
//                     _controller.AnimatedGoldAmount += increment;
//                     break;
//                 case BankCurrencyEnum.GEM:
//                     _controller.AnimatedGemAmount += increment;
//                     break;
//                 case BankCurrencyEnum.Energy:
//                     _controller.AnimatedEnergyAmount += increment;
//                     break;
//                 default:
//                     Log.BankMiss();
//                     break;
//             }
//
//             _controller.Refresh();
//             yield return new WaitForSeconds(animDelay);
//
//             var speed = amount / incrementDuration;
//             float value = 0;
//             while (increment > 0)
//             {
//                 value += Time.deltaTime * speed;
//                 if (value > 1)
//                 {
//                     value--;
//                     increment--;
//                     Decrement(type);
//                 }
//
//                 yield return null;
//             }
//         }
//
//
//         void Decrement(BankCurrencyEnum type)
//         {
//             switch (type)
//             {
//                 case BankCurrencyEnum.Gold:
//                     _controller.AnimatedGoldAmount--;
//                     break;
//                 case BankCurrencyEnum.GEM:
//                     _controller.AnimatedGemAmount--;
//                     break;
//                 case BankCurrencyEnum.Energy:
//                     _controller.AnimatedEnergyAmount--;
//                     break;
//                 default:
//                     Log.BankMiss();
//                     break;
//             }
//
//             _controller.Refresh();
//         }
//     }
// }