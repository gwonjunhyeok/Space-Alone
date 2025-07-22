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
            GameObject bullet = ObjectPool.Instance.GetBullet(); // �̱������� ����

            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;//��ġ �� ���� ����
                Debug.Log("�Ѿ˻���");
            }
        }
    }
}
