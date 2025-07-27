using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven_System : MonoBehaviour
{
    public List<ItemStack> items = new List<ItemStack>();

    [SerializeField] private Transform mainSlotParent;
    [SerializeField] private Transform subSlotParent;

    [SerializeField] private Inven_Slot[] mainSlots;
    [SerializeField] private Inven_Slot[] subSlots;

    private List<ItemStack> mainItems = new List<ItemStack>();
    private List<ItemStack> subItems = new List<ItemStack>();

#if UNITY_EDITOR
    private void OnValidate()//인스펙터 창에서 속성값 수정시 호출
    {
        mainSlots = mainSlotParent.GetComponentsInChildren<Inven_Slot>();
        subSlots = subSlotParent.GetComponentsInChildren<Inven_Slot>();
    }
#endif

    void Awake()
    {
        FreshSlot();
    }
    private void Update()
    {
        UseSubItem();
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("다이아몬드 지급됨"); 
            ItemData item = FindItemByName("sword");
            if (item != null)
            {
                AddItem(item);
                Debug.Log("다이아몬드 지급됨");
            }
            else
            {
                Debug.LogWarning("아이템 'diamond' 를 찾을 수 없습니다.");
            }
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
        // 1. 먼저 main에 넣을 수 있는지 확인
        if (TryAddToList(mainItems, mainSlots.Length, item)) return;

        // 2. main이 꽉 찼으면 sub에 넣기 시도
        if (TryAddToList(subItems, subSlots.Length, item)) return;

        Debug.Log("인벤토리 공간이 부족합니다.");
    }
    private bool TryAddToList(List<ItemStack> list, int slotLimit, ItemData item)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var stack = list[i];
            if (stack != null && stack.itemData.itemName == item.itemName && !stack.IsFull)
            {
                stack.count++;
                FreshSlot();
                return true;
            }
        }

        // 빈 칸 먼저 찾기
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
            {
                list[i] = new ItemStack(item);
                FreshSlot();
                return true;
            }
        }

        if (list.Count < slotLimit)
        {
            list.Add(new ItemStack(item));
            FreshSlot();
            return true;
        }

        return false;
    }
    public void DelItem(string itemName)
    {
        // mainInven에서 먼저 찾기
        if (TryRemoveFromList(mainItems, itemName)) return;

        // 못 찾으면 subInven에서 찾기
        if (TryRemoveFromList(subItems, itemName)) return;

        Debug.LogWarning("삭제하려는 아이템이 인벤토리에 없습니다: " + itemName);
    }
    private bool TryRemoveFromList(List<ItemStack> list, string itemName)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null && list[i].itemData.itemName == itemName)
            {
                list[i].count--;
                if (list[i].count <= 0)
                    list[i] = null;

                FreshSlot();
                return true;
            }
        }
        return false;
    }

    private void UseSubItem()//1,2,3,4번키로 아이템 사용
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i >= subItems.Count)
                {
                    Debug.Log($"Sub 인벤 {i + 1} 칸에 아이템이 없습니다.");
                    return;
                }

                var item = subItems[i].itemData;
                Debug.Log($"아이템 사용됨 (서브 인벤 {i + 1}): {item.itemName}");

                // 실제 사용 로직은 여기에

                DelSubItemByIndex(i); // SubInven에서만 삭제
                return;
            }
        }
    }
    private void DelSubItemByIndex(int index)
    {
        if (index < 0 || index >= subItems.Count)
            return;

        if (subItems[index] == null) return;

        subItems[index].count--;

        if (subItems[index].count <= 0)
            subItems[index] = null;

        FreshSlot();
    }
    private ItemData FindItemByName(string name)//리소스 폴더에서 해당하는 아이템 체크
    {
        ItemData[] allItems = Resources.LoadAll<ItemData>("");
        foreach (var item in allItems)
        {
            if (item.itemName == name)
                return item;
        }
        return null;
    }
}
