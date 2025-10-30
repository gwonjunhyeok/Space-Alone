using System.Collections.Generic;
using UnityEngine;

public class MonsterDatabase : MonoBehaviour
{
    public List<MonsterData> monsterList = new();

    void Awake()
    {
        // 초기 몬스터 데이터 등록
        //string name, int hp, float speed, float aRange, float aDelay, int aDamage, int aPattern 순서
        monsterList.Add(new MonsterData("BugMonster", 150, 3f,4f, 3f, 10, 2));
        monsterList.Add(new MonsterData("SmallBug", 30,4f, 3f, 4f, 5, 1));
    }

    // 이름으로 몬스터 데이터 가져오기
    public MonsterData GetMonsterDataByName(string name)
    {
        return monsterList.Find(m => m.Name == name);
    }
}
