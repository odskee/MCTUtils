using System;
using System.Collections.Generic;
using System.Text;

namespace MCTUtils.Internal.DCS
{
    /// <summary>
    /// Represents the translation parameters for a theatre in DCS (Digital Combat Simulator).
    /// </summary>
    public partial class TheatreTranslation
    {
        /// <summary>
        /// Gets or sets the central meridian for the theatre translation.
        /// </summary>
        public int Central_meridian { get; set; } = 0;

        /// <summary>
        /// Gets or sets the latitude of origin for the theatre translation.
        /// </summary>
        public double False_easting { get; set; } = 0;

        /// <summary>
        /// Gets or sets the false northing for the theatre translation.
        /// </summary>
        public double False_northing { get; set; } = 0;

        /// <summary>
        /// Gets or sets the scale factor for the theatre translation.
        /// </summary>
        public double Scale_factor { get; set; } = 0;
    }
}
