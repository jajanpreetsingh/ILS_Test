using UnityEngine;
using UnityEngine.UI;

namespace MagicLeap.Viewer3D
{
    [RequireComponent(typeof(LayoutElement))]
    public class ResponsiveElement : MonoBehaviour
    {
        private LayoutElement _layoutElement;

        public LayoutElement LayoutElement => _layoutElement;

        public bool SquareDimensions = false;
        public bool AllocateOnInactive = false;

        public bool UseAsFiller;

        [Header("Width Settings")]
        public bool AdjustWidth;
        [Range(0.0f, 1.0f)]
        public float WidthPercentage;

        [Header("Height Settings")]
        public bool AdjustHeight;
        [Range(0.0f, 1.0f)]
        public float HeightPercentage;

        public void Init()
        {
            _layoutElement = GetComponent<LayoutElement>();
        }

        public void SetPreferredWidth(float preferredWidth)
        {
            LayoutElement.preferredWidth = preferredWidth;
            LayoutElement.flexibleWidth = 0;
        }

        public void SetPreferredHeight(float preferredHeight)
        {
            LayoutElement.preferredHeight = preferredHeight;
            LayoutElement.flexibleHeight = 0;
        }

        public void RefreshDimensions(float availableWidth, float availableHeight)
        {
            if (LayoutElement == null)
            {
                return;
            }

            if (AdjustHeight)
            {
                SetPreferredHeight(HeightPercentage * availableHeight);

                if (SquareDimensions)
                {
                    SetPreferredWidth(LayoutElement.preferredHeight);
                    return;
                }
            }

            if (AdjustWidth)
            {
                SetPreferredWidth(WidthPercentage * availableWidth);

                if (SquareDimensions)
                {
                    SetPreferredHeight(LayoutElement.preferredWidth);
                    return;
                }
            }
        }
    }
}