using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MagicLeap.Viewer3D
{
    public enum LayoutType
    {
        Vertical,
        Horizontal
    }

    [ExecuteInEditMode]
    public class ResponsiveLayout : MonoBehaviour
    {
        [SerializeField]
        private List<ResponsiveElement> _responsiveElements;
        [SerializeField]
        private RectTransform _referenceViewport;
        [SerializeField]
        private HorizontalOrVerticalLayoutGroup _layoutGroup;

        private float _spacing;
        private float _availableWidth;
        private float _availableHeight;
        private LayoutType _layoutType;

        private void Awake()
        {
#if UNITY_EDITOR
            SceneManager.activeSceneChanged += (scene1, scene2) =>
            {
                OnEnable();
            };

            SceneManager.sceneLoaded += (scene1, mode) =>
            {
                OnEnable();
            };
#endif
        }

        private void OnEnable()
        {
            StopAllCoroutines();

            StartCoroutine(DelayedCall());
        }

        private IEnumerator DelayedCall()
        {
            yield return new WaitForSeconds(0.5f);
            RefreshLayout();
        }

        public void AddNewElement(ResponsiveElement element)
        {
            if (_responsiveElements == null)
            {
                _responsiveElements = new List<ResponsiveElement>();
            }

            _responsiveElements.Add(element);
        }

        public void RefreshLayout()
        {
            if (_referenceViewport == null)
            {
                return;
            }

            if (_responsiveElements == null || _responsiveElements.Count < 1)
            {
                return;
            }

            _availableWidth = _referenceViewport.rect.width;
            _availableHeight = _referenceViewport.rect.height;

            HorizontalLayoutGroup horizontalLayout = _layoutGroup.GetComponent<HorizontalLayoutGroup>();
            VerticalLayoutGroup verticalLayout = _layoutGroup.GetComponent<VerticalLayoutGroup>();

            int count = _responsiveElements != null ? _responsiveElements.Count : 0;

            if (horizontalLayout != null)
            {
                _spacing = horizontalLayout.spacing;
                _availableWidth -= Mathf.Max(0, (count - 1)) * _spacing;
                _availableWidth -= (horizontalLayout.padding.left + horizontalLayout.padding.right);

                _availableHeight -= horizontalLayout.padding.top + horizontalLayout.padding.bottom;

                _layoutType = LayoutType.Horizontal;
            }
            else if (verticalLayout != null)
            {
                _spacing = verticalLayout.spacing;
                _availableHeight -= Mathf.Max(0, (count - 1)) * _spacing;
                _availableHeight -= (verticalLayout.padding.top + verticalLayout.padding.bottom);

                _availableWidth -= verticalLayout.padding.left + verticalLayout.padding.right;

                _layoutType = LayoutType.Vertical;
            }

            float contentWidthPercentage = 0;
            float contentHeightPercentage = 0;

            foreach (ResponsiveElement element in _responsiveElements)
            {
                if (element == null
                    || (!element.gameObject.activeInHierarchy
                        && !element.AllocateOnInactive))
                {
                    continue;
                }

                element.Init();

                if (element.LayoutElement == null || element.UseAsFiller)
                {
                    continue;
                }

                if (element.SquareDimensions)
                {
                    if (_layoutType == LayoutType.Horizontal && element.AdjustHeight)
                    {
                        contentWidthPercentage += element.HeightPercentage * _availableHeight / _availableWidth;
                    }
                    else if (_layoutType == LayoutType.Vertical && element.AdjustWidth)
                    {
                        contentHeightPercentage += element.WidthPercentage * _availableWidth / _availableHeight;
                    }
                    else
                    {
                        contentWidthPercentage += element.WidthPercentage;
                        contentHeightPercentage += element.HeightPercentage;
                    }
                }
                else
                {
                    contentWidthPercentage += element.WidthPercentage;
                    contentHeightPercentage += element.HeightPercentage;
                }

                element.RefreshDimensions(_availableWidth, _availableHeight);
            }

            contentWidthPercentage = Mathf.Clamp01(contentWidthPercentage);
            contentHeightPercentage = Mathf.Clamp01(contentHeightPercentage);

            foreach (ResponsiveElement element in _responsiveElements)
            {
                if (element == null
                    || !element.UseAsFiller
                    || !element.gameObject.activeInHierarchy)
                {
                    continue;
                }

                element.SetPreferredHeight((1 - contentHeightPercentage) * _availableHeight);
                element.SetPreferredWidth((1 - contentWidthPercentage) * _availableWidth);
            }
        }
    }
}
