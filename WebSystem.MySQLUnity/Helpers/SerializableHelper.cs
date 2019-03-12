using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace WebSystem.MySQLUnity.Helpers
{
    /// <summary>
    /// Serializable helper class for converting standard classes to serializable instances.
    /// </summary>
    public static class SerializableHelper
    {
        /// <summary>
        /// Converts this istance to a serializable instance.
        /// </summary>
        /// <param name="value">This instance.</param>
        /// <returns>Returns the serializaed instance.</returns>
        public static Models.Serializables.Quaternion ToSerializable(this Quaternion value)
        {
            return new Models.Serializables.Quaternion(value.x, value.y, value.z, value.w);
        }

        /// <summary>
        /// Converts a serialized object to a standard instance.
        /// </summary>
        /// <param name="value">The serialized instance.</param>
        /// <returns>Returns the standard instance.</returns>
        public static Quaternion ToStandard(this Models.Serializables.Quaternion value)
        {
            return new Quaternion(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Converts this istance to a serializable instance.
        /// </summary>
        /// <param name="value">This instance.</param>
        /// <returns>Returns the serializaed instance.</returns>
        public static Models.Serializables.Vector3 ToSerializable(this Vector3 value)
        {
            return new Models.Serializables.Vector3(value.x, value.y, value.z);
        }

        /// <summary>
        /// Converts a serialized object to a standard instance.
        /// </summary>
        /// <param name="value">The serialized instance.</param>
        /// <returns>Returns the standard instance.</returns>
        public static Vector3 ToStandard(this Models.Serializables.Vector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Converts this istance to a serializable instance.
        /// </summary>
        /// <param name="value">This instance.</param>
        /// <returns>Returns the serializaed instance.</returns>
        public static Models.Serializables.Vector2 ToSerializable(this Vector2 value)
        {
            return new Models.Serializables.Vector2(value.x, value.y);
        }

        /// <summary>
        /// Converts a serialized object to a standard instance.
        /// </summary>
        /// <param name="value">The serialized instance.</param>
        /// <returns>Returns the standard instance.</returns>
        public static Vector2 ToStandard(this Models.Serializables.Vector2 value)
        {
            return new Vector2(value.X, value.Y);
        }

        /// <summary>
        /// Converts this istance to a serializable instance.
        /// </summary>
        /// <param name="value">This instance.</param>
        /// <returns>Returns the serializaed instance.</returns>
        public static Models.Serializables.Color ToSerializable(this Color value)
        {
            return new Models.Serializables.Color(value.r, value.g, value.b, value.a);
        }

        /// <summary>
        /// Converts a serialized object to a standard instance.
        /// </summary>
        /// <param name="value">The serialized instance.</param>
        /// <returns>Returns the standard instance.</returns>
        public static Color ToStandard(this Models.Serializables.Color value)
        {
            return new Color(value.R, value.G, value.B, value.A);
        }

        /// <summary>
        /// Converts a hex to a color value.
        /// </summary>
        /// <param name="hex">The string hex to be parsed.</param>
        /// <returns>Returns the newly created color.</returns>
        public static Color ToColor(this string hex)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#"+hex, out color))
                return color;
            else
                return Color.magenta;
        }

        /// <summary>
        /// Converts a color to a hex value.
        /// </summary>
        /// <param name="color">The color to be converted.</param>
        /// <returns>Returns the newly created hex string.</returns>
        public static string ToHex(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
    }
}
