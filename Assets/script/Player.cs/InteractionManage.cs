using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManage : MonoBehaviour
{
    [SerializeField] private HashSet<string> CanInteractionObj;
    public Material newMaterial; // �߰��� ���͸���
    public Transform RayPos; // Raycast �߻� ��ġ
    public float lineSize = 4f;
    public GameObject gun;
    public Transform handPos;//�� ��ġ : �÷��̾� ���� ���� �����ʿ� ��ġ
    private GameObject itemInHand = null;
    private Renderer previousRenderer; // ������ ������ ������Ʈ�� Renderer

    private void Start()
    {
        // HashSet �ʱ�ȭ
        CanInteractionObj = new HashSet<string>
        {
            "Book", "Cube"
        };
    }

    void Update()
    {
        CheckObject();
        DropItem();
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
                    else if (Input.GetKeyDown(KeyCode.F) && hit.collider.CompareTag("Cube"))
                    {
                        Debug.Log("��Ʈ ��ȣ�ۿ� ����");
                        PickUp(hitObject);
                        gun.SetActive(false);
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

    void AddMaterial(Renderer objRenderer)//�ƿ����� ���͸��� �߰� �ڵ�
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

    void RemoveMaterial(Renderer objRenderer)//�þ� ������ ������ �� ���͸��� ����
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
    public void PickUp(GameObject item)
    {
        if (itemInHand != null)
        {
            Debug.Log("�̹� �������� ��� �ֽ��ϴ�.");
            return;
        }

        // 1. Collider ��Ȱ��ȭ
        Collider col = item.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // 2. Rigidbody ��Ȱ��ȭ
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        // 3. �� ��ġ�� �̵��ϰ� �ڽ����� ����
        item.transform.SetParent(handPos);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        itemInHand = item;

        Debug.Log("�������� �տ� ������ϴ�: " + item.name);
    }
    public void DropItem()
    {
        if (Input.GetKeyDown(KeyCode.Q) && itemInHand!=null)
        {
            if (itemInHand == null) return;

            itemInHand.transform.SetParent(null);

            Collider col = itemInHand.GetComponent<Collider>();
            if (col != null)
                col.enabled = true;

            Rigidbody rb = itemInHand.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.detectCollisions = true;
                rb.AddForce(handPos.forward * 9f, ForceMode.Impulse);
            }

            itemInHand = null;
        }
    }
}
