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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItVitaeAssenstelsel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private variables
        private bool firstClick = true;
        private Point middenPunt;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = PointToScreen(Mouse.GetPosition(this));
            TextBkBeeld.Text = string.Format("({0}, {1})", mousePos.X, mousePos.Y);
        }

        private void CanvasMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePosCanvas = Mouse.GetPosition(this);
            if (firstClick)
            {
                //Set middenPunt
                middenPunt = mousePosCanvas;
                TextBkMiddenP.Text = string.Format("({0}, {1})", 0, 0);

                //Create and add lines to CanvasMain.
                //Start at 0 end at PrimaryScreen Width/Height to ensure the line will cross the entire window.
                Line lineHor = new Line
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    X1 = 0,
                    X2 = SystemParameters.PrimaryScreenWidth,
                    Y1 = mousePosCanvas.Y,
                    Y2 = mousePosCanvas.Y
                };
                Line lineVer = new Line
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    X1 = mousePosCanvas.X,
                    X2 = mousePosCanvas.X,
                    Y1 = 0,
                    Y2 = SystemParameters.PrimaryScreenHeight
                };
                //Add both lines to canvas.
                CanvasMain.Children.Add(lineHor);
                CanvasMain.Children.Add(lineVer);

                firstClick = false;
            }

            //Set Wiskundige coordinaten. Middenpunt - mouseCanvas positie. 
            TextBkWis.Text = string.Format("({0:0}, {1:0})", middenPunt.X - mousePosCanvas.X, middenPunt.Y - mousePosCanvas.Y);
        }
    }
}
