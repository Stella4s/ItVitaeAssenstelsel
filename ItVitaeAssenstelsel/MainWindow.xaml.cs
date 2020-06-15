using System;
using System.Collections.Generic;
using System.Globalization;
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
        private int rasterOffSet = 10;
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
                middenPunt =  mousePosCanvas;
                DrawGrid(rasterOffSet, middenPunt, CanvasMain);
                TextBkMiddenP.Text = string.Format("({0}, {1})", 0, 0);


               // firstClick = false;
            }

            //Set Wiskundige coordinaten. Middenpunt - mouseCanvas positie. 
            TextBkWis.Text = string.Format("({0:0}, {1:0})", middenPunt.X - mousePosCanvas.X, middenPunt.Y - mousePosCanvas.Y);
        }

        public void DrawGrid(int offSet, Point middenP, Canvas mainCanvas)
        {
            RemoveGrid(mainCanvas);
            Image lines = new Image();
            lines.SetValue(Panel.ZIndexProperty, -100);
            //Draw the grid
            DrawingVisual gridLinesVisual = new DrawingVisual();
            DrawingContext dct = gridLinesVisual.RenderOpen();
            Pen darkPen = new Pen(Brushes.DarkOrange, 1), lightPen = new Pen(Brushes.Orange, 0.5),
                mainPen = new Pen(Brushes.Red, 1);
            darkPen.Freeze();
            lightPen.Freeze();
            mainPen.Freeze();

            int yOffset = offSet,
                xOffset = offSet,
                rows = (int)(SystemParameters.PrimaryScreenHeight),
                columns = (int)(SystemParameters.PrimaryScreenWidth),
                //Alternate makes after each 10 lines the tenth be drawn with darkPen.
                alternate = 10,
                j = 0;

            //Draw the horizontal lines
            Point a = new Point(0, AdjustingPoint(middenP.Y, mainCanvas.ActualHeight));
            Point b = new Point(SystemParameters.PrimaryScreenWidth, AdjustingPoint(middenP.Y, mainCanvas.ActualHeight));

            for (int i = 0; i <= rows; i++, j++)
            {
                dct.DrawLine(a.Y == (Math.Round(middenP.Y) + 0.5) ? mainPen : (j % alternate == 0 ? darkPen : lightPen), a, b);
                a.Offset(0, yOffset);
                b.Offset(0, yOffset);
            }
            j = 0;

            //Draw the vertical lines
            a = new Point(AdjustingPoint(middenP.X, mainCanvas.ActualWidth), 0);
            b = new Point(AdjustingPoint(middenP.X, mainCanvas.ActualWidth), SystemParameters.PrimaryScreenHeight);

            for (int i = 0; i <= columns; i++, j++)
            {
                if (j % alternate == 0)
                {
                    AddGridText(dct, j + ((int)((AdjustingPoint(middenP.X, mainCanvas.ActualWidth) + 0.5) / 10) * 2), a.X, (Math.Round(middenP.Y) + 0.5));
                }
                dct.DrawLine(a.X == (Math.Round(middenP.X) + 0.5) ? mainPen : (j % alternate == 0 ? darkPen : lightPen), a, b);
                a.Offset(xOffset, 0);
                b.Offset(xOffset, 0);
            }

            AddGridText(dct, 0, 0, 0);

            dct.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)SystemParameters.PrimaryScreenWidth,
                (int)SystemParameters.PrimaryScreenHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(gridLinesVisual);
            bmp.Freeze();
            lines.Source = bmp;

            mainCanvas.Children.Add(lines);
        }

        private void AddGridText(DrawingContext drawingContext, int number, double x, double y)
        {
            FormattedText formattedText = new FormattedText(
                number.ToString(),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"),
                10,
                Brushes.ForestGreen);

            drawingContext.DrawText(formattedText, new Point(x, y));
        }

        /// <summary>
        /// To use with making new Points to use with drawingLines in DrawGrid.
        /// Using rounded actualheight to have the horizontal lines reach from top to bottom canvas without offsetting them from main lines.
        /// Adding + 0.5 to prevent grid from being blurry.
        /// </summary>
        /// <param name="point">Based off the middenpoint for the 0,0 coordinates.</param>
        /// <param name="Canvas">To send Canvas.actualHeight or actualWidth accordingly.</param>
        /// <returns></returns>
        private double AdjustingPoint(double point, double Canvas)
        {
            return Math.Round(point - (Math.Round((Canvas / 100)) * 100) - 100) + 0.5;
        }

        private void RemoveGrid(Canvas mainCanvas)
        {
            foreach (UIElement obj in mainCanvas.Children)
            {
                if (obj is Image)
                {
                    mainCanvas.Children.Remove(obj);
                    break;
                }
            }
        }
    }
}
