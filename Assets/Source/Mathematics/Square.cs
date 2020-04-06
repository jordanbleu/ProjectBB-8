using System;
using UnityEngine;

namespace Assets.Source.Mathematics
{
    /// <summary>
    /// Represents a square in 2d space
    /// </summary>
    [Serializable]
    public struct Square
    {
        public Square(float width, float height) {
            Width = width;
            Height = height;
        }

        public float Width;
        public float Height;

        public Vector2 TopLeft => new Vector2(-Width, Height);

        public Vector2 TopRight => new Vector2(Width, Height);

        public Vector2 BottomLeft => new Vector2(-Width, -Height);

        public Vector2 BottomRight => new Vector2(Width, -Height);

        public override bool Equals(object obj)
        {
            if (obj is Square other)
            {
                return (Width == other.Width && Height == other.Height);            
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 859600377;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Square left, Square right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Square left, Square right)
        {
            return !(left == right);
        }
    }
}

