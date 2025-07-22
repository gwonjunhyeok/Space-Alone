using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public int poolSize = 3;
    private List<GameObject> bulletPool;
    public GameObject bulletPrefab;
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
}
