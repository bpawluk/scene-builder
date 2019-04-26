using SceneBuilder.Model;
using JeremyAnsel.DirectX.DXMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;

namespace SceneBuilder.ViewModel
{
    class WorkspaceVM : EditableObject
    {
        private int _viewAngle = 90;
        private float _canvasHeight;
        private float _canvasWidth;
        private XMMatrix _orthographicXY;
        private XMMatrix _orthographicXZ;
        private XMMatrix _orthographicYZ;
        private XMMatrix _worldToCamera;
        private XMVector _cameraOrigin;
        private XMVector _screenCenter;
        private XMVector[] _points;
        private XMVector[] _frustum;

        public float CanvasHeight
        {
            get => _canvasHeight;
            set
            {
                _canvasHeight = value;
                OnPropertyChanged("CanvasHeight");
            }
        }
        public float CanvasWidth
        {
            get => _canvasWidth;
            set
            {
                _canvasWidth = value;
                OnPropertyChanged("CanvasWidth");
            }
        }
        public int ViewAngle
        {
            get => _viewAngle;
            set
            {
                _viewAngle = value;
                OnPropertyChanged("ViewAngle");
            }
        }
        public Triangle3D[] Triangles { get; set; }
        public XMVector[][] ClippedTriangles { get; set; }
        public XMVector[] PointsXY { get; set; }
        public XMVector[] PointsXZ { get; set; }
        public XMVector[] PointsYZ { get; set; }
        public XMMatrix OrthographicXY
        {
            get => _orthographicXY;
            set
            {
                _orthographicXY = value;
                OnPropertyChanged("OrthographicXY");
            }
        }
        public XMMatrix OrthographicXZ
        {
            get => _orthographicXZ;
            set
            {
                _orthographicXZ = value;
                OnPropertyChanged("OrthographicXZ");
            }
        }
        public XMMatrix OrthographicYZ
        {
            get => _orthographicYZ;
            set
            {
                _orthographicYZ = value;
                OnPropertyChanged("OrthographicYZ");
            }
        }
        public XMMatrix OrthographicInvXY { get; set; }
        public XMMatrix OrthographicInvXZ { get; set; }
        public XMMatrix OrthographicInvYZ { get; set; }
        public XMVector CameraOrigin
        {
            get => _cameraOrigin;
            set
            {
                _cameraOrigin = value;
                OnPropertyChanged("CameraOriginXY", "CameraOriginXZ", "CameraOriginYZ", "Perspective");
            }
        }
        public XMVector CameraOriginXY
        {
            get => TransformPoint(_cameraOrigin, OrthographicXY);
            set
            {
                var newValue = TransformPoint(value, OrthographicInvXY);
                _cameraOrigin.X = newValue.X;
                _cameraOrigin.Y = newValue.Y;
                OnPropertyChanged("CameraOriginXY", "CameraOriginXZ", "CameraOriginYZ", "Perspective");
            }
        }
        public XMVector CameraOriginXZ
        {
            get => TransformPoint(_cameraOrigin, OrthographicXZ);
            set
            {
                var newValue = TransformPoint(value, OrthographicInvXZ);
                _cameraOrigin.X = newValue.X;
                _cameraOrigin.Z = newValue.Z;
                OnPropertyChanged("CameraOriginXY", "CameraOriginXZ", "CameraOriginYZ", "Perspective");
            }
        }
        public XMVector CameraOriginYZ
        {
            get => TransformPoint(_cameraOrigin, OrthographicYZ);
            set
            {
                var newValue = TransformPoint(value, OrthographicInvYZ);
                _cameraOrigin.Y = newValue.Y;
                _cameraOrigin.Z = newValue.Z;
                OnPropertyChanged("CameraOriginXY", "CameraOriginXZ", "CameraOriginYZ", "Perspective");
            }
        }
        public XMVector ScreenCenter
        {
            get => _screenCenter;
            set
            {
                _screenCenter = value;
                OnPropertyChanged("ScreenCenterXY", "ScreenCenterXZ", "ScreenCenterYZ", "Perspective");
            }
        }
        public XMVector ScreenCenterXY
        {
            get => TransformPoint(_screenCenter, OrthographicXY);
            set
            {
                var newValue = TransformPoint(value, OrthographicInvXY);
                _screenCenter.X = newValue.X;
                _screenCenter.Y = newValue.Y;
                OnPropertyChanged("ScreenCenterXY", "ScreenCenterXZ", "ScreenCenterYZ", "Perspective");
            }
        }
        public XMVector ScreenCenterXZ
        {
            get => TransformPoint(_screenCenter, OrthographicXZ);
            set
            {
                var newValue = TransformPoint(value, OrthographicInvXZ);
                _screenCenter.X = newValue.X;
                _screenCenter.Z = newValue.Z;
                OnPropertyChanged("ScreenCenterXY", "ScreenCenterXZ", "ScreenCenterYZ", "Perspective");
            }
        }
        public XMVector ScreenCenterYZ
        {
            get => TransformPoint(_screenCenter, OrthographicYZ);
            set
            {
                var newValue = TransformPoint(value, OrthographicInvYZ);
                _screenCenter.Y = newValue.Y;
                _screenCenter.Z = newValue.Z;
                OnPropertyChanged("ScreenCenterXY", "ScreenCenterXZ", "ScreenCenterYZ", "Perspective");
            }
        }
        public XMVector[] ScreenXY
        {
            get
            {
                XMVector[] value = new XMVector[_frustum.Length];
                for (int i = 0; i < _frustum.Length; i++) value[i] = TransformPoint(_frustum[i], OrthographicXY);
                return value;
            }
        }
        public XMVector[] ScreenXZ
        {
            get
            {
                XMVector[] value = new XMVector[_frustum.Length];
                for (int i = 0; i < _frustum.Length; i++) value[i] = TransformPoint(_frustum[i], OrthographicXZ);
                return value;
            }
        }
        public XMVector[] ScreenYZ
        {
            get
            {
                XMVector[] value = new XMVector[_frustum.Length];
                for (int i = 0; i < _frustum.Length; i++) value[i] = TransformPoint(_frustum[i], OrthographicYZ);
                return value;
            }
        }

        public WorkspaceVM()
        {
            _frustum = new XMVector[12];
        }

        public void LoadCamera(XMVector cameraOrigin, XMVector screenCenter, int viewAngle)
        {
            _cameraOrigin = cameraOrigin;
            _screenCenter = screenCenter;
            _viewAngle = viewAngle;
            OnPropertyChanged("CameraOriginXY", "CameraOriginXZ", "CameraOriginYZ", 
                              "ScreenCenterXY", "ScreenCenterXZ", "ScreenCenterYZ", "ViewAngle");
        }

        public void Initialize()
        {
            var app = Application.Current as App;
            SceneLoader sl = new SceneLoader();
            sl.LoadScene(app.FilePath);

            var range = sl.Range;
            _points = sl.Points;
            Triangles = sl.Triangles;

            PointsXY = new XMVector[_points.Length];
            PointsXZ = new XMVector[_points.Length];
            PointsYZ = new XMVector[_points.Length];

            _cameraOrigin = new XMVector { X = 0, Y = 0, Z = 0, W = 1 };
            _screenCenter = new XMVector { X = 0, Y = 0, Z = -1, W = 1 };

            var dimensions = GetDimensions(range.MaxX.X - range.MinX.X, range.MaxY.Y - range.MinY.Y);
            OrthographicXY = new XMMatrix(new float[] { 2 / (range.MaxX.X - range.MinX.X), 0, 0, 0, 0, 2 / (range.MaxY.Y - range.MinY.Y), 0, 0, 0, 0, 1, 0, -(range.MaxX.X + range.MinX.X) / (range.MaxX.X - range.MinX.X), -(range.MaxY.Y + range.MinY.Y) / (range.MaxY.Y - range.MinY.Y), 0, 1 });
            OrthographicXY = XMMatrix.Multiply(OrthographicXY, new XMMatrix(new float[] { dimensions[0], 0, 0, 0, 0, -dimensions[1], 0, 0, 0, 0, 1, 0, CanvasWidth / 2, CanvasHeight / 2, 0, 1 }));

            dimensions = GetDimensions(range.MaxX.X - range.MinX.X, range.MaxZ.Z - range.MinZ.Z);
            OrthographicXZ = XMMatrix.Multiply(new XMMatrix(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1), new XMMatrix(new float[] { 2 / (range.MaxX.X - range.MinX.X), 0, 0, 0, 0, 2 / (range.MaxZ.Z - range.MinZ.Z), 0, 0, 0, 0, 1, 0, -(range.MaxX.X + range.MinX.X) / (range.MaxX.X - range.MinX.X), -(range.MaxZ.Z + range.MinZ.Z) / (range.MaxZ.Z - range.MinZ.Z), 0, 1 }));
            OrthographicXZ = XMMatrix.Multiply(OrthographicXZ, new XMMatrix(new float[] { dimensions[0], 0, 0, 0, 0, -dimensions[1], 0, 0, 0, 0, 1, 0, CanvasWidth / 2, CanvasHeight / 2, 0, 1 }));

            dimensions = GetDimensions(range.MaxZ.Z - range.MinZ.Z, range.MaxY.Y - range.MinY.Y);
            OrthographicYZ = XMMatrix.Multiply(new XMMatrix(0, 0, -1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1), new XMMatrix(new float[] { 2 / (range.MaxZ.Z - range.MinZ.Z), 0, 0, 0, 0, 2 / (range.MaxY.Y - range.MinY.Y), 0, 0, 0, 0, 1, 0, -(range.MaxZ.Z + range.MinZ.Z) / (range.MaxZ.Z - range.MinZ.Z), -(range.MaxY.Y + range.MinY.Y) / (range.MaxY.Y - range.MinY.Y), 0, 1 }));
            OrthographicYZ = XMMatrix.Multiply(OrthographicYZ, new XMMatrix(new float[] { dimensions[0], 0, 0, 0, 0, -dimensions[1], 0, 0, 0, 0, 1, 0, CanvasWidth / 2, CanvasHeight / 2, 0, 1 }));

            OrthographicInvXY = OrthographicXY.Inverse();
            OrthographicInvXZ = OrthographicXZ.Inverse();
            OrthographicInvYZ = OrthographicYZ.Inverse();

            TransformPoints(_points, PointsXY, OrthographicXY);
            TransformPoints(_points, PointsXZ, OrthographicXZ);
            TransformPoints(_points, PointsYZ, OrthographicYZ);

            OnPropertyChanged("Triangles", "CameraOriginXY", "CameraOriginXZ", "CameraOriginYZ", "ScreenCenterXY", "ScreenCenterXZ", "ScreenCenterYZ");

            PropertyChanged += Reproject;

            Project();
        }

        private XMVector TransformPoint(XMVector point, XMMatrix transform)
        {
            return XMVector3.Transform(point, transform);
        }

        private void TransformPoints(XMVector[] pointsSrc, XMVector[] pointsDst, XMMatrix transform)
        {
            for (int i = 0; i < pointsSrc.Length; i++)
            {
                pointsDst[i] = XMVector3.Transform(pointsSrc[i], transform);
            }
        }

        private void TransformPointsHomogeneus(XMVector[] pointsSrc, XMVector[] pointsDst, XMMatrix transform)
        {
            for (int i = 0; i < pointsSrc.Length; i++)
            {
                var homogeneus = XMVector3.Transform(pointsSrc[i], transform);
                pointsDst[i] = homogeneus / (homogeneus.W == 0 ? -1 : homogeneus.W);
            }
        }   

        private float[] GetDimensions(float srcWidth, float srcHeight)
        {
            float[] dimensions = new float[] { 0, 0 };
            var dstRatio = CanvasWidth / CanvasHeight;
            var srcRatio = srcWidth / srcHeight;
            if (srcRatio > dstRatio)
            {
                dimensions[0] = CanvasWidth / 2;
                dimensions[1] = CanvasWidth / (2 * srcRatio);
            }
            else
            {
                dimensions[0] = CanvasHeight * srcRatio / 2;
                dimensions[1] = CanvasHeight / 2;
            }
            return dimensions;
        }

        private void Project()
        {
            float ratio = CanvasWidth / CanvasHeight;
            float near = XMVector3.Length(_cameraOrigin - _screenCenter).X;
            float top = (float)Math.Tan(ViewAngle * 0.5 * Math.PI / 180) * near;
            float bottom = -top;
            float right = top * ratio;
            float left = -right;

            var cameraZ = XMVector3.Normalize(_cameraOrigin - _screenCenter);
            var cameraX = XMVector3.Cross(new XMVector(0, 1, 0, 1), cameraZ);
            var cameraY = XMVector3.Cross(cameraZ, cameraX);
            var cameraMatrix = new XMMatrix(new XMVector(cameraX.X, cameraX.Y, cameraX.Z, 0), new XMVector(cameraY.X, cameraY.Y, cameraY.Z, 0), new XMVector(cameraZ.X, cameraZ.Y, cameraZ.Z, 0), new XMVector(_cameraOrigin.X, _cameraOrigin.Y, _cameraOrigin.Z, 1));
            _worldToCamera = cameraMatrix.Inverse();

            DrawFrustum(left, right, top, bottom, near, cameraMatrix);

            var cameraPoints = new XMVector[_points.Length];
            TransformPoints(_points, cameraPoints, _worldToCamera);

            var nearClippedTriangles = ClipNear(cameraPoints, near);
            foreach(XMVector[] polygon in nearClippedTriangles) TransformPoints(polygon, polygon, new XMMatrix(new float[] { 2 * near / (right - left), 0, 0, 0, 0, 2 * near / (top - bottom), 0, 0, (right + left) / (right - left), (top + bottom) / (top - bottom), -1, -1, 0, 0, 0, 0 }));

            ClippedTriangles = Clip(nearClippedTriangles).ToArray();
            for (int i = 0; i < ClippedTriangles.Length; i++) TransformPoints(ClippedTriangles[i], ClippedTriangles[i], new XMMatrix(new float[] { CanvasWidth / 2, 0, 0, 0, 0, -CanvasHeight / 2, 0, 0, 0, 0, 1, 0, CanvasWidth / 2, CanvasHeight / 2, 0, 1 }));

            OnPropertyChanged("ClippedTriangles", "ScreenXY", "ScreenXZ", "ScreenYZ");
        }

        private List<XMVector[]> ClipNear(XMVector[] points, float near)
        {
            List<XMVector[]> clippedTriangles = new List<XMVector[]>();
            foreach (Triangle3D triangle in Triangles)
            {
                var inputList = new List<XMVector>(new XMVector[] { points[triangle.Points[0]], points[triangle.Points[1]], points[triangle.Points[2]] });
                var outputList = new List<XMVector>();
                XMVector s = inputList.Last();
                foreach (XMVector e in inputList)
                {
                    if (e.Z < near)
                    {
                        if (s.Z >= near) outputList.Add(ZIntersection(s, e));
                        outputList.Add(e);
                    }
                    else
                    {
                        if (s.Z < near) outputList.Add(ZIntersection(s, e));
                    }
                    s = e;
                }
                if(outputList.Count != 0)clippedTriangles.Add(outputList.ToArray());
            }
            return clippedTriangles;
        }

        private XMVector ZIntersection(XMVector s, XMVector e)
        {
            float a = (0 - s.Z) / (e.Z - s.Z);
            return new XMVector(s.X * a + e.X * (1 - a), s.Y * a + e.Y * (1 - a), s.Z * a + e.Z * (1 - a), 1);
        }

        private List<XMVector[]> Clip(List<XMVector[]> nearClippedTriangles)
        {
            List<XMVector[]> clippedTriangles = new List<XMVector[]>();
            string[] edges = new string[] { "left", "right", "top", "bottom" };
            foreach (XMVector[] polygon in nearClippedTriangles)
            {
                List<XMVector> outputList = new List<XMVector>(polygon);
                foreach (string edge in edges)
                {
                    if (outputList.Count == 0) break;
                    var inputList = new List<XMVector>(outputList);
                    outputList.Clear();
                    XMVector s = inputList.Last();
                    foreach (XMVector e in inputList)
                    {
                        if (IsInside(e, edge))
                        {
                            if (!IsInside(s, edge)) outputList.Add(EdgeIntersection(s, e, edge));
                            outputList.Add(e);
                        }
                        else
                        {
                            if (IsInside(s, edge)) outputList.Add(EdgeIntersection(s, e, edge));
                        }
                        s = e;
                    }
                }
                if (outputList.Count != 0) clippedTriangles.Add(outputList.ToArray());
            }
            return clippedTriangles;
        }

        private bool IsInside(XMVector point, string edge)
        {
            bool isInside = false;
            switch (edge)
            {
                case "left":
                    isInside = point.X >= -1;
                    break;
                case "right":
                    isInside = point.X <= 1;
                    break;
                case "bottom":
                    isInside = point.Y >= -1;
                    break;
                case "top":
                    isInside = point.Y <= 1;
                    break;
                default:
                    break;
            }
            return isInside;
        }

        private XMVector EdgeIntersection(XMVector pointA, XMVector pointB, string edge)
        {
            XMVector intersection = new XMVector(0, 0, 0, 1);
            float a = (pointA.X - pointB.X) == 0 ? 0 : (pointA.Y - pointB.Y) / (pointA.X - pointB.X);
            float b = pointA.Y - a * pointA.X;
            switch (edge)
            {
                case "left":
                    intersection.X = -1;
                    intersection.Y = -a + b;
                    break;
                case "right":
                    intersection.X = 1;
                    intersection.Y = a + b;
                    break;
                case "bottom":
                    intersection.Y = -1;
                    intersection.X = a == 0 ? pointA.X : (-1 - b) / a;
                    break;
                case "top":
                    intersection.Y = 1;
                    intersection.X = a == 0 ? pointA.X : (1 - b) / a;
                    break;
                default:
                    break;
            }
            return intersection;
        }

        private void DrawFrustum(float left, float right, float top, float bottom, float near, XMMatrix cameraMatrix)
        {
            _frustum[0] = XMVector3.Transform(new XMVector(left, top, -near, 1), cameraMatrix);
            _frustum[1] = XMVector3.Transform(new XMVector(right, top, -near, 1), cameraMatrix);
            _frustum[2] = XMVector3.Transform(new XMVector(right, bottom, -near, 1), cameraMatrix);
            _frustum[3] = XMVector3.Transform(new XMVector(left, bottom, -near, 1), cameraMatrix);
            _frustum[4] = XMVector3.Transform(new XMVector(0, 0, 0, 1), cameraMatrix);
            _frustum[5] = XMVector3.Transform(new XMVector(left, top, -near, 1), cameraMatrix);
            _frustum[6] = XMVector3.Transform(new XMVector(0, 0, 0, 1), cameraMatrix);
            _frustum[7] = XMVector3.Transform(new XMVector(right, top, -near, 1), cameraMatrix);
            _frustum[8] = XMVector3.Transform(new XMVector(0, 0, 0, 1), cameraMatrix);
            _frustum[9] = XMVector3.Transform(new XMVector(right, bottom, -near, 1), cameraMatrix);
            _frustum[10] = XMVector3.Transform(new XMVector(0, 0, 0, 1), cameraMatrix);
            _frustum[11] = XMVector3.Transform(new XMVector(left, bottom, -near, 1), cameraMatrix);
        }

        private void Reproject(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Perspective" || e.PropertyName == "ViewAngle") Project();
        }
    }
}
