using TMPro;
using UnityEngine;
using GunsData;

public class AttackScript : MonoBehaviour
{
    public TMP_Text text;                // ź�� �� UI �ؽ�Ʈ
    public Transform firePoint;          // �Ѿ� �߻� ��ġ

    private GunData currentGunData;      // ���� ��� ���� �ѱ��� ����
    public int Current_magazine = 30;    // ���� ���� ź ��
    private float fireDelayTimer = 0f;   // �ѱ� �߻� ������ Ÿ�̸�

    void Start()
    {
        SetCurrentGun(GameManage.instance.Current_Gun);// ���� ���� �̸��� ������� �ѱ� ���� ����
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
            Debug.Log("���� ������: " + gunName);
        }
        else
        {
            Debug.LogError("�ѱ� ���� ����: " + gunName);
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
            GameObject bullet = ObjectPool.Instance.GetBullet(); // ������Ʈ Ǯ���� �Ѿ� ��������

            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;

                fireDelayTimer = currentGunData.fireDelay;
                Current_magazine--;

                Debug.Log("�� �߻��");
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
                Debug.Log("������ �Ϸ�");
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
