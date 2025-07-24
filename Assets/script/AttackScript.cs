using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public TMP_Text text;
    public int Max_magazine = 30;
    public int Current_magazine = 30;
    public float FireDelay = 0f;
    public Transform firePoint;

    void Update()
    {
        DelayControl();
        FireBullet();
        Reload();
        MagazineText();
    }
    void MagazineText()
    {
        if (GameManage.instance.Current_Gun == "basic_gun")
        {
            text.text = Current_magazine + " / " + Max_magazine;
        }
    }
    public void FireBullet()
    {
        if (Input.GetMouseButton(0))
        {
            if (Current_magazine > 0 && FireDelay <= 0f)
            {
                GameObject bullet = ObjectPool.Instance.GetBullet();

                if (bullet != null)
                {
                    bullet.transform.position = firePoint.position;
                    bullet.transform.rotation = firePoint.rotation;
                    FireDelay = 0.05f; // 0.5초 간격
                    Current_magazine--;
                    Debug.Log("총알 발사");
                }
            }
        }
    }

    public void DelayControl()
    {
        if (FireDelay > 0f)
        {
            FireDelay -= Time.deltaTime;
        }
    }

    public void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Current_magazine < Max_magazine)
            {
                // 재장전 시간 등 추가 코드 필요
                Current_magazine = Max_magazine;
                Debug.Log("재장전 완료");
            }
        }
    }
}
