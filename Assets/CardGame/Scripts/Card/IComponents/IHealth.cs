
public interface IHealth
{
    public float Health { get; }
    void AddHealth(float amount);
    void DecreaseHealth(float amount);
}