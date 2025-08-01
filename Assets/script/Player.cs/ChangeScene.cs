using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
//using static UnityEngine.Rendering.DebugUI;
using Image = UnityEngine.UI.Image;

public class ChangeScene : MonoBehaviour
{
    public static ChangeScene instance;
    public GameObject fadePanel; // 패널 오브젝트 (검은 배경용)
    public float fadeDuration = 1f;

    private Image panelImage;

    void Awake()
    {
        ChangeScene.instance = this;
        // 패널에서 Image 컴포넌트 가져오기
        panelImage = fadePanel.GetComponent<Image>();
    }

    void Start()
    {
        
    }
    private void Update()
    {
        
    }

    public void Change()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }
    public void Change1()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 0f;
        Color color = panelImage.color;
        color.a = 1f;
        panelImage.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = 1f - (time / fadeDuration);
            panelImage.color = color;
            yield return null;
        }

        color.a = 0f;
        panelImage.color = color;
    }

    IEnumerator FadeOutAndLoadScene()
    {
        float time = 0f;
        Color color = panelImage.color;
        color.a = 0f;
        panelImage.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = time / fadeDuration;
            panelImage.color = color;
            yield return null;
        }

        color.a = 1f;
        panelImage.color = color;
    }
}
