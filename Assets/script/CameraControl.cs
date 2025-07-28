using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerBody; // �÷��̾��� Ʈ�������� ���� (ȸ�� �� ������)

    private float xRotation = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        if (!GameManage.instance.isUIActive)
        {
            HandleLook(); 
        }
    }

    // ���� ȸ�� ó��
    void HandleLook()
    {
        float mouseY = Input.GetAxis("Mouse Y") * GameManage.instance.mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f); 

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
