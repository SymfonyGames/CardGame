public interface IHitable
{
    public float AttackPower { get; }
    void Hit(float damage);
}