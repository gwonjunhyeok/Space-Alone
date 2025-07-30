using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inven_Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI countText;

    public int slotIndex;      // �� ������ �ε���
    public bool isMainSlot;    // ���� �κ����� ����

    private bool isPointerDown = false;
    private float holdTime = 0f;
    private const float requiredHoldTime = 0.3f;

    private Inven_System invenSystem;
    private bool hasItem = false;

    private void Start()
    {
        invenSystem = FindObjectOfType<Inven_System>();
    }
    private void Awake()
    {
        
    }
    private void Update()
    {
        if (isPointerDown)
        {
            holdTime += Time.deltaTime;
            if (holdTime >= requiredHoldTime)
            {
                isPointerDown = false;

                // �������� �ִ� ��쿡�� �巡�� ����
                if (hasItem)
                {
                    invenSystem.OnSlotClicked(isMainSlot, slotIndex);
                }
            }
        }
    }

    public void SetItem(ItemData data, int count)
    {
        if (data != null)
        {
            image.sprite = data.icon;
            image.enabled = true;
            countText.text = count > 1 ? count.ToString() : "";
            hasItem = true;
        }
    }

    public void ClearSlot()
    {
        image.sprite = null;
        //image.enabled = false;
        countText.text = "";
        hasItem = false;
    }

    public void OnPointerDown(PointerEventData eventData)//�Լ��� �ش� ������Ʈ ������ ���콺 ��ư�� �������� ����
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isPointerDown = true;
            holdTime = 0f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)//�Լ��� �ش� ������Ʈ ������  ������ ���콺 ��ư�� ���� ����
    {
        isPointerDown = false;
        holdTime = 0f;

        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (DragSlot.Instance.dragData != null)
        {
            if (invenSystem.IsPointerOverSlot(out Inven_Slot targetSlot))
            {
                invenSystem.OnSlotDrop(targetSlot.isMainSlot, targetSlot.slotIndex);
            }
            else
            {
                if (invenSystem.IsPointerOutsideInventory())
                {
                    invenSystem.DiscardItem(); // ������ ������ ó��
                }
                else
                {
                    invenSystem.ReturnDragItem(); // �κ� �������� ���� �ƴ�: �ǵ�����
                }
            }
        }
    }

}
