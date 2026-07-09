using System;
using System.Collections.Generic;
using System.Text;

namespace MCTUtils.Internal.DCS
{
    /// <summary>
    /// Represents a 2D vector with X and Y coordinates.
    /// </summary>
    public interface IVec2
    {
        /// <summary>
        /// Gets or sets the X coordinate of the vector.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the vector.
        /// </summary>
        public double Y { get; set; }
    }


    /// <summary>
    /// Represents a 2D vector with X and Y coordinates.
    /// </summary>
    public partial class Vec2 : IVec2
    {
        /// <summary>
        /// Gets or sets the X coordinate of the vector.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the vector.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vec2"/> class with default values (0, 0).
        /// </summary>
        public Vec2() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vec2(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vec2"/> class with specified X and Y coordinates.
        /// </summary>
        /// <param name="DCSVec3"></param>
        public Vec2(Vec3 DCSVec3) 
        {
            X = DCSVec3.X;
            Y = DCSVec3.Y;
        }
    }
}
