using JeremyAnsel.DirectX.DXMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SceneBuilder.View
{
    class DragAndDrop
    {
        #region "Movable"
        public static readonly DependencyProperty MovableProperty =
            DependencyProperty.RegisterAttached("Movable", typeof(bool), typeof(DragAndDrop),
            new UIPropertyMetadata(false, OnObserveChanged));

        public static bool GetMovable(FrameworkElement elem)
        {
            return (bool)elem.GetValue(MovableProperty);
        }

        public static void SetMovable(FrameworkElement elem, bool value)
        {
            elem.SetValue(MovableProperty, value);
        }

        static void OnObserveChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement elem = depObj as FrameworkElement;

            if (elem == null || e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
            {
                elem.MouseUp += OnMouseUp;
                elem.MouseDown += OnMouseDown;
                (elem.Parent as FrameworkElement).MouseMove += OnMouseMove;
            }
            else
            {
                elem.MouseUp -= OnMouseUp;
                elem.MouseDown -= OnMouseDown;
                (elem.Parent as FrameworkElement).MouseMove -= OnMouseMove;
            }
        }

        private static void OnMouseMove(object sender, MouseEventArgs e)
        {
            var canvas = sender as FrameworkElement;
            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(canvas); i++)
            {
                var elem = VisualTreeHelper.GetChild(canvas, i) as DependencyObject;
                if (GetIsMoving(elem))
                {
                    var position = Mouse.GetPosition(canvas);
                    SetPosition(elem, new XMVector((float)position.X, (float)position.Y, 0, 1));
                }
            }
        }

        private static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            SetIsMoving(sender as DependencyObject, true);
        }

        private static void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            SetIsMoving(sender as DependencyObject, false);
        }
        #endregion

        #region "Position"
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.RegisterAttached("Position", typeof(XMVector), typeof(DragAndDrop), new UIPropertyMetadata(new XMVector(0,0,0,1)));

        public static XMVector GetPosition(DependencyObject obj)
        {
            return (XMVector)obj?.GetValue(PositionProperty);
        }

        public static void SetPosition(DependencyObject obj, XMVector value)
        {
            obj?.SetValue(PositionProperty, value);
        }
        #endregion

        #region "IsMoving"
        public static readonly DependencyProperty IsMovingProperty =
            DependencyProperty.RegisterAttached("IsMoving", typeof(bool), typeof(DragAndDrop), new UIPropertyMetadata(false));

        public static bool GetIsMoving(DependencyObject obj)
        {
            return (bool)obj?.GetValue(IsMovingProperty);
        }

        public static void SetIsMoving(DependencyObject obj, bool value)
        {
            obj?.SetValue(IsMovingProperty, value);
        }
        #endregion

    }
}
