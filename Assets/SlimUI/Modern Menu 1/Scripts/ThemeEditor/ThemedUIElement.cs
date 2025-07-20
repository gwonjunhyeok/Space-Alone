using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu
{
    [System.Serializable]
    public class ThemedUIElement : ThemedUI
    {
        [Header("Parameters")]
        Color outline;
        Image image;
        GameObject message;
        public enum OutlineStyle { solidThin, solidThick, dottedThin, dottedThick };

        public bool hasImage = false;
        public bool isText = false;

        [Header("UI 크기 고정 옵션")]
        public bool useFixedSize = false;
        public Vector2 fixedSize = new Vector2(300, 80);  // 예: 버튼 크기

        protected override void OnSkinUI()
        {
            base.OnSkinUI();

            if (hasImage)
            {
                image = GetComponent<Image>();
                image.color = themeController.currentColor;
            }

            message = gameObject;

            if (isText)
            {
                var tmp = message.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.color = themeController.textColor;
                }
            }

            if (useFixedSize)
            {
                RectTransform rt = GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.sizeDelta = fixedSize;
                }
            }
        }
    }
}
