using System;
using System.Collections.Generic;
using System.Text;

namespace MCTUtils.Internal.Geo
{
    /// <summary>
    /// Represents a basic geographical coordinate with latitude and longitude.
    /// </summary>
    public interface IBasicCoordinate
    {
        public double Latitidue { get; set; }
        public double Longtitude { get; set; }
    }

    /// <summary>
    /// Represents a basic geographical coordinate with latitude and longitude.
    /// </summary>

    public partial class BasicCoordinate : IBasicCoordinate
    {
        /// <summary>
        /// Gets or sets the latitude of the coordinate.
        /// </summary>
        public double Latitidue { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the coordinate.
        /// </summary>
        public double Longtitude { get; set; }
    }
}
