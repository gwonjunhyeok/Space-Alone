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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isPointerDown = true;
            holdTime = 0f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        holdTime = 0f;

        if (eventData.button == PointerEventData.InputButton.Left &&
            DragSlot.Instance.dragData != null)
        {
            invenSystem.OnSlotDrop(isMainSlot, slotIndex);
        }
        else
        {
            // 드래그 중이지만 슬롯이 아닌 데서 뗀 경우
            if (DragSlot.Instance.dragData != null)
            {
                invenSystem.ReturnDragItem();  // 원래 자리로 되돌림
            }
        }
    }
}
