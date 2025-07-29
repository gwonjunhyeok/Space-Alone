using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Inven_System : MonoBehaviour
{
    public List<ItemStack> items = new();

    [SerializeField] private Transform mainSlotParent;
    [SerializeField] private Transform subSlotParent;
    [SerializeField] private Inven_Slot[] mainSlots;
    [SerializeField] private Inven_Slot[] subSlots;

    private List<ItemStack> mainItems = new();
    private List<ItemStack> subItems = new();

#if UNITY_EDITOR
    private void OnValidate()
    {
        mainSlots = mainSlotParent.GetComponentsInChildren<Inven_Slot>();
        subSlots = subSlotParent.GetComponentsInChildren<Inven_Slot>();
    }
#endif

    private void Awake()
    {
        for (int i = 0; i < mainSlots.Length; i++)
        {
            mainSlots[i].slotIndex = i;
            mainSlots[i].isMainSlot = true;
        }

        for (int i = 0; i < subSlots.Length; i++)
        {
            subSlots[i].slotIndex = i;
            subSlots[i].isMainSlot = false;
        }

        FreshSlot();
    }

    private void Update()
    {
        UseSubItem();

        if (Input.GetKeyDown(KeyCode.H))
        {
            ItemData item = FindItemByName("sword");
            if (item != null) AddItem(item);
            else Debug.LogWarning("아이템 'sword'를 찾을 수 없습니다.");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            ItemData item = FindItemByName("diamond");
            if (item != null) AddItem(item);
            else Debug.LogWarning("아이템 'sword'를 찾을 수 없습니다.");
        }
    }

    public void FreshSlot()
    {
        for (int i = 0; i < mainSlots.Length; i++)
        {
            if (i < mainItems.Count && mainItems[i] != null)
                mainSlots[i].SetItem(mainItems[i].itemData, mainItems[i].count);
            else
                mainSlots[i].ClearSlot();
        }

        for (int i = 0; i < subSlots.Length; i++)
        {
            if (i < subItems.Count && subItems[i] != null)
                subSlots[i].SetItem(subItems[i].itemData, subItems[i].count);
            else
                subSlots[i].ClearSlot();
        }
    }

    public void AddItem(ItemData item)
    {
        if (TryAddToList(mainItems, mainSlots.Length, item)) return;
        if (TryAddToList(subItems, subSlots.Length, item)) return;
        Debug.Log("인벤토리 공간이 부족합니다.");
    }

    private bool TryAddToList(List<ItemStack> list, int limit, ItemData item)
    {
        foreach (var stack in list)
        {
            if (stack != null && stack.itemData.itemName == item.itemName && !stack.IsFull)
            {
                stack.count++;
                FreshSlot();
                return true;
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
            {
                list[i] = new ItemStack(item);
                FreshSlot();
                return true;
            }
        }

        if (list.Count < limit)
        {
            list.Add(new ItemStack(item));
            FreshSlot();
            return true;
        }

        return false;
    }

    public void OnSlotClicked(bool isMain, int index)
    {
        var list = isMain ? mainItems : subItems;
        if (index >= list.Count || list[index] == null) return;

        var stack = list[index];
        bool isSplit = Input.GetKey(KeyCode.LeftControl);
        int moveCount = isSplit ? Mathf.FloorToInt(stack.count / 2f) : stack.count;
        if (moveCount <= 0) return;

        if (isSplit) stack.count -= moveCount;
        else list[index] = null;

        DragSlot.Instance.StartDrag(isMain, index, new ItemStack(stack.itemData, moveCount), isSplit);
        FreshSlot();
    }

    public void OnSlotDrop(bool isMainTarget, int targetIndex)
    {
        var drag = DragSlot.Instance.dragData;
        if (drag == null) return;

        var fromList = drag.fromMain ? mainItems : subItems;
        var toList = isMainTarget ? mainItems : subItems;

        // drag 정보
        var dragged = drag.draggedItem;
        var originIndex = drag.originIndex;

        // 병합 가능한 경우
        if (targetIndex < toList.Count && toList[targetIndex] != null)
        {
            var targetStack = toList[targetIndex];

            if (targetStack.itemData.itemName == dragged.itemData.itemName)
            {
                int total = targetStack.count + dragged.count;
                int max = targetStack.itemData.maxStack;

                if (total <= max)
                {
                    targetStack.count = total;
                    if (!drag.isSplit) fromList[originIndex] = null;
                }
                else
                {
                    int canMove = max - targetStack.count;
                    targetStack.count = max;
                    dragged.count -= canMove;

                    if (!drag.isSplit)
                    {
                        if (fromList[originIndex] == null)
                            fromList[originIndex] = dragged;
                        else
                            fromList[originIndex].count = dragged.count;
                    }
                    ReturnDragItem(); // 잔여분 다시 원래 슬롯으로
                }

                DragSlot.Instance.ClearDrag();
                FreshSlot();
                return;
            }
        }

        // 교체 로직
        if (targetIndex < toList.Count && toList[targetIndex] != null)
        {
            var temp = toList[targetIndex];
            toList[targetIndex] = dragged;
            fromList[originIndex] = temp;
        }
        else
        {
            while (toList.Count <= targetIndex) toList.Add(null);
            toList[targetIndex] = dragged;

            if (!drag.isSplit)
            {
                if (originIndex < fromList.Count)
                    fromList[originIndex] = null;
            }
        }

        DragSlot.Instance.ClearDrag();
        FreshSlot();
    }

    public void ReturnDragItem()
    {
        var drag = DragSlot.Instance.dragData;
        if (drag == null) return;

        var list = drag.fromMain ? mainItems : subItems;
        if (drag.originIndex < list.Count && list[drag.originIndex] == null)
            list[drag.originIndex] = drag.draggedItem;
        else if (!list.Contains(drag.draggedItem))
            list.Insert(drag.originIndex, drag.draggedItem);

        DragSlot.Instance.ClearDrag();
        FreshSlot();
    }

    public bool IsPointerOverSlot(out Inven_Slot slot)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            slot = result.gameObject.GetComponentInParent<Inven_Slot>();
            if (slot != null)
            {
                return true;
            }
        }

        slot = null;
        return false;
    }

    private void UseSubItem()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i >= subItems.Count) return;
                if (subItems[i] == null) return;

                Debug.Log($"서브 아이템 사용: {subItems[i].itemData.itemName}");
                subItems[i].count--;
                if (subItems[i].count <= 0) subItems[i] = null;
                FreshSlot();
                return;
            }
        }
    }

    private ItemData FindItemByName(string name)
    {
        ItemData[] allItems = Resources.LoadAll<ItemData>("");
        foreach (var item in allItems)
        {
            if (item.itemName == name) return item;
        }
        return null;
    }
}
