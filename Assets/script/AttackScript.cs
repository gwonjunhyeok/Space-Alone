using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public int Max_magazine = 30;
    public int Current_magazine = 30;
    public Transform firePoint;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(Current_magazine>=0)
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
    public void Reload()
    {
        if(Max_magazine==Current_magazine){
        //재장전 시간 추가 필요
        //총기별 탄창수 구분 필요
        Current_magazine=Max_magazine;
        }
    }
}
