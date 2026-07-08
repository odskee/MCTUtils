using System;
using System.Collections.Generic;
using System.Text;

namespace MCTUtils.Geo
{
    public interface IBasicCoordinate
    {
        public double Latitidue { get; set; }
        public double Longtitude { get; set; }
    }

    public partial class BasicCoordinate : IBasicCoordinate
    {
        public double Latitidue { get; set; }
        public double Longtitude { get; set; }
    }
}
