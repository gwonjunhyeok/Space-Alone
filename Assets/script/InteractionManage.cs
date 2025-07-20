using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManage : MonoBehaviour
{
    [SerializeField] private HashSet<string> CanInteractionObj;
    public Material newMaterial; // 추가할 머터리얼
    public Transform RayPos; // Raycast 발사 위치
    public float lineSize = 4f;

    private Renderer previousRenderer; // 이전에 감지된 오브젝트의 Renderer

    private void Start()
    {
        // HashSet 초기화
        CanInteractionObj = new HashSet<string>
        {
            "Book", "Door"
        };
    }

    void Update()
    {
        CheckObject();
        
    }

    private void CheckObject()
    {
            Debug.DrawRay(RayPos.position, RayPos.forward * lineSize, Color.yellow);

            RaycastHit hit;
            if (Physics.Raycast(RayPos.position, RayPos.forward, out hit, lineSize))
            {
                GameObject hitObject = hit.collider.gameObject;
                Renderer objRenderer = hitObject.GetComponent<Renderer>();
                if (CanInteractionObj.Contains(hitObject.tag))
                {
                    if (Input.GetKeyDown(KeyCode.F) && hit.collider.CompareTag("Locker"))
                    {
                        Debug.Log("상호작용 성공");
                    }
                    else if (Input.GetKeyDown(KeyCode.F) && hit.collider.CompareTag("Note"))
                    {
                        Debug.Log("노트 상호작용 성공");
                    }
                    // 머터리얼 추가
                    if (objRenderer != null && previousRenderer != objRenderer)
                    {
                        if (previousRenderer != null)
                            RemoveMaterial(previousRenderer);

                        AddMaterial(objRenderer);
                        previousRenderer = objRenderer;
                    }
                }
                else
                {
                    if (previousRenderer != null)
                    {
                        RemoveMaterial(previousRenderer);
                        previousRenderer = null;
                    }
                }
            }
        }

    void AddMaterial(Renderer objRenderer)
    {
        if (newMaterial == null) return;

        Material[] currentMaterials = objRenderer.materials;
        Material[] newMaterials = new Material[currentMaterials.Length + 1];

        for (int i = 0; i < currentMaterials.Length; i++)
        {
            newMaterials[i] = currentMaterials[i];
        }
        newMaterials[newMaterials.Length - 1] = newMaterial;

        objRenderer.materials = newMaterials;
    }

    void RemoveMaterial(Renderer objRenderer)
    {
        Material[] currentMaterials = objRenderer.materials;

        if (currentMaterials.Length <= 1) return;

        Material[] newMaterials = new Material[currentMaterials.Length - 1];
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = currentMaterials[i];
        }

        objRenderer.materials = newMaterials;
    }
    public void AddInteractionTag(string newTag)
    {
        CanInteractionObj.Add(newTag); // 중복은 자동 무시됨
    }
}
