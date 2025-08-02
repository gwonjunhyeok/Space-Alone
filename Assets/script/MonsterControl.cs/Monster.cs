using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public Transform target;

    private NavMeshAgent agent;
    private MonsterData data;

    private float attackTimer;
    private int currentHP;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject player = GameObject.FindWithTag("Player");//태그로 플레이어 인식
        if (player != null)
            target = player.transform;

        MonsterDatabase db = FindObjectOfType<MonsterDatabase>();//태그로 몬스터 인식
        data = db.GetMonsterDataByName(gameObject.tag);

        if (data != null)
        {
            currentHP = data.HP;
            attackTimer = data.A_Delay;
            agent.speed = data.Speed;
            agent.stoppingDistance = data.A_Range;
        }
    }

    void Update()
    {
        if (target == null || agent == null || data == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > data.A_Range)
        {
            // 공격 사정거리 밖이면 따라감
            agent.SetDestination(target.position);
        }
        else
        {
            agent.ResetPath();//재정의 

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = data.A_Delay;
            }
        }
    }

    void Attack()
    {
        int pattern = 1;

        if (data.A_Pattern >= 2)
        {
            // 1~A_Pattern 사이에서 랜덤 선택
            pattern = Random.Range(1, data.A_Pattern + 1); // 상한 포함
        }

        Debug.Log($"{data.Name}이(가) 공격 패턴 {pattern}번 공격을 시전함! 피해량: {data.A_Damage}");
    }
}
