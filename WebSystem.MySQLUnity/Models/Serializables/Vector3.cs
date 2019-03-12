using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WebSystem.MySQLUnity.Models.Serializables
{
    /// <summary>
    /// A serializable vector2 class.
    /// </summary>
    [Serializable]
    public class Vector3
    {
        private float _x;
        private float _y;
        private float _z;

        /// <summary>
        /// The x value ofthis vector2.
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// The y value ofthis vector2.
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// The y value ofthis vector2.
        /// </summary>
        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }

        /// <summary>
        /// The default constructor.
        /// </summary>
        public Vector3()
        {

        }

        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
    }
}
