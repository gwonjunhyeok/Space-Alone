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

        GameObject player = GameObject.FindWithTag("Player");//�±׷� �÷��̾� �ν�
        if (player != null)
            target = player.transform;

        MonsterDatabase db = FindObjectOfType<MonsterDatabase>();//�±׷� ���� �ν�
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
            // ���� �����Ÿ� ���̸� ����
            agent.SetDestination(target.position);
        }
        else
        {
            agent.ResetPath();//������ 

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
            // 1~A_Pattern ���̿��� ���� ����
            pattern = Random.Range(1, data.A_Pattern + 1); // ���� ����
        }

        Debug.Log($"{data.Name}��(��) ���� ���� {pattern}�� ������ ������! ���ط�: {data.A_Damage}");
    }
}
