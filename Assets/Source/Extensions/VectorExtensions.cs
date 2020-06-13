using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Extensions
{
    public static class VectorExtensions
    {
        /// <summary>
        /// Returns a new Vector3 instance with either this vector3's x, y, and z values or the ones specified in the parameters
        /// <param name="vector3">the original vector3 object</param>
        /// <param name="x">A new X value</param>
        /// <param name="y">A new Y value</param>
        /// <param name="z">A new Z value</param>
        /// <returns>The new Vector3 instance</returns>
        public static Vector3 Copy(this Vector3 vector3, float? x = null, float? y = null, float? z = null) {
            return new Vector3 (
                x: x ?? vector3.x,
                y: y ?? vector3.y,
                z: z ?? vector3.z
            );
        }

        /// <summary>
        /// Returns a new Vector2 instance with either this vector2's x and y values or the ones specified in the parameters
        /// The values are optional, and if left out, will remain as the original value
        /// </summary>
        /// <param name="vector2">the original vector2 object</param>
        /// <param name="x">A new X value</param>
        /// <param name="y">A new Y value</param>
        /// <returns>The new Vector2 instance</returns>
        public static Vector2 Copy(this Vector2 vector2, float? x = null, float? y = null)
        {
            return new Vector2(
                x: x ?? vector2.x,
                y: y ?? vector2.y
            );
        }

        /// <summary>
        /// Returns a new Vector2 instance with either this vector3's x and y values or the ones specified
        /// </summary>
        /// <param name="vector3">the original vector3 object</param>
        /// <param name="x">A new X value</param>
        /// <param name="y">A new Y value</param>
        /// <returns>A new Vector2 instance</returns>
        public static Vector2 ToVector2(this Vector3 vector3, float? x = null, float? y = null) {
            return new Vector2 (
                x: x ?? vector3.x,
                y: y ?? vector3.y
            );
        }

        /// <summary>
        /// Returns a new Vector3 instance with either this vector3's x and y values or the ones specified.  If z is
        /// not specified it will default to zero.
        /// </summary>
        /// <param name="vector3">the original vector3 object</param>
        /// <param name="x">A new X value</param>
        /// <param name="y">A new Y value</param>
        /// <param name="z">A new Z value</param>   
        /// <returns>A new Vector3 instance</returns>
        public static Vector3 ToVector3(this Vector2 vector2, float? x = null, float? y = null, float? z = null) {
            return new Vector3(
                x: x ?? vector2.x,
                y: y ?? vector2.y,
                z: z ?? 0
            );
        }

        /// <summary>
        /// Returns true if <paramref name="otherPosition"/> is within a square of size <paramref name="boundarySize"/>
        /// of <paramref name="thisPosition"/>
        /// </summary>
        public static bool IsWithin(this Vector2 thisPosition, Vector2 boundarySize, Vector2 otherPosition) 
        {
            // is x < otherposition-boundaries.x or x > otherposition+boundaries.x
            if (!thisPosition.x.IsWithin(boundarySize.x, otherPosition.x)) {
                return false;
            }

            if (!thisPosition.y.IsWithin(boundarySize.y, otherPosition.y)) {
                return false;
            }

            return true;        
        }
    }
}
