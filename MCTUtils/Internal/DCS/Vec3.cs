using System;
using System.Collections.Generic;
using System.Text;

namespace MCTUtils.Internal.DCS
{
    /// <summary>
    /// Represents a 3D vector with X, Y, and Z coordinates.
    /// </summary>
    public interface IVec3 : IVec2
    {
        /// <summary>
        /// Gets or sets the Z coordinate of the vector.
        /// </summary>
        public double Z { get; set; }
    }


    /// <summary>
    /// Represents a 3D vector with X, Y, and Z coordinates.
    /// </summary>
    public partial class Vec3 : Vec2, IVec3
    {
        /// <summary>
        /// Gets or sets the Z coordinate of the vector.
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vec3"/> class with default values (0, 0, 0).
        /// </summary>
        public Vec3() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vec3"/> class with provided X, Y, and Z coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vec3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Vec3"/> class with specified X, Y, and Z coordinates.
        /// </summary>
        /// <param name="DCSVec2"></param>
        public Vec3(Vec2 DCSVec2)
        {
            X = DCSVec2.X;
            Y = DCSVec2.Y;
            Z = 0;
        }
    }
}
