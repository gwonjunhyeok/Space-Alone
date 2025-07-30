using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inven_Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI countText;

    public int slotIndex;      // 이 슬롯의 인덱스
    public bool isMainSlot;    // 메인 인벤인지 여부

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

                // 아이템이 있는 경우에만 드래그 시작
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

    public void OnPointerDown(PointerEventData eventData)//함수는 해당 오브젝트 위에서 마우스 버튼이 내려가는 순간
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isPointerDown = true;
            holdTime = 0f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)//함수는 해당 오브젝트 위에서  눌렀던 마우스 버튼을 떼는 순간
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
                    invenSystem.DiscardItem(); // 아이템 버리기 처리
                }
                else
                {
                    invenSystem.ReturnDragItem(); // 인벤 내부지만 슬롯 아님: 되돌리기
                }
            }
        }
    }

}
