 
namespace Shop
{
    public interface IShopItem
    {
        public int GoldCost { get; }
        public int GemCost { get; }
        void TryBuy();
    }
}
