using UnityEngine;

namespace CustomUtilityScripts
{
    public class MotionUtils
    {
        /// <summary>
        /// Rotates the given UI game object to face the
        /// camera with the given transform.
        /// </summary>
        public static void FaceCamera(
            GameObject uiGameObject,
            Transform cameraTransform)
        {
            FaceTransform(uiGameObject.transform, cameraTransform);
        }

        public static void FacePoint(
            Transform target,
            in Vector3 lookAtPoint)
        {
            Vector3 objectToTarget = (target.position - lookAtPoint);

            Vector3 oldEulerAngles = target.eulerAngles;
            target.LookAt(target.position + objectToTarget.normalized);

            Vector3 newObjectEulers = target.eulerAngles;
            target.eulerAngles = newObjectEulers.SetZ(oldEulerAngles.z);
        }

        public static void FaceTransform(
            Transform target,
            Transform lookAtTransform)
        {
            FacePoint(target, lookAtTransform.position);
        }
    }
}