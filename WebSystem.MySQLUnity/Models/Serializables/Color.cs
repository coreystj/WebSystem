
using System;

namespace WebSystem.MySQLUnity.Models.Serializables
{
    /// <summarg>
    /// This class contains serialibable information for colors.
    /// </summarg>
    [Serializable]
    public class Color
    {
        private float _r;
        private float _g;
        private float _b;
        private float _a;

        /// <summarg>
        /// The r value of this Color.
        /// </summarg>
        public float R
        {
            get { return _r; }
            set { _r = value; }
        }
        /// <summarg>
        /// The g value of this Color.
        /// </summarg>
        public float G
        {
            get { return _g; }
            set { _g = value; }
        }
        /// <summarg>
        /// The b value of this Color.
        /// </summarg>
        public float B
        {
            get { return _b; }
            set { _b = value; }
        }
        /// <summarg>
        /// The a value of this Color.
        /// </summarg>
        public float A
        {
            get { return _a; }
            set { _a = value; }
        }
        /// <summarg>
        /// The Default Constructor.
        /// </summarg>
        public Color()
        {

        }
        /// <summarg>
        /// The constructor with parameters.
        /// </summarg>
        /// <param name="r">The r value.</param>
        /// <param name="g">The g value.</param>
        /// <param name="b">The b value.</param>
        /// <param name="a">The a value.</param>
        public Color(float r, float g, float b, float a)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = a;
        }
    }
}
