using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

using Petzold.Media3D;

namespace Driver
{
    public partial class MainWindow : Window
    {
        MeshGeometry3D  meshGeometry  = new MeshGeometry3D ();
        GeometryModel3D geometryModel = new GeometryModel3D ();
        ModelVisual3D   visual = null;

        // transform for model rotation 
        AxisAngleRotation3D AAR = new AxisAngleRotation3D ();
        RotateTransform3D   rot = new RotateTransform3D ();

        AmbientLight     ambient  = new AmbientLight ((Color)ColorConverter.ConvertFromString ("#808080"));
        DirectionalLight dir      = new DirectionalLight ((Color)ColorConverter.ConvertFromString ("#808080"), new Vector3D (2, -3, -1));
        Model3DGroup     lighting = new Model3DGroup ();
        ModelVisual3D    lightingVisual = new ModelVisual3D ();

        public MainWindow ()
        {
            try
            {
                InitializeComponent ();

                //          CylinderMesh shape = new CylinderMesh ();
                //                HollowCylinderMesh shape = new HollowCylinderMesh ();
                //SphereMesh shape = new SphereMesh ();

                //CubeMesh shape = new CubeMesh ();
                TetrahedronMesh shape = new TetrahedronMesh ();

                shape.Slices = 12;
             //   shape.Stacks = 12;
//              shape.Length = 3;
             //   shape.Radius = 2;
                
                
                meshGeometry = shape.Geometry; // mesh triangles




                /*
                for (int i=0; i<meshGeometry.Positions.Count; i++)
                {
                    Point3D p = meshGeometry.Positions [i];
                    p.X *= (5 - p.Z) / 2;
                    p.Y *= (5 - p.Z) / 2;
                    meshGeometry.Positions [i] = p;
                }

                foreach (Point3D pt in meshGeometry.Positions)
                    Console.WriteLine ("{0}", pt);

                Console.WriteLine ("{0}", meshGeometry.Positions.Count);
                */



                MaterialGroup grp = new MaterialGroup ();
                grp.Children.Add (new DiffuseMaterial (Brushes.Gray));
                grp.Children.Add (new SpecularMaterial (Brushes.Gray, 50));

                // mesh plus material
                geometryModel.Geometry = meshGeometry;
                geometryModel.Material = grp;
                geometryModel.BackMaterial = new DiffuseMaterial (Brushes.DarkGray);


                bool showSolid = false;

                if (showSolid)
                { 
                    visual = new ModelVisual3D ();
                    visual.Content = geometryModel;
                }
                else
                {
                    visual = new WireFrame ();
                    (visual as WireFrame).Positions = meshGeometry.Positions;
                    (visual as WireFrame).TriangleIndices = meshGeometry.TriangleIndices;
                }
                
                visual.Transform = new Transform3DGroup ();
                rot.Rotation = AAR;
                (visual.Transform as Transform3DGroup).Children.Add (rot);

                view.Children.Add (visual);

                //******************************************************************************************

                lighting.Children.Add (ambient);
                lighting.Children.Add (dir);
                lightingVisual.Content = lighting;

                // camera
                view.Camera = new PerspectiveCamera (new Point3D (10, 10, 5),
                                                     new Vector3D (-10,-10,-5),
                                                     new Vector3D (0, 0, 1),
                                                     55);
                view.Children.Add (lightingVisual);

                // animation
                AAR.Axis = new Vector3D (0,0,1);//(1,1,1);
                DoubleAnimation anima = new DoubleAnimation (360, new Duration (TimeSpan.FromSeconds (20)));
                anima.RepeatBehavior = RepeatBehavior.Forever;
                anima.AutoReverse = true;
                AAR.BeginAnimation (AxisAngleRotation3D.AngleProperty, anima);
                
       //         Int32Animation anima2 = new Int32Animation (5, 15, new Duration (TimeSpan.FromSeconds (2)));
         //       anima2.RepeatBehavior = RepeatBehavior.Forever;
           //     anima2.AutoReverse = true;               
      //          sm.BeginAnimation (SphereMeshGenerator2.FirstStackProperty, anima2);
                
   //             DoubleAnimation anima3 = new DoubleAnimation (1, 2, new Duration (TimeSpan.FromSeconds (6)));
     //           anima3.Completed += Anima3_Completed;
       //         anima3.RepeatBehavior = RepeatBehavior.Forever;
         //       anima3.AutoReverse = true;               
           //     sm.BeginAnimation (SphereMeshGenerator2.RadiusProperty, anima3);
                
            }

            catch (Exception ex)
            {
                Console.WriteLine ("Exception: ", ex.Message);
                Console.WriteLine ("Stack: ", ex.StackTrace);
            }
        }

        private void Anima3_Completed (object sender, EventArgs e)
        {
            Console.WriteLine ("Done");
        }
    }
}
