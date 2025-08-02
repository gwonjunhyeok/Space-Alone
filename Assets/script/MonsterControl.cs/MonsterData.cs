[System.Serializable]
public class MonsterData
{
    public string Name;
    public int HP;
    public float Speed;
    public float A_Range;
    public float A_Delay;
    public int A_Damage;
    public int A_Pattern;

    public MonsterData(string name, int hp, float speed, float aRange, float aDelay, int aDamage, int aPattern)
    {
        Name = name;
        HP = hp;
        Speed = speed;
        A_Range = aRange;
        A_Delay = aDelay;
        A_Damage = aDamage;
        A_Pattern = aPattern;
    }
}