using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVControl : MonoBehaviour
{
    public GameObject Cam1;
    public GameObject Cam2;
    public GameObject Cam3;
    public float rotationSpeed = 100f;   // ���콺 ���� �Ǵ� ȸ�� �ӵ�
    public float minAngle = -30f;        // �ּ� ȸ�� ����
    public float maxAngle = 30f;         // �ִ� ȸ�� ����
    private float currentYRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentYRotation = transform.localEulerAngles.y;
        if (currentYRotation > 180f)
            currentYRotation -= 360f;

        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Cam();
        if (GameManage.instance.CamState == 1)
        {
            Cam1.SetActive(true);
            Cam2.SetActive(false);
            Cam3.SetActive(false);
        }
        else if (GameManage.instance.CamState == 2)
        {
            Cam1.SetActive(false);
            Cam2.SetActive(true);
            Cam3.SetActive(false);
        }
    }
    void Cam()
    {
        if (GameManage.instance.CamState != 1)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            currentYRotation += mouseX;

            // ȸ�� ���� ����
            currentYRotation = Mathf.Clamp(currentYRotation, minAngle, maxAngle);

            transform.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);
        }
    }
}
