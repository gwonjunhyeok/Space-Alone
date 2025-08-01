using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Basic Gun Pool")]
    public static ObjectPool Instance;
    public int poolSize = 30;
    private List<GameObject> bulletPool;
    public GameObject bulletPrefab;

    [Header("Monster Pool")]
    public int MonsterPoolSize = 10;
    public GameObject MonsterPrefab;
    private List<GameObject> MonsterPool;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new List<GameObject>();
        //Ǯ �����ŭ ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
        MonsterPool = new List<GameObject>();
        //Ǯ �����ŭ ����
        for (int i = 0; i < MonsterPoolSize; i++)
        {
            GameObject monster = Instantiate(MonsterPrefab);
            monster.SetActive(false);
            MonsterPool.Add(monster);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetBullet()
    {
        foreach (var bullet in bulletPool)//pool�� ������ �Ѿ� �˻�
        {
            if (!bullet.activeInHierarchy)//Ȱ��ȭ ���°� �ƴϸ�
            {
                bullet.SetActive(true);
                return bullet;
            }
        }
        return null;
    }
    public GameObject GetMonster()
    {
        foreach (var m in MonsterPool)
        {
            if (!m.activeInHierarchy)
            {
                m.SetActive(true);
                return m;
            }
        }
        return null;
    }
}
