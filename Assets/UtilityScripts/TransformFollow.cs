using System.Collections;
using UnityEngine;

namespace CustomUtilityScripts
{
    public class TransformFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        private Coroutine _followRoutine;

        private void OnEnable()
        {
            OnSetTarget();
        }

        private void OnSetTarget()
        {
            if (_target == null)
            {
                return;
            }
        }

        private IEnumerator StartFollowingRoutine()
        {
            Vector3 diff = transform.position - _target.position;

            while (true)
            {
                transform.position = _target.position + diff;
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;

            OnSetTarget();
        }

        public void StartFollowing()
        {
            StartCoroutine(StartFollowingRoutine());
        }

        public void StopFollowing()
        {
            StopAllCoroutines();
        }
    }
}
