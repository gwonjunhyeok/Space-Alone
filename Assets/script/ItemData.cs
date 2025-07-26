using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject//��ũ���ͺ������Ʈ�� �����۵����͸� �������� ���� �߰�
{
    public string itemName;       // ������ �̸�
    public int maxStack;          // �ִ� ����
    public Sprite icon;           // ������ �̹��� (UI��)
}