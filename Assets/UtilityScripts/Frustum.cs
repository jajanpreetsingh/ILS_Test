using UnityEngine;

namespace CustomUtilityScripts
{
    /// <summary>
    /// Creates a frustum object representing the camera corners at a certain depth.
    /// Use indexer to iterate through corners
    /// </summary>
    public class Frustum
    {
        public enum Corner
        {
            BottomLeft = 0,
            TopLeft = 1,
            TopRight = 2,
            BottomRight = 3
        }

        private Vector3[] _corners;

        public Frustum(float depth, 
                        Camera camera = null, 
                        Camera.MonoOrStereoscopicEye renderType = Camera.MonoOrStereoscopicEye.Mono)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }

            _corners = new Vector3[4];
            
            camera.CalculateFrustumCorners(
                camera.rect,
                depth,
                renderType,
                _corners);
        }

        public ref Vector3 this[Corner corner]
        {
            get { return ref _corners[(int)corner]; }
        }

        public float Width
        {
            get
            {
                return Vector3.Distance(this[Corner.TopLeft], this[Corner.TopRight]);
            }
        }

        public float Height
        {
            get
            {
                return Vector3.Distance(this[Corner.TopRight], this[Corner.BottomRight]);
            }
        }
    }
}
