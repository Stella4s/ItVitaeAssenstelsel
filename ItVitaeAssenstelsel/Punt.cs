using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ItVitaeAssenstelsel
{
    public class Punt : INotifyPropertyChanged
    {
        #region private properties
        double _wisX, _wisY, _beeldX, _beeldY;
        string _raster;
        int _dikteP, _breedteRand;
        Brush _kleurP, _kleurRand;
        #endregion

        #region public properties
        public double WisX
        {
            get { return _wisX; }
            set
            {
                _wisX = value;
                OnPropertyChanged();
            }
        }
        public double WisY
        {
            get { return _wisY; }
            set
            {
                _wisY = value;
                OnPropertyChanged();
            }
        }
        public double BeeldX
        {
            get { return _beeldX; }
            set
            {
                 _beeldX = value;
                OnPropertyChanged();
            }
        }
        public double BeeldY
        {
            get { return _beeldY; }
            set
            {
                _beeldY = value;
                OnPropertyChanged();
            }
        }
        public int DikteP
        {
            get { return _dikteP; }
            set
            {
                _dikteP = value;
                OnPropertyChanged();
            }
        }
        public int BreedteRand
        {
            get { return _breedteRand; }
            set
            {
                _breedteRand = value;
                OnPropertyChanged();
            }
        }
        public Brush KleurP
        {
            get { return _kleurP; }
            set
            {
                _kleurP = value;
                OnPropertyChanged();
            }
        }
        public Brush KleurRand
        {
            get { return _kleurRand; }
            set
            {
                _kleurRand = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region methods
        public override string ToString()
        {
            return string.Format("Raster ({0},{1}", WisX / 10, WisY / 10);
        }
        public Ellipse DrawPoint(Point centerPoint)
        {
            Ellipse punt = new Ellipse
            {
                Height = DikteP,
                Width = DikteP,
                Fill = KleurP,
                Stroke = KleurRand,
                StrokeThickness = BreedteRand
            };
            Canvas.SetTop(punt, centerPoint.Y + (WisY * -1)  - (punt.Height / 2));
            Canvas.SetLeft(punt, centerPoint.X + (WisX * -1)  - (punt.Width / 2));

            return punt;
        }
        #endregion


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
