using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSystem.MySQLUnity.Models.Serializables
{
    /// <summary>
    /// A serializable transform class.
    /// </summary>
    [Serializable]
    public class Transform
    {
        private Vector3 _basisX;
        private Vector3 _basisY;
        private Vector3 _basisZ;
        private Vector3 _origin;
        private double _scale;

        /// <summary>
        /// The x basis.
        /// </summary>
        public Vector3 BasisX
        {
            get { return _basisX; }
            set { _basisX = value; }
        }
        /// <summary>
        /// The y basis.
        /// </summary>
        public Vector3 BasisY
        {
            get { return _basisY; }
            set { _basisY = value; }
        }
        /// <summary>
        /// The z basis.
        /// </summary>
        public Vector3 BasisZ
        {
            get { return _basisZ; }
            set { _basisZ = value; }
        }
        /// <summary>
        /// The origin of this transform.
        /// </summary>
        public Vector3 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// The scale of this object.
        /// </summary>
        public double Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// The default constructor.
        /// </summary>
        public Transform()
        {

        }

        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="basisX">The x basis.</param>
        /// <param name="basisY">The y basis.</param>
        /// <param name="basisZ">The z basis.</param>
        /// <param name="origin">The origin of this transform.</param>
        /// <param name="scale">The scale of this transform.</param>
        public Transform(Vector3 basisX, Vector3 basisY, Vector3 basisZ, Vector3 origin, double scale)
        {
            _basisX = basisX;
            _basisY = basisY;
            _basisZ = basisZ;
            _origin = origin;
            _scale = scale;
        }
    }
}
