using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Petzold.Media3D;

namespace Test_Wires
{
    public partial class MainWindow : Window
    {
        Point3D eyePoint = new Point3D (8,8,16);

        AmbientLight     ambient  = new AmbientLight ((Color)ColorConverter.ConvertFromString ("#808080"));
        DirectionalLight dir      = new DirectionalLight ((Color)ColorConverter.ConvertFromString ("#808080"), new Vector3D (2, -3, -1));
        Model3DGroup     lighting = new Model3DGroup ();
        ModelVisual3D    lightingVisual = new ModelVisual3D ();

        public MainWindow ()
        {
            InitializeComponent ();

            Axes axes = new Axes ();
            axes.LargeTick = 0.1;
            axes.SmallTick = 0;
            axes.ShowNumbers = true;
            axes.Extent = 5;
            axes.FontSize = 0.2;

            //RedLine ();
            Helix ();

            lighting.Children.Add (ambient);
            lighting.Children.Add (dir);
            lightingVisual.Content = lighting;

            // camera
            view.Camera = new PerspectiveCamera (eyePoint,
                                                 -1 * new Vector3D (eyePoint.X, eyePoint.Y, eyePoint.Z),
                                                 new Vector3D (0, 1, 0),
                                                 45);
            view.Children.Add (axes);
            view.Children.Add (lightingVisual);
        }

        //*****************************************************************

        void Helix ()
        {
            Point3DCollection points = new Point3DCollection ();

            for (double theta = 0; theta < 720; theta += 10)
            {
                Point3D pt = new Point3D (Math.Cos (theta * Math.PI/180), Math.Sin (theta * Math.PI/180), theta/360);
                points.Add (pt);
            }

            WirePolyline poly = new WirePolyline ();
            poly.Points = points;
            poly.Color = Colors.Red;
            poly.Thickness = 3;
            poly.Decimation = 2;

            view.Children.Add (poly);
        }

        //*****************************************************************

        void RedLine ()
        {
            WireLine l1 = new WireLine ();
            l1.Point1 = new Point3D (0.1,0.1,0.1);
            l1.Point2 = new Point3D (0,5,1);
//          l1.Point2 = new Point3D (-1,5,5);
            l1.Color = Colors.Red;
            l1.Thickness = 3;
            l1.ArrowEnds = Petzold.Media2D.ArrowEnds.End;

            // one tick, at middle of l1
            Vector3D v1 = l1.Point2 - l1.Point1;
            Point3D tickAt = l1.Point1 + v1 / 2; 
            Vector3D tickDir = Vector3D.CrossProduct (v1, tickAt - eyePoint);
            tickDir.Normalize ();

            WireLine l2 = new WireLine ();
            l2.Point1 = tickAt - tickDir / 10;
            l2.Point2 = tickAt + tickDir / 10;
            l2.Color = Colors.Red;
            l2.Thickness = 2;

            // some text
            TextGenerator gen = new TextGenerator ();
            gen.FontSize = 1;
            gen.Origin = tickAt;
            gen.BaselineDirection = v1;
            gen.UpDirection = tickDir;
            Point3DCollection textStrokes = new Point3DCollection ();
            gen.Generate (textStrokes, "RSTUV");

            WireLines lines = new WireLines ();
            lines.Lines = textStrokes;

            view.Children.Add (l1);
            view.Children.Add (l2);
            view.Children.Add (lines);
        }
    }
}
