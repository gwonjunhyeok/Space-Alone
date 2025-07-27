using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 inputDirection;
    private bool jumpRequested = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (GameManage.instance.CamState == 1&&!GameManage.instance.isUIActive)
        {
            HandleInput();
            HandleRotation();
            HandleMovement();
            HandleJump();
        }
    }

    void FixedUpdate()
    {

    }

    // 입력 처리
    void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(h, 0, v).normalized;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpRequested = true;
        }
    }

    // 마우스 좌우 회전 처리
    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * GameManage.instance.mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    // 이동 처리 (MovePosition 사용)
    void HandleMovement()
    {
        if (inputDirection.sqrMagnitude < 0.01f)
        {
            rb.MovePosition(rb.position); // 입력이 없으면 즉시 정지
            return;
        }

        Vector3 move = transform.TransformDirection(inputDirection) * GameManage.instance.moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    // 점프 처리
    void HandleJump()
    {
        if (!jumpRequested) return;

        rb.AddForce(Vector3.up * GameManage.instance.jumpForce, ForceMode.Impulse);
        jumpRequested = false;
    }

    // 바닥에 닿아 있는지 확인
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
