﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : Window , INotifyPropertyChanged
    {
        #region private variables
        private bool firstClick = true;
        private bool _MakeNewGrid = false;
        private Point middenPunt;
        private int rasterOffSet = 10;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region Properties
        public bool MakeNewGrid
        {
            get { return _MakeNewGrid; }
            set
            {
                _MakeNewGrid = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = PointToScreen(Mouse.GetPosition(this));
            TextBkBeeld.Text = string.Format("({0}, {1})", mousePos.X, mousePos.Y);
        }

        private void CanvasMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePosCanvas = Mouse.GetPosition(this);
            if (MakeNewGrid || firstClick)
            {
                //Set middenPunt and save it to compare other coordinates to.
                middenPunt =  mousePosCanvas;
                DrawGrid(rasterOffSet, middenPunt, CanvasMain);
                TextBkMiddenP.Text = string.Format("({0}, {1})", 0, 0);
                firstClick = false;
            }

            //Set Wiskundige coordinaten. Middenpunt - mouseCanvas positie. 
            TextBkWis.Text = string.Format("({0:0}, {1:0})", (middenPunt.X - mousePosCanvas.X) / 10, (middenPunt.Y - mousePosCanvas.Y) / 10);
        }

        /// <summary>
        /// Called to draw and redraw the grid. Including numbers.
        /// </summary>
        /// <param name="offSet">The pixeloffset between lines.</param>
        /// <param name="middenP">The point in the middle.</param>
        /// <param name="mainCanvas">The canvas it needs to be drawn on.</param>
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

            //Make the points for the horizontal lines.
            Point a = new Point(0, AdjustingPoint(middenP.Y, mainCanvas.ActualHeight));
            Point b = new Point(SystemParameters.PrimaryScreenWidth, AdjustingPoint(middenP.Y, mainCanvas.ActualHeight));


            //Offset for nummeric lables in grid.
            //Calculated based on how many columns/rows in the actual midpoint will be reached.
            //Converting to and from a Decimal to allow for rounding up when the numberoffset is in the 10's when dividing by 100 and should not result in 0.
            int numOffset = (((int)a.Y * -1) + (int)middenP.Y - rows);
            numOffset = ((int)Math.Round((decimal)numOffset / 100)) * 10 + 10;
            //+ 10 at the end only for rows. For some reason there was a 10 offset, which I could not figure out what caused it 
            //It might be the rasteroffsset or window thickness, but I am not certain and could not determine for sure.

            //Draw Horizontal lines.
            for (int i = 0; i <= rows; i++, j++)
            {
                if (j % alternate == 0)
                {
                    //If statement so it won't add two 0's in the middle.
                    if (a.Y != (Math.Round(middenP.Y) + 0.5))
                    AddGridText(dct, (rows/ 100) * 10 - j + numOffset, (Math.Round(middenP.X) + 0.5),  a.Y + 1);
                }
                dct.DrawLine(a.Y == (Math.Round(middenP.Y) + 0.5) ? mainPen : (j % alternate == 0 ? darkPen : lightPen), a, b);
                a.Offset(0, yOffset);
                b.Offset(0, yOffset);
            }
            j = 0;

            //Make points for the vertical lines
            a = new Point(AdjustingPoint(middenP.X, mainCanvas.ActualWidth), 0);
            b = new Point(AdjustingPoint(middenP.X, mainCanvas.ActualWidth), SystemParameters.PrimaryScreenHeight);

            numOffset = ((int)a.X * -1) + (int)middenP.X - columns;
            numOffset = ((int)Math.Round((decimal)numOffset / 100)) * 10;

            //Draw vertical lines.
            for (int i = 0; i <= columns; i++, j++)
            {
                //For each 10 line mark add a number indicator.
                if (j % alternate == 0)
                {
                    AddGridText(dct, (columns / 100) * 10 - j + numOffset, a.X + 1, (Math.Round(middenP.Y) + 0.5));
                }
                dct.DrawLine(a.X == (Math.Round(middenP.X) + 0.5) ? mainPen : (j % alternate == 0 ? darkPen : lightPen), a, b);
                a.Offset(xOffset, 0);
                b.Offset(xOffset, 0);
            }

            dct.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)SystemParameters.PrimaryScreenWidth,
                (int)SystemParameters.PrimaryScreenHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(gridLinesVisual);
            bmp.Freeze();
            lines.Source = bmp;

            mainCanvas.Children.Add(lines);
        }

        /// <summary>
        /// Formats each number in the grid.
        /// </summary>
        /// <param name="drawingContext">The drawingContext it should be drawn on.</param>
        /// <param name="number">The number it should draw.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
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

        /// <summary>
        /// Simple method to remove the grid before redrawing.
        /// </summary>
        /// <param name="mainCanvas">Canvas which the grid needs to be removed off.</param>
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

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
