using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inven_Slot : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI countText;

    public void SetItem(ItemData data, int count)
    {
        if (data != null)
        {
            image.sprite = data.icon;
            //image.color = new Color(1, 1, 1, 1);
            countText.text = count > 1 ? count.ToString() : "";
        }
    }

    public void ClearSlot()
    {
        image.sprite = null;
        //image.color = new Color(1, 1, 1, 0);
        countText.text = "";
    }
}
