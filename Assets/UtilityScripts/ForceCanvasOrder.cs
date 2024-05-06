using System;
using UnityEngine;

namespace CustomUtilityScripts
{
    [Serializable]
    public enum CanvasOrder
    {
        None = 0,
        LandingPage = 6,
        DraggableThumbnail = 7,
        PopupDialog,
        ModelSpaceUI,
        SettingsDropdown
    }

    [RequireComponent(typeof(Canvas))]
    public class ForceCanvasOrder : MonoBehaviour
    {
        [SerializeField]
        private CanvasOrder _forcedSortingOrder;

        private int _defaultCanvasOrder = 0;

        [SerializeField]
        private bool _forcePermanently;

        private Canvas _target;

        private void Awake()
        {
            _target = GetComponent<Canvas>();
            _defaultCanvasOrder = _target.sortingOrder;
        }

        private void OnEnable()
        {
            ToggleCanvasOrder(true);
        }

        private void OnDisable()
        {
            ToggleCanvasOrder(false);
        }

        /// <summary>
        /// Switches between default and forced applied order
        /// </summary>
        /// <param name="applyForced"></param>
        public void ToggleCanvasOrder(bool applyForced = false)
        {
            if (_target == null)
            {
                return;
            }

            if (_forcePermanently || applyForced)
            {
                _target.overrideSorting = true;
                _target.sortingOrder = (int)_forcedSortingOrder;
                return;
            }

            _target.overrideSorting = false;
            _target.sortingOrder = _defaultCanvasOrder;
        }

        public void SetOrder(CanvasOrder order, bool applyPermanently)
        {
            _forcedSortingOrder = order;
            _forcePermanently = applyPermanently;
            ToggleCanvasOrder(true);
        }
    }
}
