using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    private float timer;

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            gameObject.SetActive(false); // 다시 풀로 돌아감
            Debug.Log("총알 생명주기 종료");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
        Debug.Log("총알 충돌 후 삭제");
    }
}