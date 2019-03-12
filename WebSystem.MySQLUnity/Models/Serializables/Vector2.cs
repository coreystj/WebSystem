using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSystem.MySQLUnity.Models.Serializables
{
    /// <summary>
    /// A serializable vector2 class.
    /// </summary>
    [Serializable]
    public class Vector2
    {
        private float _x;
        private float _y;

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
        /// The default constructor.
        /// </summary>
        public Vector2()
        {

        }

        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }
    }
}
