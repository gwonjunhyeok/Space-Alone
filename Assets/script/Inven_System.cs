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
        UseSubItem();
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
        // 1. ���� main�� ���� �� �ִ��� Ȯ��
        if (TryAddToList(mainItems, mainSlots.Length, item)) return;

        // 2. main�� �� á���� sub�� �ֱ� �õ�
        if (TryAddToList(subItems, subSlots.Length, item)) return;

        Debug.Log("�κ��丮 ������ �����մϴ�.");
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

        // �� ĭ ���� ã��
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
        // mainInven���� ���� ã��
        if (TryRemoveFromList(mainItems, itemName)) return;

        // �� ã���� subInven���� ã��
        if (TryRemoveFromList(subItems, itemName)) return;

        Debug.LogWarning("�����Ϸ��� �������� �κ��丮�� �����ϴ�: " + itemName);
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

    private void UseSubItem()//1,2,3,4��Ű�� ������ ���
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i >= subItems.Count)
                {
                    Debug.Log($"Sub �κ� {i + 1} ĭ�� �������� �����ϴ�.");
                    return;
                }

                var item = subItems[i].itemData;
                Debug.Log($"������ ���� (���� �κ� {i + 1}): {item.itemName}");

                // ���� ��� ������ ���⿡

                DelSubItemByIndex(i); // SubInven������ ����
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
