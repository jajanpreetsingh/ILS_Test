using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomUtilityScripts
{
    public class TransformDistance
    {
        private float _sqrDistance;
        private Transform _transform;

        public float SqrDistance => _sqrDistance;
        public Transform Transform => _transform;

        public TransformDistance(Transform transform, float sqrDistance)
        {
            _transform = transform;
            _sqrDistance = sqrDistance;
        }
    }

    public static class VectorExtensions
    {
        /// <summary>
        /// Takes a vector by reference and adds the value to x component 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="val"></param>
        /// <returns>Reference to the original vector</returns>
        public static ref Vector3 AddX(this ref Vector3 vec, float val)
        {
            vec.x += val;
            return ref vec;
        }

        /// <summary>
        /// Takes a vector by reference and adds the value to y component 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="val"></param>
        /// <returns>Reference to the original vector</returns>
        public static ref Vector3 AddY(this ref Vector3 vec, float val)
        {
            vec.y += val;
            return ref vec;
        }

        /// <summary>
        /// Takes a vector by reference and adds the value to z component 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="val"></param>
        /// <returns>Reference to the original vector</returns>
        public static ref Vector3 AddZ(this ref Vector3 vec, float val)
        {
            vec.z += val;
            return ref vec;
        }

        /// <summary>
        /// Takes a vector by reference and sets the value to x component 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="val"></param>
        /// <returns>Reference to the original vector</returns>
        public static ref Vector3 SetX(this ref Vector3 vec, float val)
        {
            vec.x = val;
            return ref vec;
        }

        /// <summary>
        /// Takes a vector by reference and sets the value to y component 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="val"></param>
        /// <returns>Reference to the original vector</returns>
        public static ref Vector3 SetY(this ref Vector3 vec, float val)
        {
            vec.y = val;
            return ref vec;
        }

        /// <summary>
        /// Takes a vector by reference and sets the value to z component 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="val"></param>
        /// <returns>Reference to the original vector</returns>
        public static ref Vector3 SetZ(this ref Vector3 vec, float val)
        {
            vec.z = val;
            return ref vec;
        }

        /// <summary>
        /// Multiplies this vector by the other component-wise.
        /// </summary>
        public static ref Vector3 Multiply(this ref Vector3 vec, in Vector3 other)
        {
            vec.x *= other.x;
            vec.y *= other.y;
            vec.z *= other.z;

            return ref vec;
        }

        /// <summary>
        /// Divides this vector by the other component-wise.
        /// </summary>
        ///
        /// <param name="other">
        /// It is assumed that no components equal zero.
        /// </param>
        public static ref Vector3 Divide(this ref Vector3 vec, in Vector3 other)
        {
            vec.x /= other.x;
            vec.y /= other.y;
            vec.z /= other.z;

            return ref vec;
        }

        /// <summary>
        /// Takes a list of transforms and returns a sorted list of objects
        /// containg transform and square distance from ref point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="transforms"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<TransformDistance> SortByRelativeDistance(
            Vector3 point,
            List<Transform> transforms)
        {
            if (transforms == null || transforms.Count < 2)
            {
                throw new Exception("Transform list cannot be empty and should" +
                    " contain at least 2 elements to sort");
            }

            List<TransformDistance> result = new();

            transforms.ForEach(transform =>
            {
                result.Add(new(
                transform,
                (point - transform.position).sqrMagnitude));
            });

            result.Sort((x,y) => x.SqrDistance.CompareTo(y.SqrDistance));

            return result;
        }
    }
}