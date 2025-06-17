using System;

namespace Tutorials
{
   public interface ITutorialItem
   {
      float Delay { get; }
      event Action OnComplete;
      void Init(TutorialConfig tutorial);
      void StartItem();
   }
}
