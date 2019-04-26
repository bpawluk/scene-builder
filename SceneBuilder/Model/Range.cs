using JeremyAnsel.DirectX.DXMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneBuilder.Model
{
    class Range3D
    {
        public XMVector MinX { get; set; } = new XMVector(float.MaxValue, float.MaxValue, float.MaxValue, 1);
        public XMVector MinY { get; set; } = new XMVector(float.MaxValue, float.MaxValue, float.MaxValue, 1);
        public XMVector MinZ { get; set; } = new XMVector(float.MaxValue, float.MaxValue, float.MaxValue, 1);
        public XMVector MaxX { get; set; } = new XMVector(float.MinValue, float.MinValue, float.MinValue, 1);
        public XMVector MaxY { get; set; } = new XMVector(float.MinValue, float.MinValue, float.MinValue, 1);
        public XMVector MaxZ { get; set; } = new XMVector(float.MinValue, float.MinValue, float.MinValue, 1);


        public void evaluate(XMVector p)
        {
            if (p.X < MinX.X) MinX = new XMVector(p);
            if (p.Y < MinY.Y) MinY = new XMVector(p);
            if (p.Z < MinZ.Z) MinZ = new XMVector(p);
            if (p.X > MaxX.X) MaxX = new XMVector(p);
            if (p.Y > MaxY.Y) MaxY = new XMVector(p);
            if (p.Z > MaxZ.Z) MaxZ = new XMVector(p);
        }
    }
}