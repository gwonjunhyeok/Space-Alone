using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    [Header("¼³Á¤Ã¢")]
    public bool isUIActive = false;
    public float mouseSensitivity = 100f;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public int CamState = 1;
    public String Current_Gun = "basic_gun";
    public static GameManage instance;
    public GameObject InvenPanel;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }
    public void OpenInven()
    {
        if (Input.GetKeyDown(KeyCode.E)&&!isUIActive)
        {
            InvenPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isUIActive = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CamState == 1)
            {
                CamState = 2;
                ChangeScene.instance.Change();
                StartCoroutine(WaitTime(1.5f));
                ChangeScene.instance.Change1();
            }
            else
            {
                CamState = 1;
                ChangeScene.instance.Change1();
                StartCoroutine(WaitTime(1.5f));
                ChangeScene.instance.Change1();
            }
        }
        OpenInven();
    }
    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
