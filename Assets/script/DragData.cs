public class DragData
{
    public bool fromMain;               // true�� mainItems, false�� subItems
    public int originIndex;             // �巡�� ���� ���� �ε���
    public ItemStack draggedItem;       // �巡�� ���� ������ ����
    public int draggedCount;            // �巡�� ���� ����
    public bool isSplit;                // Ctrl�� ���ݸ� �巡���ߴ��� ����
}