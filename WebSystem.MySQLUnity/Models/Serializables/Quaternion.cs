using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSystem.MySQLUnity.Models.Serializables
{
    /// <summary>
    /// This class contains serializable information for quaternions.
    /// </summary>
    [Serializable]
    public class Quaternion
    {
        private float _x;
        private float _y;
        private float _z;
        private float _w;

        /// <summary>
        /// The x value of this quaternion.
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        /// <summary>
        /// The y value of this quaternion.
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        /// <summary>
        /// The z value of this quaternion.
        /// </summary>
        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }
        /// <summary>
        /// The w value of this quaternion.
        /// </summary>
        public float W
        {
            get { return _w; }
            set { _w = value; }
        }
        /// <summary>
        /// The Default Constructor.
        /// </summary>
        public Quaternion()
        {

        }
        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        /// <param name="w">The w value.</param>
        public Quaternion(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }
    }
}
