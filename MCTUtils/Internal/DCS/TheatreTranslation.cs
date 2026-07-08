using System;
using System.Collections.Generic;
using System.Text;

namespace MCTUtils.Internal.DCS
{
    public partial class TheatreTranslation
    {
        public int Central_meridian { get; set; } = 0;
        public double False_easting { get; set; } = 0;
        public double False_northing { get; set; } = 0;
        public double Scale_factor { get; set; } = 0;
    }
}
