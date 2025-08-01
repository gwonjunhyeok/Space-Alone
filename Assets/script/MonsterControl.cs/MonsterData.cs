[System.Serializable]
public class MonsterData
{
    public string Name;
    public int HP;
    public float A_Delay;
    public int A_Damage;
    public int A_Pattern;

    public MonsterData(string name, int hp, float aDelay, int aDamage, int aPattern)
    {
        Name = name;
        HP = hp;
        A_Delay = aDelay;
        A_Damage = aDamage;
        A_Pattern = aPattern;
    }
}