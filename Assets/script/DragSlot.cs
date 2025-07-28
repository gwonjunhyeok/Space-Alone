using UnityEngine;
using UnityEngine.UI;

public class DragSlot : Singleton<DragSlot>
{
    [SerializeField] private Image mItemImage;
    [HideInInspector] public DragData dragData;

    private void Update()
    {
        if (dragData != null && dragData.draggedItem != null)
        {
            mItemImage.transform.position = Input.mousePosition;
        }
    }

    public void StartDrag(bool fromMain, int index, ItemStack stack, bool isSplit)
    {
        dragData = new DragData
        {
            fromMain = fromMain,
            originIndex = index,
            draggedItem = stack,
            draggedCount = stack.count,
            isSplit = isSplit
        };

        // ���⼭ ������ ���� ����
        if (stack.itemData != null && stack.itemData.icon != null)
        {
            mItemImage.sprite = stack.itemData.icon;
            mItemImage.enabled = true;         //  �� Ȱ��ȭ
            SetColor(1f);
        }
        else
        {
            Debug.LogWarning($"[DragSlot] �巡���� �����ۿ� �������� ����: {stack.itemData?.itemName}");
            mItemImage.enabled = false;
        }
    }

    private void SetIconByItemName(string itemName)
    {
        Sprite icon = Resources.Load<Sprite>($"ItemIcons/{itemName}");
        if (icon != null)
        {
            mItemImage.sprite = icon;
            SetColor(1f);
        }
        else
        {
            Debug.LogWarning($"[DragSlot] Resources�� ��ġ�ϴ� ������ ����: {itemName}");
            mItemImage.sprite = null;
            SetColor(0f);
        }
    }

    public void ClearDrag()
    {
        dragData = null;
        mItemImage.sprite = null;
        SetColor(0f);
    }

    public void SetColor(float alpha)
    {
        Color c = mItemImage.color;
        c.a = alpha;
        mItemImage.color = c;
    }
}
