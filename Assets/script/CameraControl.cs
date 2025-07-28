using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerBody; // 플레이어의 트랜스폼을 참조 (회전 축 고정용)

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

    // 상하 회전 처리
    void HandleLook()
    {
        float mouseY = Input.GetAxis("Mouse Y") * GameManage.instance.mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f); 

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
