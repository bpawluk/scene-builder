using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneBuilder.Model
{
    class Range3D
    {
        public Point3D MinX { get; set; } = new Point3D(double.MaxValue, double.MaxValue, double.MaxValue);
        public Point3D MinY { get; set; } = new Point3D(double.MaxValue, double.MaxValue, double.MaxValue);
        public Point3D MinZ { get; set; } = new Point3D(double.MaxValue, double.MaxValue, double.MaxValue);
        public Point3D MaxX { get; set; } = new Point3D(double.MinValue, double.MinValue, double.MinValue);
        public Point3D MaxY { get; set; } = new Point3D(double.MinValue, double.MinValue, double.MinValue);
        public Point3D MaxZ { get; set; } = new Point3D(double.MinValue, double.MinValue, double.MinValue);


        public void evaluate(Point3D p)
        {
            if (p.X < MinX.X) MinX = p;
            if (p.Y < MinY.Y) MinY = p;
            if (p.Z < MinZ.Z) MinZ = p;
            if (p.X > MaxX.X) MaxX = p;
            if (p.Y > MaxY.Y) MaxY = p;
            if (p.Z > MaxZ.Z) MaxZ = p;
        }
    }
}
