using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JeremyAnsel.DirectX.DXMath;
using SceneBuilder.Model;

namespace SceneBuilder
{
    class SceneLoader
    {
        private int _pointsCount = -1;
        private int _currentPoint = 0;
        private int _trianglesCount = -1;
        private int _currentTriangle = 0;
        public Range3D Range { get; set; }
        public XMVector[] Points { get; set; }
        public Triangle3D[] Triangles { get; set; }

        public void LoadScene(string filePath)
        {
            Range = new Range3D();
            var lines = File.ReadLines(filePath);
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line) && !line.StartsWith(@"//")) ParseLine(line);
            }
            return;
        }

        private void ParseLine(string line)
        {
            var elements = line.Split(' ');
            if (_pointsCount < 0)
            {
                try
                {
                    _pointsCount = int.Parse(elements[0]);
                    Points = new XMVector[_pointsCount];
                }
                catch { }
            }
            else if(_currentPoint < _pointsCount)
            {
                if(elements.Length >= 3) AddPoint(elements[0], elements[1], elements[2]);
            }
            else if (_trianglesCount < 0)
            {
                try
                {
                    _trianglesCount = int.Parse(elements[0]);
                    Triangles = new Triangle3D[_trianglesCount];
                }
                catch { }
            }
            else if(_currentTriangle < _trianglesCount)
            {
                if (elements.Length >= 3) AddTriangle(elements[0], elements[1], elements[2]);
            }
        }

        private void AddPoint(string a, string b, string c)
        {
            XMVector temp = new XMVector(float.NaN, float.NaN, float.NaN, 1);
            try
            {
                temp.X = float.Parse(a, CultureInfo.InvariantCulture);
                temp.Y = float.Parse(b, CultureInfo.InvariantCulture);
                temp.Z = float.Parse(c, CultureInfo.InvariantCulture);
            }
            catch { }
            if (!float.IsNaN(temp.X) && !float.IsNaN(temp.Y) && !float.IsNaN(temp.Z))
            {
                Points[_currentPoint] = temp;
                _currentPoint++;
                Range.evaluate(temp);
            }
        }

        private void AddTriangle(string a, string b, string c)
        {
            Triangle3D temp = new Triangle3D { Points = new int[] { -1, -1, -1 } };
            try
            {
                temp.Points[0] = int.Parse(a);
                temp.Points[1] = int.Parse(b);
                temp.Points[2] = int.Parse(c);
            }
            catch { }
            if (temp.IsValid())
            {
                Triangles[_currentTriangle] = temp;
                _currentTriangle++;
            }
        }
    }
}
