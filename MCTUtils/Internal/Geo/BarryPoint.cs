using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTUtils.Internal.Geo
{
    /// <summary>
    /// Represents a point in barycentric coordinates, which are used to express the position of a point within a triangle as a combination of the triangle's vertices.
    /// </summary>
    public class BarryPoint
    {
        /// <summary>
        /// Gets or sets the barycentric coordinate corresponding to vertex A of the triangle.
        /// </summary>
        public float wA { get; set; }

        
        /// <summary>
        /// Gets or sets the barycentric coordinate corresponding to vertex B of the triangle.
        /// </summary>
        public float wB { get; set; }

        
        /// <summary>
        /// Gets or sets the barycentric coordinate corresponding to vertex C of the triangle.
        /// </summary>
        public float wC { get; set; }
    }
}
