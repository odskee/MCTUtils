using System;
using System.Collections.Generic;
using System.Text;

namespace MCTUtils.Internal.DCS
{
    public interface IVec3 : IVec2
    {
        public double Z { get; set; }
    }


    public partial class Vec3 : Vec2, IVec3
    {
        public double Z { get; set; }

        public Vec3() { }

        public Vec3(Vec2 DCSVec2)
        {
            X = DCSVec2.X;
            Y = DCSVec2.Y;
            Z = 0;
        }
    }
}
