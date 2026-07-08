using System;
using System.Collections.Generic;
using System.Text;

namespace MCTUtils.Internal.DCS
{
    public interface IVec2
    {
        public double X { get; set; }
        public double Y { get; set; }
    }


    public partial class Vec2 : IVec2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vec2() { }

        public Vec2(Vec3 DCSVec3) 
        {
            X = DCSVec3.X;
            Y = DCSVec3.Y;
        }
    }
}
