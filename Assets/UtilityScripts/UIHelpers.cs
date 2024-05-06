using UnityEngine;
using static CustomUtilityScripts.Frustum;

namespace CustomUtilityScripts
{
    public static class UIHelpers
    {
        public static bool ContainsPoint(this RectTransform caller, Vector3 worldPosition)
        {
            Vector3[] localCorners = new Vector3[4];

            caller.GetLocalCorners(localCorners);

            Plane rectPlane = new(-caller.transform.forward, caller.transform.position);

            Vector3 pointOnPlane = rectPlane.ClosestPointOnPlane(worldPosition);
            Vector3 localPoint = caller.transform.InverseTransformPoint(pointOnPlane);

            localPoint.z = 0;

            bool insideRect = localPoint.x > localCorners[(int)Corner.TopLeft].x
                && localPoint.x < localCorners[(int)Corner.TopRight].x
                && localPoint.y < localCorners[(int)Corner.TopLeft].y
                && localPoint.y > localCorners[(int)Corner.BottomLeft].y;

            return insideRect;
        }
    }
}
