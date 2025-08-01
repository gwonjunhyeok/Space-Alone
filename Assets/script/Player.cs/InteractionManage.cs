using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManage : MonoBehaviour
{
    [SerializeField] private HashSet<string> CanInteractionObj;
    public Material newMaterial; // 추가할 머터리얼
    public Transform RayPos; // Raycast 발사 위치
    public float lineSize = 4f;
    public GameObject gun;
    public Transform handPos;//손 위치 : 플레이어 기준 조금 오른쪽에 위치
    private GameObject itemInHand = null;
    private Renderer previousRenderer; // 이전에 감지된 오브젝트의 Renderer

    private void Start()
    {
        // HashSet 초기화
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
                        Debug.Log("상호작용 성공");
                    }
                    else if (Input.GetKeyDown(KeyCode.F) && hit.collider.CompareTag("Cube"))
                    {
                        Debug.Log("노트 상호작용 성공");
                        PickUp(hitObject);
                        gun.SetActive(false);
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

    void AddMaterial(Renderer objRenderer)//아웃라인 머터리얼 추가 코드
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

    void RemoveMaterial(Renderer objRenderer)//시야 밖으로 나갔을 때 머터리얼 제거
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
    public void PickUp(GameObject item)
    {
        if (itemInHand != null)
        {
            Debug.Log("이미 아이템을 들고 있습니다.");
            return;
        }

        // 1. Collider 비활성화
        Collider col = item.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // 2. Rigidbody 비활성화
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        // 3. 손 위치로 이동하고 자식으로 설정
        item.transform.SetParent(handPos);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        itemInHand = item;

        Debug.Log("아이템을 손에 들었습니다: " + item.name);
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
