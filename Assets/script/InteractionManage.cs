using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManage : MonoBehaviour
{
    [SerializeField] private HashSet<string> CanInteractionObj;
    public Material newMaterial; // �߰��� ���͸���
    public Transform RayPos; // Raycast �߻� ��ġ
    public float lineSize = 4f;

    private Renderer previousRenderer; // ������ ������ ������Ʈ�� Renderer

    private void Start()
    {
        // HashSet �ʱ�ȭ
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
                        Debug.Log("��ȣ�ۿ� ����");
                    }
                    else if (Input.GetKeyDown(KeyCode.F) && hit.collider.CompareTag("Note"))
                    {
                        Debug.Log("��Ʈ ��ȣ�ۿ� ����");
                    }
                    // ���͸��� �߰�
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
        CanInteractionObj.Add(newTag); // �ߺ��� �ڵ� ���õ�
    }
}
