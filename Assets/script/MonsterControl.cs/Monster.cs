using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public Transform target;
    public int state = 1;//1 = idle / 2 = attack / 3 = tracking
    private NavMeshAgent agent;
    private MonsterData data;

    private float attackTimer;
    private int currentHP;
    public Transform playertr;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
            target = playertr.transform;

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

        Debug.Log($"{data.Name}이(가) 공격 패턴 {pattern}번 공격, 피해량: {data.A_Damage}");
    }
    public void MonsterPattern(int M_P)
    {
        if (data == null) return;
        //if (isAttacking) return; // 이미 공격 중이면 무시

        // 패턴 값 범위 보정 (1..data.A_Pattern)
        int pattern = Mathf.Clamp(M_P, 1, Mathf.Max(1, data.A_Pattern));

        // 몬스터 식별: data.Name 우선, 없으면 Tag 사용
        string type = !string.IsNullOrEmpty(data.Name) ? data.Name : gameObject.tag;
        type = type.ToLowerInvariant();
        if (type == "BugMonster")
        {
        //    if (pattern == 1)
        //        StartCoroutine(BugMonster_Attack1());
        //    else if (pattern == 2)
        //        StartCoroutine(Orc_Attack2());
        //    else
        //        StartCoroutine(Orc_Attack1()); // fallback
        //}
        //else if (type == "SmallBug")
        //{
        //    if (pattern == 1)
        //        StartCoroutine(Bug_Attack1());
        //    else if (pattern == 2)
        //        StartCoroutine(Bug_Attack2());
        //    else
        //        StartCoroutine(Bug_Attack1()); // fallback
        }
        else
        {
            // 기본 몬스터 처리: pattern으로 구분하거나 기본 공격 실행
            //if (pattern == 1)
            //    StartCoroutine(Default_Attack());
            //else
            //    StartCoroutine(Default_Attack());
        }
    }
    IEnumerator BugMonster_Attack1()
    {
        if (target == null || data == null) yield break;

        // 1) 빠르게 타겟을 바라보기 (부드럽게)
        float rotateDuration = 0.12f;
        float t = 0f;
        Quaternion startRot = transform.rotation;
        Vector3 dir = (target.position - transform.position);
        dir.y = 0f;
        Quaternion endRot = dir.sqrMagnitude > 0.0001f ? Quaternion.LookRotation(dir) : startRot;
        while (t < rotateDuration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, t / rotateDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRot;
        yield return null;
    }
}
