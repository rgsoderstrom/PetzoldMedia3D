using System.Windows;
using System.Collections.Generic;

using System.Windows.Media;
using System.Windows.Media.Media3D;

using Petzold.Media3D;

namespace Test_Wires2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point3D eyePoint = new Point3D (15,17,9);

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
            axes.Extent = 12;
            axes.FontSize = 0.2;

         //   axes.LineCollection.

            AxisAngleRotation3D AAR = new AxisAngleRotation3D (new Vector3D (1,1,1), 20);

            Box (new ScaleTransform3D (1,2,3), new RotateTransform3D (AAR), new TranslateTransform3D (1,2,3), Colors.Red);

            lighting.Children.Add (ambient);
            lighting.Children.Add (dir);
            lightingVisual.Content = lighting;

            // camera
            view.Camera = new PerspectiveCamera (eyePoint,
                                                 -1 * new Vector3D (eyePoint.X, eyePoint.Y, eyePoint.Z),
                                                 new Vector3D (0, 1, 0),
                                                 40);
            view.Children.Add (axes);
            view.Children.Add (lightingVisual);
        }

        //*****************************************************************

        void Box (ScaleTransform3D size, RotateTransform3D orientation, TranslateTransform3D position, Color color)
        {
            double s = 0.5;
            double t = 2; // wire thickness

            List<Point3D> top    = new List<Point3D> () {new Point3D (s,s,s),  new Point3D (-s,s,s),  new Point3D (-s,-s,s),  new Point3D (s,-s,s)};
            List<Point3D> bottom = new List<Point3D> () {new Point3D (s,s,-s), new Point3D (-s,s,-s), new Point3D (-s,-s,-s), new Point3D (s,-s,-s)};

            List<WireLine> lines = new List<WireLine> (12);

            WireLine line;
            line = new WireLine (); line.Point1 = top [0]; line.Point2 = top [1]; line.Color = color; line.Thickness = t; lines.Add (line);
            line = new WireLine (); line.Point1 = top [1]; line.Point2 = top [2]; line.Color = color; line.Thickness = t; lines.Add (line);
            line = new WireLine (); line.Point1 = top [2]; line.Point2 = top [3]; line.Color = color; line.Thickness = t; lines.Add (line);
            line = new WireLine (); line.Point1 = top [3]; line.Point2 = top [0]; line.Color = color; line.Thickness = t; lines.Add (line);

            line = new WireLine (); line.Point1 = bottom [0]; line.Point2 = bottom [1]; line.Color = color; line.Thickness = t; lines.Add (line);
            line = new WireLine (); line.Point1 = bottom [1]; line.Point2 = bottom [2]; line.Color = color; line.Thickness = t; lines.Add (line);
            line = new WireLine (); line.Point1 = bottom [2]; line.Point2 = bottom [3]; line.Color = color; line.Thickness = t; lines.Add (line);
            line = new WireLine (); line.Point1 = bottom [3]; line.Point2 = bottom [0]; line.Color = color; line.Thickness = t; lines.Add (line);

            line = new WireLine (); line.Point1 = bottom [0]; line.Point2 = top [0]; line.Color = color; line.Thickness = t; lines.Add (line);
            line = new WireLine (); line.Point1 = bottom [1]; line.Point2 = top [1]; line.Color = color; line.Thickness = t; lines.Add (line);
            line = new WireLine (); line.Point1 = bottom [2]; line.Point2 = top [2]; line.Color = color; line.Thickness = t; lines.Add (line);
            line = new WireLine (); line.Point1 = bottom [3]; line.Point2 = top [3]; line.Color = color; line.Thickness = t; lines.Add (line);

          

            ModelVisual3D lineVisual = new ModelVisual3D ();

            foreach (Visual3D l in lines)
                lineVisual.Children.Add (l);

            lineVisual.Transform = new Transform3DGroup ();
            (lineVisual.Transform as Transform3DGroup).Children.Add (size);
            (lineVisual.Transform as Transform3DGroup).Children.Add (orientation);
            (lineVisual.Transform as Transform3DGroup).Children.Add (position);

            view.Children.Add (lineVisual);
        }
    }
}
