using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    Animator anim;
    public Transform target;
    // 0 = idle / 1 = tracking / 2 = attack
    public int state = 0;

    private NavMeshAgent agent;
    private MonsterData data;
    private bool isSeeingPlayer = false;

    private float attackTimer;
    private int currentHP;

    public Transform playertr;

    [SerializeField] private float turnSpeed = 540f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float loseSightAfter = 1.0f;

    private float lastSeenTime = -999f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        // 우리가 직접 회전 제어
        if (agent) agent.updateRotation = false;

        if (playertr == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) playertr = p.transform;
        }

        target = null;

        MonsterDatabase db = FindObjectOfType<MonsterDatabase>();
        data = db != null ? db.GetMonsterDataByName(gameObject.tag) : null;

        if (data != null && agent != null)
        {
            currentHP = data.HP;
            attackTimer = data.A_Delay;
            agent.speed = data.Speed;
            agent.stoppingDistance = data.A_Range;
            agent.isStopped = true;   // 시작은 정지
        }

        state = 0;
        UpdateAnimation();
        if (data != null && agent != null)
        {
            currentHP = data.HP;
            attackTimer = data.A_Delay;
            Debug.Log("초기값 지정 완료");
            ApplyStatsFromData();
            agent.isStopped = true;
        }
    }
    private void ApplyStatsFromData()
    {
        if (agent == null || data == null) return;

        agent.speed = Mathf.Max(0f, data.Speed);
        agent.stoppingDistance = Mathf.Max(0f, data.A_Range);
        agent.autoBraking = true;
        agent.angularSpeed = 120f;
    }
    void Update()
    {
        View(); // 시야 판정 + 추격/정지/바라보기까지 처리

        if (agent == null || data == null) return;
        if (!agent.isOnNavMesh) return;

        if (target != null && isSeeingPlayer)
        {
            float stopRange = Mathf.Max(0.1f, data.A_Range);
            float distanceToTgt = Vector3.Distance(transform.position, target.position);

            if (distanceToTgt <= stopRange)
            {
                // 공격
                if (!agent.isStopped) { agent.isStopped = true; agent.ResetPath(); }
                FaceTarget(target.position);

                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0f)
                {
                    state = 2; // attack
                    UpdateAnimation();
                    Attack();
                    attackTimer = data.A_Delay;
                }
            }
        }
    }

    // === 시야/추격/정지
    private void View()
    {
        Vector3 origin = transform.position + Vector3.up * 1.0f;

        Vector3 fwd = transform.forward; fwd.y = 0f;
        if (fwd.sqrMagnitude < 0.0001f) fwd = Vector3.forward;
        fwd.Normalize();

        Vector3 left = Quaternion.AngleAxis(-viewAngle * 0.5f, Vector3.up) * fwd;
        Vector3 right = Quaternion.AngleAxis(viewAngle * 0.5f, Vector3.up) * fwd;
        Debug.DrawRay(origin, left * viewDistance, Color.red);
        Debug.DrawRay(origin, right * viewDistance, Color.red);

        Collider[] candidates = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        Transform seen = null;
        float bestDot = -1f;

        foreach (Collider col in candidates)
        {
            if (!col.CompareTag("Player")) continue;

            Vector3 to = col.transform.position - transform.position;
            to.y = 0f;
            if (to.sqrMagnitude < 0.0001f) continue;

            Vector3 dir = to.normalized;
            float angle = Vector3.Angle(fwd, dir);
            if (angle > viewAngle * 0.5f) continue;

            float dist = to.magnitude;
            // obstacleMask에는 Player 레이어가 포함되면 안 됨
            if (Physics.Raycast(origin, (col.transform.position - origin).normalized, out RaycastHit hit, dist, obstacleMask))
                continue;

            float dot = Vector3.Dot(fwd, dir);
            if (dot > bestDot)
            {
                bestDot = dot;
                seen = col.transform;
            }

            Debug.DrawRay(origin, dir * Mathf.Min(viewDistance, dist), Color.blue);
        }

        if (seen != null && agent != null && agent.isOnNavMesh)
        {
            target = seen;
            lastSeenTime = Time.time;

            if (!isSeeingPlayer) Debug.Log("[FOV] Player IN");

            isSeeingPlayer = true;

            // 추격 시작 (여기서만)
            Vector3 dest = target.position;
            if (NavMesh.SamplePosition(dest, out var hitNM, 2.0f, NavMesh.AllAreas))
                dest = hitNM.position;

            agent.isStopped = false;

            if (!agent.hasPath || (agent.destination - dest).sqrMagnitude > 0.25f)
                agent.SetDestination(dest);

            state = 1; // tracking
            UpdateAnimation();

            FaceTarget(target.position);
        }
        else
        {
            bool keep = (Time.time - lastSeenTime) <= loseSightAfter;

            if (!keep && isSeeingPlayer) Debug.Log("[FOV] Player OUT");

            isSeeingPlayer = keep;
            if (!keep)
            {
                target = null;
                if (agent != null)
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                }
                state = 0; // idle
                UpdateAnimation();
            }
        }
    }

    private void FaceTarget(Vector3 worldPoint)
    {
        Vector3 look = worldPoint - transform.position;
        look.y = 0f;
        if (look.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        if (anim != null)
            anim.SetInteger("MonsterState", state); // 0 idle / 1 tracking / 2 attack
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.3f, 0.2f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 left = BoundaryAngle(-viewAngle * 0.5f) * viewDistance;
        Vector3 right = BoundaryAngle(viewAngle * 0.5f) * viewDistance;
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + left);
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + right);
    }

    void Attack()
    {
        int pattern = 1;
        if (data != null && data.A_Pattern >= 2)
            pattern = Random.Range(1, data.A_Pattern + 1);

        Debug.Log($"{(data != null ? data.Name : name)}이(가) 패턴 {pattern} 공격");
        // 실제 공격 판정/이펙트는 여기서
    }

    public void MonsterPattern(int M_P) { /* 패턴별 코루틴 필요 시 구현 */ }

    IEnumerator BugMonster_Attack1()
    {
        if (target == null || data == null) yield break;

        float rotateDuration = 0.12f;
        float t = 0f;
        Quaternion startRot = transform.rotation;
        Vector3 dir = (target.position - transform.position); dir.y = 0f;
        Quaternion endRot = dir.sqrMagnitude > 0.0001f ? Quaternion.LookRotation(dir) : startRot;

        while (t < rotateDuration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, t / rotateDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRot;
    }
}
