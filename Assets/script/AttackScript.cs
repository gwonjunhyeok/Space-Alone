using TMPro;
using UnityEngine;
using GunsData;

public class AttackScript : MonoBehaviour
{
    public TMP_Text text;                // 탄약 수 UI 텍스트
    public Transform firePoint;          // 총알 발사 위치

    private GunData currentGunData;      // 현재 사용 중인 총기의 정보
    public int Current_magazine = 30;    // 현재 남은 탄 수
    private float fireDelayTimer = 0f;   // 총기 발사 딜레이 타이머

    void Start()
    {
        SetCurrentGun(GameManage.instance.Current_Gun);// 현재 무기 이름을 기반으로 총기 정보 설정
    }

    void Update()
    {
        DelayControl();
        FireBullet();
        Reload();
        UpdateMagazineUI();
        ChangeGun();
    }

    void SetCurrentGun(string gunName)
    {
        GameManage.instance.Current_Gun = gunName;

        if (GunDatabase.GunDict.TryGetValue(gunName, out currentGunData))
        {
            Current_magazine = currentGunData.maxMagazine;
            fireDelayTimer = 0f;
            Debug.Log("무기 설정됨: " + gunName);
        }
        else
        {
            Debug.LogError("총기 정보 없음: " + gunName);
        }
    }
    void ChangeGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCurrentGun("basic_gun");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetCurrentGun("shotgun");
        }
    }
    void FireBullet()
    {
        if (Input.GetMouseButton(0) && fireDelayTimer <= 0f && Current_magazine > 0)
        {
            GameObject bullet = ObjectPool.Instance.GetBullet(); // 오브젝트 풀에서 총알 가져오기

            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;

                fireDelayTimer = currentGunData.fireDelay;
                Current_magazine--;

                Debug.Log("총 발사됨");
            }
        }
    }

    void DelayControl()
    {
        if (fireDelayTimer > 0f)
        {
            fireDelayTimer -= Time.deltaTime;
        }
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Current_magazine < currentGunData.maxMagazine)
            {
                Current_magazine = currentGunData.maxMagazine;
                Debug.Log("재장전 완료");
            }
        }
    }

    void UpdateMagazineUI()
    {
        if (currentGunData != null)
        {
            text.text = Current_magazine + " / " + currentGunData.maxMagazine;
        }
    }
}
