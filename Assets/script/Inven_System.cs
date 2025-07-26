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
    private void OnValidate()//�ν����� â���� �Ӽ��� ������ ȣ��
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
            Debug.Log("���̾Ƹ�� ���޵�"); 
            ItemData item = FindItemByName("sword");
            if (item != null)
            {
                AddItem(item);
                Debug.Log("���̾Ƹ�� ���޵�");
            }
            else
            {
                Debug.LogWarning("������ 'diamond' �� ã�� �� �����ϴ�.");
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
        // 1. ���� main�� ���� �� �ִ��� Ȯ��
        if (TryAddToList(mainItems, mainSlots.Length, item)) return;

        // 2. main�� �� á���� sub�� �ֱ� �õ�
        if (TryAddToList(subItems, subSlots.Length, item)) return;

        Debug.Log("�κ��丮 ������ �����մϴ�.");
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
    private ItemData FindItemByName(string name)//���ҽ� �������� �ش��ϴ� ������ üũ
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
