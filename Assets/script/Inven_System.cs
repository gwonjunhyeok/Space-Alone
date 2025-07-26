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
            if (i < mainItems.Count)
                mainSlots[i].SetItem(mainItems[i].itemData, mainItems[i].count);
            else
                mainSlots[i].ClearSlot();
        }

        for (int i = 0; i < subSlots.Length; i++)
        {
            if (i < subItems.Count)
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
        foreach (var stack in list)
        {
            if (stack.itemData.itemName == item.itemName)
            {
                if (!stack.IsFull)
                {
                    stack.count++;
                    FreshSlot();
                    return true;
                }
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
