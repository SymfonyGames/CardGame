public interface IMana
{
    public float Mana { get;   }
    public float MaxMana { get;   }
    void AddMana(float amount);
    void DecreaseMana(float amount);
}