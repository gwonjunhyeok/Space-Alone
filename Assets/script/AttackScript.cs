using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public Transform firePoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = ObjectPool.Instance.GetBullet(); // 싱글톤으로 접근

            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;//위치 및 방향 지정
                Debug.Log("총알생성");
            }
        }
    }
}
